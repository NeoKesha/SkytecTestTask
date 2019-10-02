using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangText : MonoBehaviour
{
    // Start is called before the first frame update
    public string Key = "";
    public UnityEngine.UI.Text Text;
    private string lang;
    void Start()
    {
        lang = GlobalContext.Settings["LANG"];
        Text.text = GlobalContext.LanguageLines[lang][Key];
    }

    // Update is called once per frame
    void Update()
    {
        //Bottleneck
        if (lang != GlobalContext.Settings["LANG"]) {
            lang = GlobalContext.Settings["LANG"];
            Text.text = GlobalContext.LanguageLines[lang][Key];
        }
        
    }
}
