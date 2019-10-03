using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gib : MonoBehaviour
{
    private List<GameObject> gibs;
    public AudioClip Clip;
    void Start()
    {
        gibs = new List<GameObject>();
        for (int i = 0; i < transform.childCount; ++i) { // Get all childs and find possible gib meshes 
            if (transform.GetChild(i).gameObject.name.StartsWith("Gib")) {
                gibs.Add(transform.GetChild(i).gameObject); 
            }
        }
        gibs[Random.Range(0, gibs.Count)].SetActive(true);
    }
    private void OnCollisionEnter(Collision collision) {
        AudioSource.PlayClipAtPoint(Clip,transform.position);
    }
}
