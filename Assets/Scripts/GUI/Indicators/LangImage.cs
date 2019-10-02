using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangImage : MonoBehaviour
{
    // Start is called before the first frame update
    public string Key = "";
    public UnityEngine.UI.Image Image;
    private string lang;
    void Start() {
        lang = GlobalContext.Settings["LANG"];
        Image.sprite = GlobalContext.LanguageSprites[lang][Key];
    }

    // Update is called once per frame
    void Update() {
        //Bottleneck
        if (lang != GlobalContext.Settings["LANG"]) {
            lang = GlobalContext.Settings["LANG"];
            Image.sprite = GlobalContext.LanguageSprites[lang][Key];
        }

    }
}
