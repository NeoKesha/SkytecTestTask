﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SwitchScene : MonoBehaviour
{
    public string Destination;
    public void OnClick() {
        SceneManager.LoadSceneAsync(Destination);
    }
}
