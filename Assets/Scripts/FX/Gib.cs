using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gib : MonoBehaviour
{
    public GameObject[] Gibs;
    public AudioClip Clip;
    void Start()
    {
        Gibs[Random.Range(0, Gibs.Length)].SetActive(true);
    }
    private void OnCollisionEnter(Collision collision) {
        AudioSource.PlayClipAtPoint(Clip,transform.position);
    }
}
