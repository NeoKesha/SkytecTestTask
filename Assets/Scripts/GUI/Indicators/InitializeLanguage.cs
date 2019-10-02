using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLanguage : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> Languages;
    void Awake()
    {
        foreach (var lang in Languages) {
            var langLines = Resources.Load<TextAsset>($"Language/Lines/{lang}");
            var langSprites = Resources.Load<TextAsset>($"Language/Sprites/{lang}");
            GlobalContext.LanguageLines.Add(lang, new Dictionary<string, string>());
            GlobalContext.LanguageSprites.Add(lang, new Dictionary<string, Sprite>());
            var linesData = langLines.text.Split('\n');
            var spritesData = langSprites.text.Split('\n');
            foreach (var data in linesData) {
                var tmp = data.Split(new string[] {" = "}, System.StringSplitOptions.None);
                var key = tmp[0];
                var value = tmp[1].Replace(@"\n","\n").TrimEnd('\n','\r');
                GlobalContext.LanguageLines[lang].Add(key, value);
            }
            foreach (var data in spritesData) {
                var tmp = data.Split(new string[] { " = " }, System.StringSplitOptions.None);
                var key = tmp[0];
                var value = Resources.Load<Sprite>(tmp[1].TrimEnd('\n', '\r'));
                GlobalContext.LanguageSprites[lang].Add(key, value);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
