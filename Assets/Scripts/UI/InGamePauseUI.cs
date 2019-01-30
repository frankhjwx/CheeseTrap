using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGamePauseUI : MonoBehaviour
{

    public GameController gameController;
    public GameObject pauseUI;
    public Button pauseBtn;
    public GameObject MainCamera;
    //public Image audioImage;

    public Sprite musicOn;
    public Sprite musicOff;
    
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        if (gameController.CurrentStatus == GameController.gameStatus.Play)
        {
            pauseBtn.enabled = false;
            gameController.PauseGame();
            StartCoroutine(AddBlur());
            pauseUI.GetComponent<Animator>().SetTrigger("Pause");
        }
    }

    public void Resume()
    {
        if (gameController.CurrentStatus == GameController.gameStatus.Pause)
        {
            pauseBtn.enabled = true;
            gameController.ResumeGame();
            StartCoroutine(RemoveBlur());
            pauseUI.GetComponent<Animator>().SetTrigger("Continue");
        }
    }


    IEnumerator AddBlur(){
        int DownSampleNum = 1;
        float BlurSpreadSize = 0;
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
        float BlurSpreadSize = 0;
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
