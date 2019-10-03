using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public float sizeStart = 3.0f;
    public float sizeEnd = 1.0f;
    public float speedMultiplier = 1.5f;

    private UnityEngine.UI.Text text;
    private float t;
    // Start is called before the first frame update
    void OnEnable()
    {
        text = gameObject.GetComponent<UnityEngine.UI.Text>();
        if (!text) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float s = Mathf.Lerp(sizeEnd,sizeStart,t); //Interpolate size 
        transform.localScale = new Vector3(s, s, s);
        if (t > 0.0f) {
            t -= Time.deltaTime * speedMultiplier;
        } else if (t < 0.0f) {
            t = 0.0f;
        }
    }
    public void Tick(int second) {
        t = 1.0f;
        text.text = second.ToString();
    }
}
