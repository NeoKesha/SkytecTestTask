using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class StartServer : MonoBehaviour
{
    // Start is called before the first frame update
    public NetworkManager NetworkManager;
    
    public void OnClick() {
        GlobalContext.isServer = true;
        NetworkManager.StartHost();
    }
}
