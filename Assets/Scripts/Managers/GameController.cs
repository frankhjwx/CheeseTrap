using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour {
    public enum gameStatus {DisplayHint, CountDown, Play, Pause, MouseDieOver, TimeUpOver};
    public gameStatus currentStatus;
    public bool isPlaying = false;
    public int gameLevel = 1;
    private GameObject holeManager;
    private GameObject levelManager;
    private GameObject mice1, mice2;
    private float gameTime;
    public int maxTime = 60;

    public Vector2[] startPos1, startPos2;
    public GameObject GameOverUI, AreaDisplayer;
    public InGamePauseUI pauseUi;
    public GameObject hint;
    public GameObject readyObject, goObject;


    //InputManager inputManager;


    private AudioManager audioManager;

    private void Awake(){
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    void Start(){
        if (GameObject.FindWithTag("LocalMapChoiceManager"))
        {
            var localMapChoice = GameObject.FindWithTag("LocalMapChoiceManager").GetComponent<MapChoiceManager>();
            gameLevel = localMapChoice.GetMapChosen();
        }
        Debug.Log(gameLevel);
        currentStatus = gameStatus.DisplayHint;
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
        isPlaying = false;

        if (gameLevel != 0) {
            hint.SetActive(true);
        } else hint.SetActive(false);
        hint.transform.Find("HintConfirmButton").GetComponent<Button>().onClick.AddListener(hintConfirmed);

        readyObject.SetActive(false);
        goObject.SetActive(false);

        gameTime = 0f;
        
    }

    private void hintConfirmed(){
        hint.SetActive(false);
        currentStatus = gameStatus.CountDown;
        CountDown();
    }

    private void CountDown(){
        readyObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000, 0);
        goObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000, 0);
        readyObject.SetActive(true);
        goObject.SetActive(true);
        Sequence readyGoSq = DOTween.Sequence();
        readyGoSq.Insert(0, readyObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.InBack));
        readyGoSq.Insert(1, readyObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(2000, 0), 0.5f).SetEase(Ease.OutBack));
        readyGoSq.Insert(1, goObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.InBack));
        readyGoSq.Insert(2, goObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(2000, 0), 0.5f).SetEase(Ease.OutBack));
        readyGoSq.SetLoops(1);
        readyGoSq.OnComplete(()=>currentStatus = gameStatus.Play);
    }

    void Update() {
        if (currentStatus == gameStatus.DisplayHint || currentStatus == gameStatus.CountDown) {
            isPlaying = false;
            if (currentStatus == gameStatus.DisplayHint && (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit"))){
                audioManager.PlayOnceAudioByPath("audio/buttonOnClick");
                hintConfirmed();
            }

            return;
        }

        if (currentStatus == gameStatus.Play){
            isPlaying = true;
            gameTime += Time.deltaTime;
        }
        // if ((currentStatus == gameStatus.MouseDieOver || currentStatus == gameStatus.TimeUpOver) && InputManager.instance.GetRestart()){
        //     gameTime = 0f;
        //     startGame(gameLevel);
        // }
        if (gameTime >= maxTime && currentStatus != gameStatus.TimeUpOver) {
            // send a signal of Game Over
            isPlaying = false;
            TimeUpGameOver();
        }

        if (currentStatus == gameStatus.Play)
        {
            if (Input.GetButtonDown("P1 Pause") || Input.GetButtonDown("P2 Pause"))
            {
                pauseUi.Pause();
                audioManager.PlayOnceAudioByPath("audio/buttonOnClick");
                SetGameStatus(gameStatus.Pause);
            }
        }
        /*if (currentStatus == gameStatus.TimeUpOver || currentStatus == gameStatus.MouseDieOver)
        {
            if (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit"))
            {
                RestartGame();
            }

            if (Input.GetButtonDown("P1 Cancel") || Input.GetButtonDown("P2 Cancel"))
            {
                SelectLevel();
            }
        }*/
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
