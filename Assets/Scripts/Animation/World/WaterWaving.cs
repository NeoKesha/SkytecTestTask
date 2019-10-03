using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWaving : MonoBehaviour
{
    private MeshRenderer waterRenderer;
    public float waterWaveUAmp = 0.5f;
    public float waterWaveVAmp = 0.0f;
    public float phaseShift = 0.0f;
    public float speed = 3.0f;

    private float t;
    private int textureId;
    private Vector2 offset;
    // Start is called before the first frame update
    void Start()  {
        t = 0.0f;
        waterRenderer = GetComponent<MeshRenderer>(); // Get MeshRenderer component
        if (!waterRenderer) {
            enabled = false; // If component is not found, don't update script
        }
        waterRenderer.material.EnableKeyword("Offset");
    }

    // Update is called once per frame
    void Update() {
        t += Time.deltaTime;
        offset.Set(Mathf.Cos(t * speed) * waterWaveUAmp, Mathf.Sin(t * speed) * waterWaveVAmp); 
        waterRenderer.material.SetTextureOffset("_MainTex",offset); // Won't work because of LWRP i guess and that's sad... TODO something with this
    }
}
