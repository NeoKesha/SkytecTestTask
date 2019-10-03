using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFX : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer FX1;
    public SpriteRenderer FX2;

    public void Setup(Sprite spr, Color blend, float TTL, AudioClip clip) {
        if (FX1) { //Setup vertical part
            FX1.sprite = spr;
            FX1.color = blend;
        }
        if (FX2) { //Setup horizontal part
            FX2.sprite = spr;
            FX2.color = blend;
        }
        Destroy(this.gameObject, TTL); //Set time to live
        if (clip) AudioSource.PlayClipAtPoint(clip,transform.position); // If clip is set, play it
    }
}
