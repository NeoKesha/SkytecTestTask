using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsText : MonoBehaviour
{
    public string Key;
    public UnityEngine.UI.InputField Text;
    void Start() {
        Text.text = GlobalContext.Settings[Key];
    }

    public void Change() {
        GlobalContext.Settings[Key] = Text.text;
    }
}
