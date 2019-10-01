﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 camPos = Camera.main.transform.position;
        transform.LookAt( new Vector3(transform.position.x, camPos.y, camPos.z));
        transform.Rotate(new Vector3(0, 180, 0));
    }
}
