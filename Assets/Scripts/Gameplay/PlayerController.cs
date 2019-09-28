using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerController : NetworkBehaviour
{
    // Start is called before the first frame update
    public CharacterController Controller;
    public GameObject Camera;
    public GameObject VisibleBody;
    public GameObject Barrel;
    public GameObject Bullet;
    public GameObject LocalIndicator;
    public TouchControl MovementTouch;
    public TouchControl ShootingTouch;
    public Animator Animator;

    public float CharacterSpeed = 5.0f;
    public float MaxHP = 100.0f;
    public float Damage = 20.0f;
    public int Pellets = 7;

    private float movementAngle;
    private float movementMagnitude;
    private Vector3 movementDir;
    private float aimingAngle;
    private float aimingMagnitude;
    private Vector3 aimingDir;
    

    void Start()
    {
        if (isLocalPlayer) {
            Camera.SetActive(true);
            LocalIndicator.SetActive(true);
        }
    }

    // Update is called once per frame
    

    void Update()
    {
        if (isLocalPlayer) {
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
                    Shoot();
                    Animator.SetTrigger("shoot");
                } else if (ShootingTouch.GetReleased()) {
                    //shoot aimed
                    Animator.SetTrigger("shoot");
                }
            }
            if (aimingMagnitude == 0.0 && movementMagnitude > 0.0) {
                VisibleBody.transform.rotation = Quaternion.AngleAxis(movementAngle, Vector3.up);
            } else if (aimingMagnitude > 0.3) {
                VisibleBody.transform.rotation = Quaternion.AngleAxis(aimingAngle, Vector3.up);
            }
        }
    }
    private void Shoot() {
        for (int i = 0; i < Pellets; ++i) {
            var go = Instantiate(Bullet, Barrel.transform.position, Quaternion.Euler(Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f)));
            go.GetComponent<CoffeeShred>().Init(new Vector3(aimingDir.x, 0, aimingDir.y), Damage, this.gameObject);
            NetworkServer.Spawn(go);
        }
    }
}
