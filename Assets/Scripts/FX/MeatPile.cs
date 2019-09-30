using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatPile : MonoBehaviour
{
    public GameObject Gib;
    public int MinGibs = 5;
    public int MaxGibs = 7;
    public float TTL = 5.0f;

    public float DownSpeed = 0.2f;
    public float ShrinkSpeed = 0.2f;
    public float Wait = 1.0f;

    private bool shrink = false;
    private float Scale = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        int Gibs = Random.Range(MinGibs, MaxGibs + 1);
        for (int i = 0; i < Gibs; ++i) {
            var gib = Instantiate(Gib, transform.position, new Quaternion());
            gib.GetComponent<Rigidbody>().velocity = (Quaternion.Euler(Random.Range(-30,30),0, Random.Range(-30, 30))*Vector3.up) * Random.Range(5.0f, 15.0f);
            Destroy(gib, TTL);
        }
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        if (shrink) {
            Scale -= ShrinkSpeed * Time.deltaTime;
            transform.position = transform.position - new Vector3(0,DownSpeed * Time.deltaTime,0);
            transform.localScale = new Vector3(Scale, Scale, Scale);
        }
    }
    private IEnumerator FadeOut() {
        yield return new WaitForSeconds(Wait);
        shrink = true;
    }
}
