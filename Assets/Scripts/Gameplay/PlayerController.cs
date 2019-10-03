
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerController : NetworkBehaviour {
    public CharacterController Controller;
    public GameObject Camera;
    public GameObject GUI;
    public GameObject VisibleBody;
    public GameObject QuadFX;
    public GameObject Shield;
    public GameObject Barrel;
    public GameObject Bullet;
    public GameObject Pile;
    public GameObject ShotFX;
    public Notification Notification;
    public UnityEngine.UI.Text NickNameText;
    public Sprite BlodstainSprite;
    public Sprite GunshotSprite;
    public GameObject LocalIndicator;
    public GameObject DeathScreen;
    public Countdown Countdown;
    public TouchControl MovementTouch;
    public TouchControl ShootingTouch;
    public Animator Animator;
    public AudioSource AudioSource;
    public AudioClip[] Clips;

    public float CharacterSpeed = 5.0f;
    public float MaxHP = 100.0f;
    public float Damage = 20.0f;

    public float SpeedMultiplyStart = 1.0f;
    public float SpeedMultiplyEnd = 2.0f;
    public float CoverVisibilityStart = 4.0f; // Distance minimum to see emeny on the edge of cover
    public float CoverVisibilityEnd = 2.0f; // Distance minimum to see emeny at center of cover

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
    private GameObject BeansCover;

    [SyncVar] private string NickName;
    private bool MyNameSent = false;

    [SyncVar] private float HP;
    [SyncVar] private int Frags;
    [SyncVar] private bool Hide;

    [SyncVar] private float Energy;
    [SyncVar] private float Invuln;
    [SyncVar] private float Quad;

    public void Start() {
        BeansCover = null;
        movementDir = Vector3.up;
        GlobalContext.Players.Add(this.gameObject);
    }

    public override void OnStartLocalPlayer() {
        GlobalContext.LocalAuthority = this.gameObject;
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
        Hide = false;
        Energy = 0.0f;
        Invuln = 0.0f;
        Quad = 0.0f;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Projectile")) {
            if (isServer) { 
                CoffeeShred shred = collision.gameObject.GetComponent<CoffeeShred>();
                if (!shred.GetDeactivated()) {
                    TakeDamage(shred.GetDamage(), shred.GetParent().GetComponent<PlayerController>());
                    shred.Deactivate();
                }
                shred.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(collision.gameObject);
            }
            var fx = Instantiate(ShotFX, collision.GetContact(0).point, transform.rotation, transform);
            var normal = collision.GetContact(0).normal;
            var x = Vector3.Angle(normal, Vector3.right) + 90;
            var y = Vector3.Angle(normal, Vector3.down);
            var z = Vector3.Angle(normal, Vector3.back);
            var s = Random.Range(1.0f, 2.0f);
            fx.transform.localScale = new Vector3(s, s, s);
            fx.transform.localRotation = Quaternion.Euler(x, y, z);
            fx.GetComponent<ShotFX>().Setup(BlodstainSprite, (GlobalContext.Settings["GORE"] == "1")?(Color.red):Color.black, 0.25f, Clips[Random.Range(1, 4)]);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Cover")) {
            BeansCover = other.gameObject;
        } else {
            if (isServer) {
                PickUp pick = other.gameObject.GetComponent<PickUp>();
                if (pick) {
                    HP += pick.GetHeal();
                    if (HP > MaxHP) HP = MaxHP;
                    Energy += pick.GetEnergy();
                    Invuln += pick.GetInvuln();
                    Quad += pick.GetQuad();
                    Destroy(other.gameObject);
                }
            }
            
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Cover")) {
            BeansCover = null;
        }
    }
    public void Update() { // Animations and visuals
        if (!Dead && HP <= 0.0f) {
            Animator.SetTrigger("die");
            Dead = true;
        } else if (Dead && HP > 0.0f) {
            Animator.SetTrigger("respawn");
            Dead = false;
            fragged = false;
            respawning = false;
        }
        if (!Dead) {
            if (BeansCover) {
                var b_dist = (BeansCover.transform.position - transform.position).magnitude;
                var p_dist = (GlobalContext.LocalAuthority.transform.position - transform.position).magnitude;
                var t = b_dist / 1.5f; //Half of width of bush + radius of player
                var m_dist = Mathf.Lerp(CoverVisibilityEnd, CoverVisibilityStart,t); // How close foe have to be to see us in bush depends on how are we close to center of bush
                if (p_dist > m_dist) {
                    VisibleBody.SetActive(false);
                } else {
                    VisibleBody.SetActive(true);
                }
            } else if (!VisibleBody.activeSelf) {
                VisibleBody.SetActive(true); // If not visible outside of bush, go visible
            }
            if (!Controller.enabled) {
                Controller.enabled = true;
            }
            // Quad damage effect
            if (Quad > 0.0f && !QuadFX.activeSelf) {
                QuadFX.SetActive(true);
            } else if (Quad <= 0.0f && QuadFX.activeSelf) {
                QuadFX.SetActive(false);
            }
            // Shield effect
            if (Invuln > 0.0f && !Shield.activeSelf) {
                Shield.SetActive(true);
            } else if (Invuln <= 0.0f && Shield.activeSelf) {
                Shield.SetActive(false);
            }
        } else {
            if (Hide && VisibleBody.activeSelf) {
                // Excecute only once
                VisibleBody.SetActive(false);
                if (GlobalContext.Settings["GORE"] == "1") Instantiate(Pile, transform.position + new Vector3(0,0.8f,0), new Quaternion());
            } else if (!Hide && !VisibleBody.activeSelf) {
                VisibleBody.SetActive(true);
            }
            if (Controller.enabled) {
                Controller.enabled = false;
            }
        }
        if (!MyNameSent) {
            if (isLocalPlayer) CmdSetMyName(GlobalContext.Settings["NAME"]); else NickNameText.text = NickName;
            MyNameSent = true;
        }
    }
    public void FixedUpdate() { // Physics and movement
        if (isLocalPlayer) {
            float t = HP / MaxHP;
            if (t == 0.0f) t = 1.0f;
            float k = Mathf.Lerp(SpeedMultiplyEnd, SpeedMultiplyStart, t); // Lower our health, more coffee we have, faster we are
            k *= (Energy > 0.0f) ? 4 : 1; // Quad speed under energetic 
            if (!Dead) {
                // Fetch controls
                movementMagnitude = MovementTouch.GetMagnitude();
                if (movementMagnitude > 0.0f) {
                    movementDir = MovementTouch.GetDirection();
                }
                movementAngle = Vector2.SignedAngle(new Vector2(movementDir.x, movementDir.y), Vector2.up);
                aimingMagnitude = ShootingTouch.GetMagnitude();
                aimingDir = ShootingTouch.GetDirection();
                aimingAngle = Vector2.SignedAngle(new Vector2(aimingDir.x, aimingDir.y), Vector2.up);
                // Simple movement
                Controller.SimpleMove(CharacterSpeed * (new Vector3(movementDir.x, 0, movementDir.y)) * movementMagnitude*k);
                // Anamation control
                bool animator_running = Animator.GetBool("running");
                bool animator_shooting = Animator.GetBool("shooting");
                Animator.SetFloat("speed", k);
                if (movementMagnitude > 0 && !animator_running) {
                    Animator.SetBool("running", true);
                } else if (animator_running && movementMagnitude == 0) {
                    Animator.SetBool("running", false);
                }
                if (!animator_shooting) {
                    if (aimingMagnitude >= 0.8) { // Manually aim
                        CmdShoot(new Vector3(aimingDir.x, 0, aimingDir.y));
                        Animator.SetBool("shooting", true);
                    } else if (ShootingTouch.GetReleased() && ShootingTouch.GetCaptured()) { // Auto aim nearest player
                        var colliders = Physics.OverlapSphere(transform.position,8,LayerMask.GetMask("Character"));
                        var nearest = float.MaxValue;
                        aimingDir = movementDir;
                        foreach (var c in colliders) { // Scan for game objects of player character
                            var go = c.gameObject;
                            if (go == gameObject)
                                continue; // Not me!
                            var dist = (go.transform.position - transform.position).magnitude;
                            if (nearest > dist) {
                                var character = go.GetComponent<PlayerController>();
                                if (character.GetVisible()) {
                                    nearest = dist;
                                    aimingDir = (go.transform.position - transform.position).normalized;
                                    aimingDir = new Vector3(aimingDir.x, aimingDir.z, 0);
                                    aimingAngle = Vector2.SignedAngle(new Vector2(aimingDir.x, aimingDir.y), Vector2.up);
                                    VisibleBody.transform.rotation = Quaternion.AngleAxis(aimingAngle, Vector3.up);
                                }
                            }
                        }
                        CmdShoot(new Vector3(aimingDir.x, 0, aimingDir.y));
                        Animator.SetBool("shooting", true);
                    }
                }
                if (aimingMagnitude == 0.0 && movementMagnitude > 0.0) {
                    VisibleBody.transform.rotation = Quaternion.AngleAxis(movementAngle, Vector3.up); // If we move, face towards movement
                } else if (aimingMagnitude > 0.3) {
                    VisibleBody.transform.rotation = Quaternion.AngleAxis(aimingAngle, Vector3.up); // If we shoot, face towards aim
                }
            } else if (Animator.GetBool("died") && !respawning) {
                respawning = true;
                StartCoroutine(WaitForRespawn());
            }
        }
        if (isServer) {
            if (Energy > 0.0f) {
                Energy -= Time.deltaTime;
                if (Energy < 0.0f) Energy = 0.0f;
            }
            if (Invuln > 0.0f) {
                Invuln -= Time.deltaTime;
                if (Invuln < 0.0f) Invuln = 0.0f;
            }
            if (Quad > 0.0f) {
                Quad -= Time.deltaTime;
                if (Quad < 0.0f) Quad = 0.0f;
            }
        }
    }

    private bool fragged = false;
    public void TakeDamage(float Damage, PlayerController attacker) {
        if (!Dead && isServer && Invuln <= 0.0f && attacker.gameObject != this.gameObject) {
            HP -= Damage;
            if (HP <= 0.0f) {
                HP = 0.0f;
                if (!fragged) {
                    attacker.AddFrag();
                    CmdBroadcastMessage($"{attacker.GetName()} [kill] {NickName}! [yes]!"); // Broadcast message to all players, that kill happened
                    fragged = true; // No double fragging, if bullet hist dead character
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
        CmdHideMe(true);
        for (int i = 0; i < RespawnTime; ++i) { // Control Countdown 
            Countdown.Tick(RespawnTime - i);
            yield return new WaitForSeconds(1.0f);
        }
        DeathScreen.SetActive(false);
        CmdHideMe(false);
        CmdRespawn();
    }

    private void PlaySound(int id) {
        AudioSource.clip = Clips[0];
        AudioSource.Play();
    }

    [Command]
    private void CmdSetMyName(string nickName) {
        NickName = nickName;
        RpcUpdateNickname(nickName);
    }

    [Command]
    private void CmdHideMe(bool hide) {
        Hide = hide;
    }
    [Command]
    private void CmdRespawn() {
        HP = MaxHP;
        Hide = false;
        gameObject.transform.position = GlobalContext.GetSpawnPoint(); // Because of this is had to set NetworkTransform to Translation instead of CharacterController Sync!
        RpcSyncYourCharacterSpawn(gameObject.transform.position);      // I tried everything.
    }

    [Command]
    private void CmdShoot(Vector3 dir) {
        var dmg = Damage / Pellets; // Divide total damage output of player by number of pellets in shot
        for (int i = 0; i < Pellets; ++i) {
            var go = Instantiate(Bullet, Barrel.transform.position, Quaternion.Euler(Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f)));
            go.GetComponent<CoffeeShred>().Init(dir, dmg, this.gameObject);
            if (Quad > 0.0f) {
                go.GetComponent<CoffeeShred>().activeQuad = true;
            }
            NetworkServer.Spawn(go);
        }
        RpcPlayGunshot();
    }
    [Command] 
    private void CmdBroadcastMessage(string msg) {
        RpcSetNotification(msg);
    }
    [ClientRpc]
    private void RpcSetNotification(string msg) {
        List<string> keys = new List<string>();
        int pos = 0;
        bool flag = true;
        // To preserve multi-language feature, we need to parse broadcasted message
        while (flag && pos < msg.Length) {
            int pos1 = msg.IndexOf('[', pos);
            int pos2 = msg.IndexOf(']', pos);
            if (pos1 != -1 && pos2 != -1 && pos1 < pos2) {
                keys.Add(msg.Substring(pos1 + 1, pos2 - 1- pos1));
            }
            if (pos1 == -1 || pos2 == -1) {
                flag = false;
            } else {
                pos = pos2 + 1;
            }
        }
        foreach(var key in keys) {
            msg = msg.Replace($"[{key}]", GlobalContext.LanguageLines[GlobalContext.Settings["LANG"]][key]);
        }
        GlobalContext.LocalAuthority.GetComponent<PlayerController>().Notification.SetNotification(msg);
    }
    [ClientRpc]
    private void RpcSyncYourCharacterSpawn(Vector3 pos) {
        gameObject.transform.position = pos;
    }
    [ClientRpc]
    private void RpcPlayGunshot() {
        var fx = Instantiate(ShotFX, Barrel.transform.position, Barrel.transform.rotation , Barrel.transform);
        fx.transform.localRotation = Quaternion.Euler(0, 0, 90);
        fx.GetComponent<ShotFX>().Setup(GunshotSprite, Color.white, 0.25f, Clips[0]);
    }
    [ClientRpc] 
    private void RpcUpdateNickname(string name) {
            NickNameText.text = name;
    }

    public int GetFrags() { return Frags; }
    public string GetName() {  return NickName; }
    public float GetHealth() { return HP; }
    public bool GetVisible() { return VisibleBody.activeSelf; }
}
