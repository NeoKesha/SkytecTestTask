using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset LevelMap;
    public GameObject W_Block;
    public GameObject F_Block;
    public GameObject L_Block;
    public GameObject B_Block;
    void Start() {
        string[] lines = LevelMap.text.Replace("\r\n", "\n").Split('\n');
        int width = 0;
        int height = lines.Length;
        float stride = 2.0f;
        foreach (var l in lines) {
            width = Mathf.Max(width, l.Length);
        }
        float shift_x = -width * stride / 2.0f;
        float shift_z = -height * stride / 2.0f;
        int x = 0;
        int z = 0;
        foreach (var l in lines) {
            foreach (var c in l) {
                GameObject sel = null;
                switch (c) {
                    case 'W':
                        sel = W_Block;
                        break;
                    case 'F':
                        sel = F_Block;
                        break;
                    case 'L':
                        sel = L_Block;
                        break;
                    case 'B':
                        sel = B_Block;
                        break;
                }
                if (sel) {
                    Instantiate(sel, new Vector3(shift_x + x * stride, 0, shift_z + z * stride), new Quaternion());
                }
                ++x;
            }
            x = 0;
            ++z;
        }
    }
}
