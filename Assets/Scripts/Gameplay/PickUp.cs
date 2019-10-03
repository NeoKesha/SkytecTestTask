using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUp : NetworkBehaviour
{
    public GameObject[] Items;
    private static int pickUps = 0;

    // Start is called before the first frame update
    private float heal = 0.0f;
    private float energyTime = 0.0f;
    private float invulnTime = 0.0f;
    private float quadTime = 0.0f;

    [SyncVar] private int pickId = -1;
    private bool modelSet = false;
    private void OnDestroy() {
        --pickUps;
    }

    public void Spawn(float healAmount, float energySeconds, float invulnSeconds, float quadSeconds, int id) {
        heal = healAmount;
        energyTime = energySeconds;
        invulnTime = invulnSeconds;
        quadTime = quadSeconds;
        pickId = id;
        ++pickUps;
    }

    private void Update() {
        if (!modelSet) {
            if (pickId >= 0 && pickId < Items.Length) {
                Items[pickId].SetActive(true);
                modelSet = true;
            }
        }
    }
    public float GetHeal() { return heal; }
    public float GetEnergy() { return energyTime; }
    public float GetInvuln() { return invulnTime; }
    public float GetQuad() { return quadTime; }
    public static int GetCount() { return pickUps; }

}
