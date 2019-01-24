using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum gameStatus {Play, Pause, GameOver};
    private gameStatus currentStatus;
    public int gameLevel;
    private GameObject holeManager;
    private GameObject mice1, mice2;
    private float gameTime;

    void Start(){
        currentStatus = gameStatus.Play;
        holeManager = GameObject.Find("HoleManager");
        holeManager.GetComponent<HoleManager>().InitializeLevel(1);
        mice1 = GameObject.Find("mice1");
        mice2 = GameObject.Find("mice2");
        gameTime = 0f;
    }

    void Update() {
        if (currentStatus == gameStatus.Play){
            gameTime += Time.deltaTime;
        }
        if (currentStatus == gameStatus.GameOver){
            gameTime = 0f;
        }
        if (gameTime >= 60f) {
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
}
