using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.UI.Text Text;
    public float TTL = 5.0f;
    private float time = 0.0f;
    void Start()
    {
        
    }
    public void SetNotification(string msg) {
        Text.text = msg;
        time = TTL;
    }
    // Update is called once per frame
    void Update()
    {
        if (time > 0.0f) {
            time -= Time.deltaTime;
            if (time <= 0.0f) {
                Text.text = "";
            }
        }
    }
}
