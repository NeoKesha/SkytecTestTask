using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSwitch : MonoBehaviour
{
    public GameObject From = null;
    public GameObject To = null;
    public void OnClick() {
        if (To) To.SetActive(true);
        if (From) From.SetActive(false);
    }
}
