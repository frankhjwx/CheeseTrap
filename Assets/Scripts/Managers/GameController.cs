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
    public float maxTime = 60f;

    public Vector2[] startPos1, startPos2;
    public GameObject GameOverUI, TimeUpUI;


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
        if ((currentStatus == gameStatus.MouseDieOver || currentStatus == gameStatus.TimeUpOver) && InputManager.instance.GetRestart()){
            gameTime = 0f;
            startGame(gameLevel);
        }
        if (gameTime >= maxTime) {
            // send a signal of Game Over
            TimeUpGameOver();
        }
    }

    public void SetGameStatus(gameStatus status){
        currentStatus = status;
    }

    public int GetGameTime(){
        return (int)gameTime;
    }

    public void startGame(int level){
        /*
        holeManager.GetComponent<HoleManager>().InitializeLevel(level);
        currentStatus = gameStatus.Play;
        gameTime = 0f;
        mice1.transform.position = startPos1[gameLevel];
        mice2.transform.position = startPos2[gameLevel];
        mice2.transform.eulerAngles = new Vector3(0, -180, 0);
        */
        SceneManager.LoadScene("LocalGame");
    }

    public void MouseDieGameOver(int playerID){
        SetGameStatus(GameController.gameStatus.MouseDieOver);
        string msg;
        if (playerID == 1) {
            msg = "Player 2 wins!";
        } else {
            msg = "Player 1 wins!";
        }
        GameOverUI.transform.GetChild(1).GetComponent<Text>().text = msg;
        GameOverUI.transform.position = new Vector2(960, 540);
    }

    public void TimeUpGameOver(){
        SetGameStatus(GameController.gameStatus.TimeUpOver);
        string msg;
        if (holeManager.GetComponent<HoleManager>().areas[1] > holeManager.GetComponent<HoleManager>().areas[2]) {
            msg = "Player 1 wins!";
        } else {
            msg = "Player 2 wins!";
        }
        TimeUpUI.transform.GetChild(1).GetComponent<Text>().text = msg;
        TimeUpUI.transform.GetChild(2).GetComponent<Text>().text = "P1 Area:" + holeManager.GetComponent<HoleManager>().areas[1].ToString() + "\nP2 Area:" + holeManager.GetComponent<HoleManager>().areas[2].ToString();
        TimeUpUI.transform.position = new Vector2(960, 540);
    }
}
