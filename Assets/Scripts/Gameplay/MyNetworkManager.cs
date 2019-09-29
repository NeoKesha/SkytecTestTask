using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{

    public override void OnStartServer() {
        base.OnStartServer();
        //var c = NetworkServer.connections;
        //var lc = NetworkServer.localConnections;
        //int a = 0;
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        GameObject player = (GameObject)Instantiate(playerPrefab, GlobalContext.GetSpawnPoint(), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
