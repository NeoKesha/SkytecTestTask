using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public PlayerController TiedPlayer;
    public UnityEngine.UI.Text HPLabel;
    public Vector3[] Beats = new Vector3[4];
    private int BeatsCnt = 4;
    public float MinSpeedMultiplier = 1.0f;
    public float MaxSpeedMultiplier = 3.0f;
    private float T = 0.0f;

    // Start is called before the first frame update
    void Start() {
        BeatsCnt = Beats.Length;
    }

    // Update is called once per frame
    void Update()  {
        T -= Mathf.Floor(T);
        float t = T * BeatsCnt;
        t -= Mathf.Floor(t);
        int i = Mathf.FloorToInt(T * BeatsCnt);
        transform.localScale = Beats[i]*(1.0f-t) + Beats[(i + 1) % BeatsCnt]*t;
        T += Time.deltaTime * GetMultiplier();

        HPLabel.text = Mathf.FloorToInt(TiedPlayer.GetHealth()).ToString();
    }

    private float GetMultiplier() {
        float HP = TiedPlayer.GetHealth()/TiedPlayer.MaxHP;
        if (HP <= 0.0f) {
            return 0.0f;
        } else {
            return MinSpeedMultiplier * HP + MaxSpeedMultiplier * (1.0f - HP);
        }
    }
}
