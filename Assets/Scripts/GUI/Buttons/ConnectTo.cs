using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectTo : MonoBehaviour
{
    public UnityEngine.UI.InputField ipAddress;


    public void OnClick() {
        var client = NetworkManager.singleton.StartClient();
        if (!ipAddress) {
            return;
        }
        client.Connect(ipAddress.text, 21111);
    }
}
