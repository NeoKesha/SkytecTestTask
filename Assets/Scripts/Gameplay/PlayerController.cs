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
    public TouchControl MovementTouch;
    public TouchControl ShootingTouch;
    public float CharacterSpeed = 5.0f;
    void Start()
    {
        if (isLocalPlayer) {
            Camera.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer) {
            float moveMag = MovementTouch.GetMagnitude() / 128;
            Vector3 moveDir = MovementTouch.GetDirection();
            Controller.SimpleMove(CharacterSpeed * (new Vector3(moveDir.x, 0, moveDir.y)) * moveMag);
        }
    }
}
