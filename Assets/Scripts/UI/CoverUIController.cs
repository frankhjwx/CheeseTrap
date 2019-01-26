using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoverUIController : MonoBehaviour
{
    public RectTransform comingSoonPanel;

    
    public void LocalGame()
    {
        SceneManager.LoadScene("LocalMapChoose");
    }

    public void NetworkGame()
    {
        comingSoonPanel.gameObject.SetActive(true);
        StartCoroutine(DelayDisappear(0.6f));
    }

    public void About()
    {
        SceneManager.LoadScene("TeamInfo");
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator DelayDisappear(float delay)
    {
        yield return new WaitForSeconds(delay);
        comingSoonPanel.gameObject.SetActive(false);
    }
}
