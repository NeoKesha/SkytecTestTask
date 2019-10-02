using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Leaderboards : MonoBehaviour
{
    public UnityEngine.UI.Text Text;
    // Start is called before the first frame update
    void OnEnable() {
        List<KeyValuePair<string, int>> sorted = new List<KeyValuePair<string, int>>();
        foreach (GameObject go in GlobalContext.Players) {
            PlayerController player = go.GetComponent<PlayerController>();
            sorted.Add(new KeyValuePair<string, int>(player.GetName(), player.GetFrags()));
        }
        sorted = sorted.OrderBy(x => x.Value).Reverse().ToList();
        Text.text = "";
        int i = 0;
        foreach (var player in sorted) {
            Text.text += $"{++i}) {player.Key} - {player.Value}\n";
        }
    }

}
