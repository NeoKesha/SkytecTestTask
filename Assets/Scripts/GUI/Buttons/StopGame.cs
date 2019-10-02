using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StopGame : NetworkBehaviour
{
    public string MainMenu = "001_MainMenu";
    public void OnClick() {
        if (isServer) {
            MyNetworkManager.singleton.StopServer();
        } else {
            MyNetworkManager.singleton.StopClient();
        }
        GlobalContext.Players.Clear();
        SceneManager.LoadScene(MainMenu);
    }
}
