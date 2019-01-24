﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalMapChoiceUI : MonoBehaviour
{
    public void StartGame()
    {
        GameObject mapChoice = GameObject.FindWithTag("LocalMapChoiceManager");
        if (mapChoice != null)
        {
            MapChoiceManager mapChoiceManager = mapChoice.GetComponent<MapChoiceManager>();
            SceneManager.LoadScene("LocalGame");
        }
    }

    public void BackCover()
    {
        SceneManager.LoadScene("Cover");
    }
}
