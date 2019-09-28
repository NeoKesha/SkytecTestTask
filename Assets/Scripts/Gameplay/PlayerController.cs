
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerController : NetworkBehaviour {
    public CharacterController Controller;
    public GameObject Camera;
    public GameObject GUI;
    public GameObject VisibleBody;
    public GameObject Barrel;
    public GameObject Bullet;
    public GameObject LocalIndicator;
    public GameObject DeathScreen;
    public Countdown Countdown;
    public TouchControl MovementTouch;
    public TouchControl ShootingTouch;
    public Animator Animator;

    public float CharacterSpeed = 5.0f;
    public float MaxHP = 100.0f;
    public float Damage = 20.0f;
    public int RespawnTime = 3;
    public int Pellets = 8;

    private float movementAngle;
    private float movementMagnitude;
    private Vector3 movementDir;
    private float aimingAngle;
    private float aimingMagnitude;
    private Vector3 aimingDir;
    private bool respawning;
    private bool Dead;

    [SyncVar] private float HP;
    [SyncVar] private int Frags;

    void Start() {
        //REFACTOR
        Physics.IgnoreLayerCollision(9, 9);
        //REFACTOR
    }

    public override void OnStartLocalPlayer() {
        Camera.SetActive(true);
        LocalIndicator.SetActive(true);
        GUI.SetActive(true);
        respawning = false;
    }
    public override void OnStartServer() {
        base.OnStartServer();
        HP = MaxHP;
        Frags = 0;
        Dead = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Projectile") && isServer) {
            CoffeeShred shred = collision.gameObject.GetComponent<CoffeeShred>();
            TakeDamage(shred.GetDamage(), shred.GetParent().GetComponent<PlayerController>());
            Destroy(collision.gameObject);
        }
    }

    public void FixedUpdate() {
        if (isLocalPlayer) {
            if (!Dead) {
                movementMagnitude = MovementTouch.GetMagnitude();
                movementDir = MovementTouch.GetDirection();
                movementAngle = Vector2.SignedAngle(new Vector2(movementDir.x, movementDir.y), Vector2.up);
                aimingMagnitude = ShootingTouch.GetMagnitude();
                aimingDir = ShootingTouch.GetDirection();
                aimingAngle = Vector2.SignedAngle(new Vector2(aimingDir.x, aimingDir.y), Vector2.up);
                Controller.SimpleMove(CharacterSpeed * (new Vector3(movementDir.x, 0, movementDir.y)) * movementMagnitude);
                bool animator_running = Animator.GetBool("running");
                bool animator_shooting = Animator.GetBool("shooting");
                if (movementMagnitude > 0 && !animator_running) {
                    Animator.SetBool("running", true);
                } else if (animator_running && movementMagnitude == 0) {
                    Animator.SetBool("running", false);
                }
                if (!animator_shooting) {
                    if (aimingMagnitude >= 0.8) {
                        CmdShoot(new Vector3(aimingDir.x, 0, aimingDir.y));
                        Animator.SetBool("shooting", true);
                    } else if (ShootingTouch.GetReleased()) {
                        //shoot aimed
                        Animator.SetBool("shooting", true);
                    }
                }
                if (aimingMagnitude == 0.0 && movementMagnitude > 0.0) {
                    VisibleBody.transform.rotation = Quaternion.AngleAxis(movementAngle, Vector3.up);
                } else if (aimingMagnitude > 0.3) {
                    VisibleBody.transform.rotation = Quaternion.AngleAxis(aimingAngle, Vector3.up);
                }
            } else if (Animator.GetBool("died") && !respawning) {
                respawning = true;
                StartCoroutine(WaitForRespawn());
            }
        }
        if (!Dead && HP <= 0.0f) {
            Animator.SetTrigger("die");
            Dead = true;
        } else if (Dead && HP > 0.0f) {
            Animator.SetTrigger("respawn");
            Dead = false;
            fragged = false;
            respawning = false;
        }
    }

    private bool fragged = false;
    public void TakeDamage(float Damage, PlayerController attacker) {
        if (!Dead && isServer) {
            HP -= Damage;
            if (HP <= 0.0f) {
                HP = 0.0f;
                if (!fragged) {
                    attacker.AddFrag();
                    fragged = true;
                }
                
            }
        }
    }

    public void AddFrag() {
        if (isServer) {
            ++Frags;
        }
    }
    private IEnumerator WaitForRespawn() {
        DeathScreen.SetActive(true);
        for (int i = 0; i < RespawnTime; ++i) {
            Countdown.Tick(RespawnTime - i);
            yield return new WaitForSeconds(1.0f);
        }
        DeathScreen.SetActive(false);
        CmdRespawn();
    }
    [Command]
    private void CmdRespawn() {
        HP = MaxHP;
        gameObject.transform.position = GlobalContext.GetSpawnPoint(); // Because of this is had to set NetworkTransform to Translation instead of CharacterController Sync!
        RpcSyncYourCharacterSpawn(gameObject.transform.position);
    }

    [Command]
    private void CmdShoot(Vector3 dir) {
        for (int i = 0; i < Pellets; ++i) {
            var go = Instantiate(Bullet, Barrel.transform.position, Quaternion.Euler(Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f)));
            go.GetComponent<CoffeeShred>().Init(dir, Damage / Pellets, this.gameObject);
            NetworkServer.Spawn(go);
        }
    }

    [ClientRpc]
    private void RpcSyncYourCharacterSpawn(Vector3 pos) {
        gameObject.transform.position = pos;
    }

    public int GetFrags() { return Frags; }
    public float GetHealth() { return HP; }
}
