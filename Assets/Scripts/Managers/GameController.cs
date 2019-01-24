using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public enum gameStatus {Play, Pause, GameOver};
    public gameStatus currentStatus;
    public int gameLevel = 1;
    private GameObject holeManager;
    private GameObject levelManager;
    private GameObject mice1, mice2;
    private float gameTime;
    public float maxTime = 60f;

    public Vector2[] startPos1, startPos2;

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
        if (currentStatus == gameStatus.GameOver && InputManager.instance.GetRestart()){
            gameTime = 0f;
            startGame(gameLevel);
        }
        if (gameTime >= maxTime) {
            // send a signal of Game Over
            currentStatus = gameStatus.GameOver;
        }
    }

    public void SetGameStatus(gameStatus status){
        currentStatus = status;
    }

    public int GetGameTime(){
        return (int)gameTime;
    }

    public void startGame(int level){
        holeManager.GetComponent<HoleManager>().InitializeLevel(level);
        currentStatus = gameStatus.Play;
        gameTime = 0f;
        mice1.transform.position = startPos1[gameLevel];
        mice2.transform.position = startPos2[gameLevel];
        mice2.transform.eulerAngles = new Vector3(0, -180, 0);
    }
}
