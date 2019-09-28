using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public float SizeStart = 3.0f;
    public float SizeEnd = 1.0f;
    public float SpeedMultiplier = 1.5f;

    public UnityEngine.UI.Text Text;
    private float T;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float s = SizeStart * T + SizeEnd * (1.0f - T);
        transform.localScale = new Vector3(s, s, s);
        if (T > 0.0f) {
            T -= Time.deltaTime * SpeedMultiplier;
        } else if (T < 0.0f) {
            T = 0.0f;
        }
    }
    public void Tick(int second) {
        T = 1.0f;
        Text.text = second.ToString();
    }
}
