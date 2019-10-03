using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{

    public override void OnStartServer() {
        base.OnStartServer();
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        GameObject player = (GameObject)Instantiate(playerPrefab, GlobalContext.GetSpawnPoint(), Quaternion.identity); // Spawn player, using Global Spawner
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
