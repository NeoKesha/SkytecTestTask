using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController Controller;
    public TouchControl MovementTouch;
    public TouchControl ShootingTouch;
    public float CharacterSpeed = 5.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveMag = MovementTouch.GetMagnitude()/128;
        Vector3 moveDir = MovementTouch.GetDirection();
        Controller.SimpleMove(CharacterSpeed * (new Vector3(moveDir.x,0,moveDir.y))*moveMag);
    }
}
