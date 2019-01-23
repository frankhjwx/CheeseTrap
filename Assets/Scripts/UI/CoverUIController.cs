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
        SceneManager.LoadScene("LocalGame");
    }

    public void NetworkGame()
    {
        SceneManager.LoadScene("LANConnect");
    }

    public void About()
    {
        aboutPanel.gameObject.SetActive(true);
        coverButtonPanel.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void AboutBack()
    {
        aboutPanel.gameObject.SetActive(false);
        coverButtonPanel.gameObject.SetActive(true);
    }
}
