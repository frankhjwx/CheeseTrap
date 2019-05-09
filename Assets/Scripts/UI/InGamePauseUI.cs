using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGamePauseUI : MonoBehaviour
{
    enum PauseButton
    {
        Null,
        Restart,
        Continue,
        Return
    };

    public GameObject pauseUI;
    public Button pauseBtn;
    public GameObject MainCamera;
    public GameObject TimeCamera;
    //public Image audioImage;
    public GameController gameController;

    public Sprite musicOn;
    public Sprite musicOff;
    private AudioManager audioManager;
    private PauseButton currentSelected = PauseButton.Null;
    public Animator restartHighlight;
    public Animator continueHighlight;
    public Animator returnHighlight;
    public float navigationTimeGap = 0.3f;
    private float navigationCount = 0.0f;
    private bool usingHandle = false;
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
        TimeCamera.GetComponent<GaussionBlur>().enabled = false;
        pauseUI.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Mathf.Abs(Input.GetAxis("Mouse X")) >= 0.01f || Mathf.Abs(Input.GetAxis("Mouse Y")) >= 0.01f) && usingHandle)
        {
            AllUnhighlight();
            usingHandle = false;
            Cursor.visible = true;
        }
        if (gameController.currentStatus == GameController.gameStatus.Pause)
        {
            if ((Input.GetAxis("P1 Navigation Horizontal") < -0.01f || Input.GetAxis("P2 Navigation Horizontal") < -0.01f)
                && navigationCount >= navigationTimeGap)
            {
                Cursor.visible = false;
                usingHandle = true;
                NaviPrevious();
                audioManager.PlayOnceAudioByPath("audio/buttonOnEnter");
                navigationCount -= navigationTimeGap;
            }
            else if ((Input.GetAxis("P1 Navigation Horizontal") > 0.01f ||
                      Input.GetAxis("P2 Navigation Horizontal") > 0.01f)
                     && navigationCount >= navigationTimeGap)
            {
                Cursor.visible = false;
                usingHandle = true;
                NaviNext();
                audioManager.PlayOnceAudioByPath("audio/buttonOnEnter");
                navigationCount -= navigationTimeGap;
            }
            else if (Input.GetAxis("P1 Navigation Horizontal") < -0.01f 
                     || Input.GetAxis("P2 Navigation Horizontal") < -0.01f 
                     || Input.GetAxis("P1 Navigation Horizontal") > 0.01f 
                     || Input.GetAxis("P2 Navigation Horizontal") > 0.01f)
            {
                navigationCount += Time.deltaTime;
            }
            else
            {
                navigationCount = navigationTimeGap;
            }
        
            if (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit"))
            {
                audioManager.PlayOnceAudioByPath("audio/buttonOnClick");
                switch (currentSelected)
                {
                    case PauseButton.Restart:
                        gameController.RestartGame();
                        break;
                    case PauseButton.Continue:
                        Resume();
                        break;
                    case PauseButton.Return:
                        Exit();
                        break;
                }
            }
        }
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
        TimeCamera.GetComponent<GaussionBlur>().enabled = true;
        while (timer < 1){
            timer += Time.unscaledDeltaTime;
            BlurSpreadSize = timer * 4;
            MainCamera.GetComponent<GaussionBlur>().UpdateVariables(DownSampleNum, BlurSpreadSize, BlurIterations);
            TimeCamera.GetComponent<GaussionBlur>().UpdateVariables(DownSampleNum, BlurSpreadSize, BlurIterations);
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
            BlurSpreadSize = (1-timer) * 4;
            MainCamera.GetComponent<GaussionBlur>().UpdateVariables(DownSampleNum, BlurSpreadSize, BlurIterations);
            TimeCamera.GetComponent<GaussionBlur>().UpdateVariables(DownSampleNum, BlurSpreadSize, BlurIterations);
            yield return null;
        }
        MainCamera.GetComponent<GaussionBlur>().enabled = false;
        TimeCamera.GetComponent<GaussionBlur>().enabled = false;
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

    void NaviNext()
    {
        AllUnhighlight();
        switch (currentSelected)
        {
            case PauseButton.Null:
                currentSelected = PauseButton.Restart;
                restartHighlight.SetTrigger("Highlighted");
                break;
            case PauseButton.Restart:
                currentSelected = PauseButton.Continue;
                continueHighlight.SetTrigger("Highlighted");
                break;
            case PauseButton.Continue:
                currentSelected = PauseButton.Return;
                returnHighlight.SetTrigger("Highlighted");
                break;
            case PauseButton.Return:
                currentSelected = PauseButton.Restart;
                restartHighlight.SetTrigger("Highlighted");
                break;
        }
        Debug.Log("NaviNext:" + currentSelected);
    }

    void NaviPrevious()
    {
        AllUnhighlight();
        switch (currentSelected)
        {
            case PauseButton.Null:
                currentSelected = PauseButton.Return;
                returnHighlight.SetTrigger("Highlighted");
                break;
            case PauseButton.Restart:
                currentSelected = PauseButton.Return;
                returnHighlight.SetTrigger("Highlighted");
                break;
            case PauseButton.Continue:
                currentSelected = PauseButton.Restart;
                restartHighlight.SetTrigger("Highlighted");
                break;
            case PauseButton.Return:
                currentSelected = PauseButton.Continue;
                continueHighlight.SetTrigger("Highlighted");
                break;
        }
    }

    void AllUnhighlight()
    {
        restartHighlight.SetTrigger("Normal");
        continueHighlight.SetTrigger("Normal");
        returnHighlight.SetTrigger("Normal");
    }
    
}
