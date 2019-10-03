using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatPile : MonoBehaviour
{
    public GameObject gib;
    public int minGibs = 5;
    public int maxGibs = 7;
    public float TTL = 5.0f;

    public float downSpeed = 0.2f;
    public float shrinkSpeed = 0.2f;
    public float wait = 1.0f;

    private bool shrink = false;
    private float scale = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (!gib) gameObject.SetActive(false); // If Gib prefab is missing, shut down
        int gibs = Random.Range(minGibs, maxGibs + 1); // Select number of gibs to create
        for (int i = 0; i < gibs; ++i) {
            var newGib = Instantiate(gib, transform.position, new Quaternion());
            newGib.GetComponent<Rigidbody>().velocity = (Quaternion.Euler(Random.Range(-30,30),0, Random.Range(-30, 30))*Vector3.up) * Random.Range(5.0f, 15.0f); //Propell gib upwards
            Destroy(newGib, TTL);
        }
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        if (shrink) {
            scale -= shrinkSpeed * Time.deltaTime;
            transform.position = transform.position - new Vector3(0,downSpeed * Time.deltaTime,0); // Soak into floor
            transform.localScale = new Vector3(scale, scale, scale);
            if (scale <= 0.01f) Destroy(this.gameObject); // If pile is too small, destroy it.
        }
    }
    private IEnumerator FadeOut() {
        yield return new WaitForSeconds(wait); // Wait until pile will start to disappear
        shrink = true;
    }
}
