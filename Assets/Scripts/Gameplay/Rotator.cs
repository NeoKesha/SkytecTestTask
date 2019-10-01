using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 RotationSpeed = new Vector3(0,30,0);
    void Start()
    {
        transform.Rotate(new Vector3(0,Random.Range(0,360),0));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotationSpeed * Time.deltaTime);
    }
}
