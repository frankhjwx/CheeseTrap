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
    }

    public void SetGameStatus(gameStatus status){
        currentStatus = status;
    }

    public int GetGameTime(){
        return (int)gameTime;
    }

    public void RestartGame(){
        SceneManager.LoadScene("LocalGame");
    }

    public void SelectLevel(){
        SceneManager.LoadScene("LocalMapChoose");
    }

    public void MouseDieGameOver(int playerID){
        SetGameStatus(GameController.gameStatus.MouseDieOver);
        if (playerID == 1) {
            GameOverUI.transform.GetChild(2).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(1).gameObject.SetActive(false);
        } else {
            GameOverUI.transform.GetChild(1).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(2).gameObject.SetActive(false);
        }
        GameOverUI.transform.position = new Vector2(960, 540);
        GameOverUI.transform.GetChild(8).gameObject.SetActive(false);
        GameOverUI.GetComponent<Animator>().SetTrigger("GameOver");
        AreaDisplayer.GetComponent<AreaDisplayerUI>().Display();
    }

    public void TimeUpGameOver(){
        SetGameStatus(GameController.gameStatus.TimeUpOver);
        if (holeManager.GetComponent<HoleManager>().areas[1] > holeManager.GetComponent<HoleManager>().areas[2]) {
            GameOverUI.transform.GetChild(1).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(2).gameObject.SetActive(false);
        } else {
            GameOverUI.transform.GetChild(2).gameObject.SetActive(true);
            GameOverUI.transform.GetChild(1).gameObject.SetActive(false);
        }
        GameOverUI.transform.position = new Vector2(960, 540);
        GameOverUI.GetComponent<Animator>().SetTrigger("GameOver");
        AreaDisplayer.GetComponent<AreaDisplayerUI>().Display();
    }

}
