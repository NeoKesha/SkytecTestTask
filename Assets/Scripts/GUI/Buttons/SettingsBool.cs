using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsBool : MonoBehaviour
{
    public string Key;
    public string Value1;
    public Sprite Value1Sprite;
    public string Value2;
    public Sprite Value2Sprite;
    public UnityEngine.UI.Image SpriteRenderer;
    private bool Val;
    void Start() {
        Val = GlobalContext.Settings[Key] == Value2;
    }

    // Update is called once per frame
    void Update() {
        SpriteRenderer.sprite = (Val) ? Value2Sprite : Value1Sprite;
        SpriteRenderer.enabled = SpriteRenderer.sprite;
    }
    public void Toggle() {
        Val = !Val;
        GlobalContext.Settings[Key] = (Val) ? Value2 : Value1;
    }
}
