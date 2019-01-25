using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoverUIController : MonoBehaviour
{
    public RectTransform coverButtonPanel;
    public RectTransform aboutPanel;
    
    public void LocalGame()
    {
        SceneManager.LoadScene("LocalMapChoose");
    }

    public void NetworkGame()
    {
        SceneManager.LoadScene("LANConnect");
    }

    public void About()
    {
        SceneManager.LoadScene("TeamInfo");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
