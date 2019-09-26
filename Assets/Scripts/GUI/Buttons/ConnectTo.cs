using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectTo : MonoBehaviour
{
    public UnityEngine.UI.InputField IpAddress;
    public NetworkManager NetworkManager;

    public void OnClick() {
        var client = NetworkManager.StartClient();
        client.Connect(IpAddress.text, 21111);
    }
}
