using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadFX : MonoBehaviour {
    public Sprite spark;
    public GameObject FX;
    public AudioClip snd;
    public float rate = 0.2f;
    // Update is called once per frame
    private void Start() {
        if (!spark || !FX) {
            Destroy(this.gameObject); // If essentials are not set, shudown
        }
    }
    void Update()
    {
        if (Random.Range(0,1) < rate) { //Randomly create spark
            int cnt = Random.Range(1, 4); 
            for (int i = 0; i < cnt; ++i) {
                var s = Random.Range(0.5f, 2.0f);
                var fx = Instantiate(FX, transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), transform);
                fx.transform.localScale = new Vector3(s, s, s);
                fx.GetComponent<ShotFX>().Setup(spark, Color.red, Random.Range(0.05f, 0.15f), snd); // If snd is null, FX going to handle it itself
            }
        }
    }
}
