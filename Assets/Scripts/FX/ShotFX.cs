using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFX : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer FX1;
    public SpriteRenderer FX2;
    public void Setup(Sprite spr, Color blend, float TTL) {
        FX1.sprite = spr;
        FX1.color = blend;
        FX2.sprite = spr;
        FX2.color = blend;
        Destroy(this.gameObject, TTL);
    }
}
