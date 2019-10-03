using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUpSpawner : NetworkBehaviour
{
    // Start is called before the first frame update
    public GameObject pickup;
    public float minSpawnRate = 15.0f;
    public float maxSpawnRate = 45.0f;
    public int maxPickUps = 5;
    public int startingPickUps = 3;
    public float[] spawnRates;
    public Vector4[] itemAbilities; // Energy, Invuln, Quad

    private float cooldown;
    void Start() {
        if (!isServer) {
            gameObject.SetActive(false); //Make sure we don't have spawner on client's side
            return;
        }
        for (int i = 0; i < startingPickUps; ++i) {
            CmdSpawnPickUp(); //Spawn start pickups
        }
        SetCooldown(); //Randomly set wait time
    }
    // Update is called once per frame
    void Update() {
        if (cooldown > 0.0f) {
            cooldown -= Time.deltaTime;
        }
        if (cooldown <= 0.0f && PickUp.GetCount() < maxPickUps) {
            SetCooldown(); //When it's time, set neww cooldown
            CmdSpawnPickUp(); //Spawn nwe pickup
        }
    }

    private void SetCooldown() {
        cooldown = Random.Range(minSpawnRate, maxSpawnRate);
    }

    [Command]
    public void CmdSpawnPickUp() {
        var id = GetRandomId(); // Get random pickup ID
        var pickup = Instantiate(this.pickup, GlobalContext.GetSpawnPoint(), new Quaternion()); // Instantiate pickup
        pickup.GetComponent<PickUp>().Spawn(itemAbilities[id].x, itemAbilities[id].y, itemAbilities[id].z, itemAbilities[id].w, id);  // Initialize it
        NetworkServer.Spawn(pickup);// And register it 
    }
    private int GetRandomId() {
        float len = 0.0f;
        for (int i = 0; i < spawnRates.Length; ++i) {
            len += spawnRates[i];
        }
        float X = Random.Range(0.0f, len);
        float shift = 0.0f;
        //Check, what pickup id range we hit
        for (int i = 0; i < spawnRates.Length; ++i) {
            if (i == 0) {
                if (X < spawnRates[0]) {
                    return 0;
                }
            } else if (i == spawnRates.Length - 1) {
                if (X > spawnRates[spawnRates.Length - 1]) {
                    return spawnRates.Length - 1;
                }
            } else {
                if (X > shift && X < spawnRates[i+1] + shift) {
                    return i;
                }
            }
            shift += spawnRates[0];
        }
        return spawnRates.Length - 1;
    }
}
