using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGamePauseUI : MonoBehaviour
{

    public RectTransform pausePanel;
    public Button pauseBtn;
    public Image audioImage;

    public Sprite musicOn;
    public Sprite musicOff;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
        {
            audioImage.sprite = musicOff;
        }
        else
        {
            audioImage.sprite = musicOn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        pauseBtn.enabled = false;
        pausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pausePanel.gameObject.SetActive(false);
        pauseBtn.enabled = true;
        Time.timeScale = 1;
    }

    public void Muse()
    {
        if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
        {
            PlayerPrefs.SetInt("MusicOn", 0);
            audioImage.sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetInt("MusicOn", 1);
            audioImage.sprite = musicOff;
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("Opening");
    }
    
}
