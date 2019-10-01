using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpSpawner : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject Pickup;
    public float MinSpawnRate = 15.0f;
    public float MaxSpawnRate = 45.0f;
    public int MaxPickUps = 5;
    public int StartingPickUps = 3;
    public float[] SpawnRates;
    public Vector4[] ItemAbilities; // Energy, Invuln, Quad

    private float cooldown;
    void Start() {
        if (!isServer) {
            gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < StartingPickUps; ++i) {
            CmdSpawnPickUp();
        }
        SetCooldown();
    }

    // Update is called once per frame
    void Update() {
        if (cooldown > 0.0f) {
            cooldown -= Time.deltaTime;
        }
        if (cooldown <= 0.0f && PickUp.GetCount() < MaxPickUps) {
            SetCooldown();
            CmdSpawnPickUp();
        }
    }

    private void SetCooldown() {
        cooldown = Random.Range(MinSpawnRate, MaxSpawnRate);
    }

    [Command]
    public void CmdSpawnPickUp() {
        var id = GetRandomId();
        var pickup = Instantiate(Pickup, GlobalContext.GetSpawnPoint(), new Quaternion());
        pickup.GetComponent<PickUp>().Spawn(ItemAbilities[id].x, ItemAbilities[id].y, ItemAbilities[id].z, ItemAbilities[id].w, id);
        NetworkServer.Spawn(pickup);
    }
    private int GetRandomId() {
        float len = 0.0f;
        for (int i = 0; i < SpawnRates.Length; ++i) {
            len += SpawnRates[i];
        }
        float X = Random.Range(0.0f, len);
        float shift = 0.0f;
        for (int i = 0; i < SpawnRates.Length; ++i) {
            if (i == 0) {
                if (X < SpawnRates[0]) {
                    return 0;
                }
            } else if (i == SpawnRates.Length - 1) {
                if (X > SpawnRates[SpawnRates.Length - 1]) {
                    return SpawnRates.Length - 1;
                }
            } else {
                if (X > shift && X < SpawnRates[i+1] + shift) {
                    return i;
                }
            }
            shift += SpawnRates[0];
        }
        return SpawnRates.Length - 1;
    }
}
