using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverBehavior : MonoBehaviour
{
    enum GameOverNext
    {
        Null,
        Back,
        Next
    }

    public GameController gameController;
    private AudioManager audioManager;
    private GameOverNext currentSelected = GameOverNext.Null;
    public Animator backHighlight;
    public Animator nextHighlight;
    public float navigationTimeGap = 0.3f;
    private float navigationCount = 0.0f;
    private bool usingHandle = false;

    private void Start()
    {
        
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        if ((Mathf.Abs(Input.GetAxis("Mouse X")) >= 0.01f || Mathf.Abs(Input.GetAxis("Mouse Y")) >= 0.01f) && usingHandle)
        {
            AllUnhighlight();
            usingHandle = false;
            Cursor.visible = true;
        }
        if (gameController.currentStatus == GameController.gameStatus.TimeUpOver || gameController.currentStatus == GameController.gameStatus.MouseDieOver)
        {
            if ((Input.GetAxis("P1 Navigation Vertical") < -0.01f || Input.GetAxis("P2 Navigation Vertical") < -0.01f)
                && navigationCount >= navigationTimeGap)
            {
                Cursor.visible = false;
                usingHandle = true;
                NaviNext();
                audioManager.PlayOnceAudioByPath("audio/buttonOnEnter");
                navigationCount -= navigationTimeGap;
            }
            else if ((Input.GetAxis("P1 Navigation Vertical") > 0.01f ||
                      Input.GetAxis("P2 Navigation Vertical") > 0.01f)
                     && navigationCount >= navigationTimeGap)
            {
                Cursor.visible = false;
                usingHandle = true;
                NaviPrevious();
                audioManager.PlayOnceAudioByPath("audio/buttonOnEnter");
                navigationCount -= navigationTimeGap;
            }
            else if (Input.GetAxis("P1 Navigation Vertical") < -0.01f 
                     || Input.GetAxis("P2 Navigation Vertical") < -0.01f 
                     || Input.GetAxis("P1 Navigation Vertical") > 0.01f 
                     || Input.GetAxis("P2 Navigation Vertical") > 0.01f)
            {
                navigationCount += Time.deltaTime;
            }
            else
            {
                navigationCount = navigationTimeGap;
            }
        
            if (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit"))
            {
                switch (currentSelected)
                {
                    case GameOverNext.Next:
                        gameController.RestartGame();
                        audioManager.PlayOnceAudioByPath("audio/buttonOnClick");
                        break;
                    case GameOverNext.Back:
                        SelectLevel();
                        audioManager.PlayOnceAudioByPath("audio/buttonOnClick");
                        break;
                }
            }
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
    
    void NaviNext()
    {
        AllUnhighlight();
        switch (currentSelected)
        {
            case GameOverNext.Null:
                currentSelected = GameOverNext.Next;
                nextHighlight.SetTrigger("Highlighted");
                break;
            case GameOverNext.Next:
                currentSelected = GameOverNext.Back;
                backHighlight.SetTrigger("Highlighted");
                break;
            case GameOverNext.Back:
                currentSelected = GameOverNext.Next;
                nextHighlight.SetTrigger("Highlighted");
                break;
        }
    }

    void NaviPrevious()
    {
        AllUnhighlight();
        switch (currentSelected)
        {
            case GameOverNext.Null:
                currentSelected = GameOverNext.Back;
                backHighlight.SetTrigger("Highlighted");
                break;
            case GameOverNext.Next:
                currentSelected = GameOverNext.Back;
                backHighlight.SetTrigger("Highlighted");
                break;
            case GameOverNext.Back:
                currentSelected = GameOverNext.Next;
                nextHighlight.SetTrigger("Highlighted");
                break;
        }
    }
    
    void AllUnhighlight()
    {
        backHighlight.SetTrigger("Normal");
        nextHighlight.SetTrigger("Normal");
    }
}
