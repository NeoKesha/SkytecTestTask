using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public static class GlobalContext
{
    public static List<Vector3> SpawnPoints = new List<Vector3>();
    public static bool isServer = false;
    public static GameObject LocalAuthority;
    public static List<GameObject> Players;
    public static Dictionary<string, string> Settings;

    static GlobalContext() {
        Players = new List<GameObject>();
        Screen.SetResolution(992, 537, false);
        Application.targetFrameRate = 60;
        Physics.IgnoreLayerCollision(9, 9);
        Physics.IgnoreLayerCollision(9, 11);
        Physics.IgnoreLayerCollision(10, 11);
        Physics.IgnoreLayerCollision(11, 11);
        Physics.IgnoreLayerCollision(12, 11);
        Settings = new Dictionary<string, string>();
        Settings.Add("LANG", "RU");
        Settings.Add("GORE", "1");
        Settings.Add("CHARACTER", "0");
        Settings.Add("NAME", "Player");
    }
    public static Vector3 GetSpawnPoint() {
        Vector3 shift = new Vector3(0, 1.5f, 0);
        int i = Random.Range(0, SpawnPoints.Count);
        int j = SpawnPoints.Count;
        Vector3 spawn = SpawnPoints[i];
        while (Physics.OverlapSphere(spawn + shift, 0.75f).Length > 0 && j > 0) {            
            i = (i + 1) % SpawnPoints.Count;
            spawn = SpawnPoints[i];
            --j; // If ALL cells are not free, we could go into infinite cycle.
        }
        return spawn;
    }
}
