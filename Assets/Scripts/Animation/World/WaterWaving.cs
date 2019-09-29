using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaving : MonoBehaviour
{
    public MeshRenderer WaterRenderer;
    public float WaterWaveUAmp = 0.5f;
    public float WaterWaveVAmp = 0.0f;
    public float PhaseShift = 0.0f;
    public float Speed = 3.0f;

    private float T;
    private int TextureId;
    private Vector2 offset;
    // Start is called before the first frame update
    void Start()  {
        T = 0.0f;
        WaterRenderer.material.EnableKeyword("Offset");
    }

    // Update is called once per frame
    void Update() {
        T += Time.deltaTime;
        offset.Set(Mathf.Cos(T * Speed) * WaterWaveUAmp, Mathf.Sin(T * Speed) * WaterWaveVAmp);
        WaterRenderer.material.SetTextureOffset("_MainTex",offset); // Won't work because of LWRP i guess and that's sad... TODO something with this
    }
}
