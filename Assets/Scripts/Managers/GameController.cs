using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public enum gameStatus {Play, Pause, MouseDieOver, TimeUpOver, RulerHint};

    private gameStatus currentStatus = gameStatus.Play;
    public gameStatus CurrentStatus => currentStatus;
    public int gameLevel = 1;
    private GameObject holeManager;
    private GameObject levelManager;
    private GameObject mice1, mice2;
    private float gameTime;
    private Stack<gameStatus> stateRecord = new Stack<gameStatus>();
    public int maxTime = 60;

    public Vector2[] startPos1, startPos2;
    public GameObject GameOverUI, AreaDisplayer;


    //InputManager inputManager;


    void Start(){
        if (GameObject.FindWithTag("LocalMapChoiceManager"))
        {
            var localMapChoice = GameObject.FindWithTag("LocalMapChoiceManager").GetComponent<MapChoiceManager>();
            gameLevel = localMapChoice.GetMapChosen();
        }
        Debug.Log(gameLevel);
        holeManager = GameObject.Find("HoleManager");
        holeManager.GetComponent<HoleManager>().InitializeLevel(gameLevel);
        levelManager = GameObject.Find("LevelManager");
        levelManager.GetComponent<LevelManager>().SetGameLevel(gameLevel);
        //inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        mice1 = GameObject.Find("mice1");
        mice2 = GameObject.Find("mice2");
        mice1.transform.position = startPos1[gameLevel];
        mice2.transform.position = startPos2[gameLevel];
        mice2.transform.eulerAngles = new Vector3(0, -180, 0);

        gameTime = 0f;
        
    }

    void Update() {
        if (currentStatus == gameStatus.Play){
            gameTime += Time.deltaTime;
        }
        // if ((currentStatus == gameStatus.MouseDieOver || currentStatus == gameStatus.TimeUpOver) && InputManager.instance.GetRestart()){
        //     gameTime = 0f;
        //     startGame(gameLevel);
        // }
        if (gameTime >= maxTime && currentStatus != gameStatus.TimeUpOver) {
            // send a signal of Game Over
            TimeUpGameOver();
        }
    }

    public void SetGameStatus(gameStatus status){
        currentStatus = status;
    }

    public void PauseGame()
    {
        if (currentStatus == gameStatus.Play)
        {
            Time.timeScale = 0;
            stateRecord.Push(currentStatus);
            SetGameStatus(gameStatus.Pause);
        }
    }

    public void ResumeGame()
    {
        if (currentStatus == gameStatus.Pause)
        {
            StartCoroutine(ResetTimeScale());
            SetGameStatus(stateRecord.Pop());
        }
    }

    public void ShowRulerPause()
    {
        if (currentStatus == gameStatus.Play)
        {
            Time.timeScale = 0;
            stateRecord.Push(currentStatus);
            SetGameStatus(gameStatus.RulerHint);
        }
    }

    public void ShowRulerResume()
    {
        Debug.Log("ShowRulerResume-out");
        if (currentStatus == gameStatus.RulerHint)
        {
            Debug.Log("ShowRulerResume-In");
            StartCoroutine(ResetTimeScale());
            SetGameStatus(stateRecord.Pop());
            Debug.Log(CurrentStatus);
        }
    }
    
    IEnumerator ResetTimeScale(){
        float timer = 0;
        while (timer < 1){
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1;
        yield return null;
    }

    public int GetGameTime(){
        return (int)gameTime;
    }

    public void RestartGame(){
        Time.timeScale = 1;
        SceneManager.LoadScene("LocalGame");
    }

    public void SelectLevel(){
        Time.timeScale = 1;
        SceneManager.LoadScene("LocalMapChoose");
    }

    public void MouseDieGameOver(int playerID){
        stateRecord.Push(currentStatus);
        SetGameStatus(GameController.gameStatus.MouseDieOver);
        if (playerID == 1) {
            GameOverUI.transform.GetChild(2).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(1).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(3).gameObject.SetActive(false);
        } else {
            GameOverUI.transform.GetChild(1).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(2).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(3).gameObject.SetActive(false);
        }
        GameOverUI.transform.localPosition = new Vector2(0, 0);
        GameOverUI.transform.GetChild(10).gameObject.SetActive(false);
        GameOverUI.transform.GetChild(9).gameObject.SetActive(false);
        GameOverUI.GetComponent<Animator>().SetTrigger("GameOver");
        AreaDisplayer.GetComponent<AreaDisplayerUI>().Display();
    }

    public void TimeUpGameOver(){
        stateRecord.Push(currentStatus);
        SetGameStatus(GameController.gameStatus.TimeUpOver);
        int area1 = Mathf.Min((int)holeManager.GetComponent<HoleManager>().areas[1]/20, 9999);
        int area2 = Mathf.Min((int)holeManager.GetComponent<HoleManager>().areas[2]/20, 9999);
        if (area1 == area2) {
            GameOverUI.transform.GetChild(3).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(1).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(2).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(8).gameObject.SetActive(false);
        } else if (area1 > area2) {
            GameOverUI.transform.GetChild(1).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(2).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(3).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(9).gameObject.SetActive(false);
        } else {
            GameOverUI.transform.GetChild(2).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(1).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(3).gameObject.SetActive(false);
            GameOverUI.transform.GetChild(9).gameObject.SetActive(false);
        }
        GameOverUI.transform.localPosition = new Vector2(0, 0);
        GameOverUI.GetComponent<Animator>().SetTrigger("GameOver");
        AreaDisplayer.GetComponent<AreaDisplayerUI>().Display();
    }

}
