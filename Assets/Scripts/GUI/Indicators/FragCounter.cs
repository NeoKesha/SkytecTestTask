using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController Player;
    public GameObject Image;
    public GameObject Text;
    private UnityEngine.UI.Text TextText;
    void Start() {
        TextText = Text.GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var frags = Player.GetFrags();
        TextText.text = frags.ToString();
        if (frags >= 1 && !Image.activeSelf) {
            Image.SetActive(true);
        }
        if (frags >= 2 && !Text.activeSelf) {
            Text.SetActive(true);
        }
    }
}
