using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public enum gameStatus {Play, Pause, MouseDieOver, TimeUpOver};
    public gameStatus currentStatus;
    public int gameLevel = 1;
    private GameObject holeManager;
    private GameObject levelManager;
    private GameObject mice1, mice2;
    private float gameTime;
    public int maxTime = 60;

    public Vector2[] startPos1, startPos2;
    public GameObject GameOverUI, AreaDisplayer;
    public InGamePauseUI pauseUi;


    //InputManager inputManager;


    void Start(){
        if (GameObject.FindWithTag("LocalMapChoiceManager"))
        {
            var localMapChoice = GameObject.FindWithTag("LocalMapChoiceManager").GetComponent<MapChoiceManager>();
            gameLevel = localMapChoice.GetMapChosen();
        }
        Debug.Log(gameLevel);
        currentStatus = gameStatus.Play;
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

        if (currentStatus == gameStatus.Play)
        {
            if (Input.GetButtonDown("P1 Pause") || Input.GetButtonDown("P2 Pause"))
            {
                pauseUi.Pause();
                SetGameStatus(gameStatus.Pause);
            }
        }
        if (currentStatus == gameStatus.TimeUpOver || currentStatus == gameStatus.MouseDieOver)
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
    }

    public void SetGameStatus(gameStatus status){
        currentStatus = status;
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
        SetGameStatus(GameController.gameStatus.MouseDieOver);
        if (playerID == 2) {
            GameOverUI.transform.Find("p1Win").gameObject.SetActive(true);
            GameOverUI.transform.Find("p2Win").gameObject.SetActive(false);
            GameOverUI.transform.Find("Tie").gameObject.SetActive(false);
            GameOverUI.transform.Find("p1WinBG").gameObject.SetActive(true);
            GameOverUI.transform.Find("p2WinBG").gameObject.SetActive(false);
            GameOverUI.transform.Find("p1Die").gameObject.SetActive(false);
        } else {
            GameOverUI.transform.Find("p2Win").gameObject.SetActive(true);
            GameOverUI.transform.Find("p1Win").gameObject.SetActive(false);
            GameOverUI.transform.Find("Tie").gameObject.SetActive(false);
            GameOverUI.transform.Find("p2WinBG").gameObject.SetActive(true);
            GameOverUI.transform.Find("p1WinBG").gameObject.SetActive(false);
            GameOverUI.transform.Find("p2Die").gameObject.SetActive(false);
        }
        GameOverUI.transform.localPosition = new Vector2(0, 0);
        GameOverUI.transform.Find("TieMice").gameObject.SetActive(false);
        GameOverUI.transform.Find("TimeUp").gameObject.SetActive(false);
        GameOverUI.GetComponent<Animator>().SetTrigger("GameOver");
        AreaDisplayer.GetComponent<AreaDisplayerUI>().Display();
    }

    public void TimeUpGameOver(){
        SetGameStatus(GameController.gameStatus.TimeUpOver);
        int area1 = Mathf.Min((int)holeManager.GetComponent<HoleManager>().areas[1]/20, 9999);
        int area2 = Mathf.Min((int)holeManager.GetComponent<HoleManager>().areas[2]/20, 9999);
        if (area1 == area2) {
            GameOverUI.transform.Find("Tie").gameObject.SetActive(true);
            GameOverUI.transform.Find("p1Win").gameObject.SetActive(false);
            GameOverUI.transform.Find("p2Win").gameObject.SetActive(false);
            GameOverUI.transform.Find("WinMice").gameObject.SetActive(false);
            GameOverUI.transform.Find("p1WinBG").gameObject.SetActive(false);
            GameOverUI.transform.Find("p2WinBG").gameObject.SetActive(false);
            GameOverUI.transform.Find("Area").gameObject.SetActive(false);
        } else if (area1 > area2) {
            GameOverUI.transform.Find("p1Win").gameObject.SetActive(true);
            GameOverUI.transform.Find("p2Win").gameObject.SetActive(false);
            GameOverUI.transform.Find("Tie").gameObject.SetActive(false);
            GameOverUI.transform.Find("p1WinBG").gameObject.SetActive(true);
            GameOverUI.transform.Find("p2WinBG").gameObject.SetActive(false);
            GameOverUI.transform.Find("TieMice").gameObject.SetActive(false);
        } else {
            GameOverUI.transform.Find("p2Win").gameObject.SetActive(true);
            GameOverUI.transform.Find("p1Win").gameObject.SetActive(false);
            GameOverUI.transform.Find("Tie").gameObject.SetActive(false);
            GameOverUI.transform.Find("p2WinBG").gameObject.SetActive(true);
            GameOverUI.transform.Find("p1WinBG").gameObject.SetActive(false);
            GameOverUI.transform.Find("TieMice").gameObject.SetActive(false);
        }
        GameOverUI.transform.Find("p1Die").gameObject.SetActive(false);
        GameOverUI.transform.Find("p2Die").gameObject.SetActive(false);
        GameOverUI.transform.localPosition = new Vector2(0, 0);
        GameOverUI.GetComponent<Animator>().SetTrigger("GameOver");
        AreaDisplayer.GetComponent<AreaDisplayerUI>().Display();
    }

}
