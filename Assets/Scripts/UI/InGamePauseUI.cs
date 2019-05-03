using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGamePauseUI : MonoBehaviour
{

    public GameObject pauseUI;
    public Button pauseBtn;
    public GameObject MainCamera;
    //public Image audioImage;
    public GameController gameController;

    public Sprite musicOn;
    public Sprite musicOff;
    private AudioManager audioManager;
    
    // Start is called before the first frame update
    void Start()
    {
        // if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
        // {
        //     audioImage.sprite = musicOff;
        // }
        // else
        // {
        //     audioImage.sprite = musicOn;
        // }
        MainCamera.GetComponent<GaussionBlur>().enabled = false;
        pauseUI.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        pauseBtn.enabled = false;
        Time.timeScale = 0;
        StartCoroutine(AddBlur());
        audioManager.StartLowPassEffect();
        pauseUI.GetComponent<Animator>().SetTrigger("Pause");
    }

    public void Resume()
    {
        pauseBtn.enabled = true;
        StartCoroutine(ResetTimeScale());
        StartCoroutine(RemoveBlur());
        audioManager.EndLowPassEffect();
        pauseUI.GetComponent<Animator>().SetTrigger("Continue");
        gameController.SetGameStatus(GameController.gameStatus.Play);
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

    IEnumerator AddBlur(){
        int DownSampleNum = 1;
        float BlurSpreadSize;
        int BlurIterations = 1;
        float timer = 0;
        MainCamera.GetComponent<GaussionBlur>().enabled = true;
        while (timer < 1){
            timer += Time.unscaledDeltaTime;
            BlurSpreadSize = timer * 6;
            MainCamera.GetComponent<GaussionBlur>().UpdateVariables(DownSampleNum, BlurSpreadSize, BlurIterations);
            yield return null;
        }
    }

    IEnumerator RemoveBlur(){
        int DownSampleNum = 1;
        float BlurSpreadSize;
        int BlurIterations = 1;
        float timer = 0;
        while (timer < 1){
            timer += Time.unscaledDeltaTime;
            BlurSpreadSize = (1-timer) * 6;
            MainCamera.GetComponent<GaussionBlur>().UpdateVariables(DownSampleNum, BlurSpreadSize, BlurIterations);
            yield return null;
        }
        MainCamera.GetComponent<GaussionBlur>().enabled = false;
    }

    public void Muse()
    {
        // if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
        // {
        //     PlayerPrefs.SetInt("MusicOn", 0);
        //     audioImage.sprite = musicOn;
        // }
        // else
        // {
        //     PlayerPrefs.SetInt("MusicOn", 1);
        //     audioImage.sprite = musicOff;
        // }
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Cover");
    }
    
}
