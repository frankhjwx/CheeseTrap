using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGamePauseUI : MonoBehaviour
{

    public GameObject pauseUI;
    public Button pauseBtn;
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

        pauseUI.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        pauseBtn.enabled = false;
        Time.timeScale = 0;
        pauseUI.GetComponent<Animator>().SetTrigger("Pause");
    }

    public void Resume()
    {
        pauseBtn.enabled = true;
        Time.timeScale = 1;
        pauseUI.GetComponent<Animator>().SetTrigger("Continue");
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
