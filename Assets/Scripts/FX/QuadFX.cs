using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadFX : MonoBehaviour
{
    public Sprite Spark;
    public GameObject FX;
    public AudioClip Snd;
    public float Rate = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0,1) < Rate) {
            int cnt = Random.Range(1, 4);
            for (int i = 0; i < cnt; ++i) {
                var s = Random.Range(0.5f, 2.0f);
                var fx = Instantiate(FX, transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), transform);
                fx.transform.localScale = new Vector3(s, s, s);
                fx.GetComponent<ShotFX>().Setup(Spark, Color.red, Random.Range(0.05f, 0.15f), Snd);
            }
        }
    }
}
