using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverBehavior : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit"))
        {
            RestartGame();
        }

        if (Input.GetButtonDown("P1 Cancel") || Input.GetButtonDown("P2 Cancel"))
        {
            SelectLevel();
        }
    }
    
    public void RestartGame(){
        Time.timeScale = 1;
        SceneManager.LoadScene("LocalGame");
    }

    public void SelectLevel(){
        Time.timeScale = 1;
        SceneManager.LoadScene("LocalMapChoose");
    }
}
