using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoverUIController : MonoBehaviour
{
    public enum CoverButton
    {
        START,
        HOWTOPLAY,
        EXIT,
        ABOUTUS,
        NULL
    }

    enum UiFocus
    {
        Main,
        HowToPlayPanel
    }
    
    public RectTransform comingSoonPanel;
    public CoverUIScaler startUiScaler;
    public CoverUIScaler onlineUiScaler;
    public CoverUIScaler exitUiScaler;
    public CoverMiceUpUI aboutUsUi;
    [HideInInspector]
    public CoverButton currentButton = CoverButton.NULL;
    public GameObject UICamera;

    public float navigationTimeGap = 0.3f;
    private float navigationCount = 0.0f;
    private UiFocus focus = UiFocus.Main;private AudioManager audioManager;

    private void Awake(){
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    private void Start()
    {
        navigationCount = navigationTimeGap;
    }

    private void Update()
    {
        if (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit")) Debug.Log(focus);
        if (focus == UiFocus.Main)
        {
            if ((Input.GetAxis("P1 Navigation Vertical") < -0.01f || Input.GetAxis("P2 Navigation Vertical") < -0.01f)
                && navigationCount >= navigationTimeGap)
            {
                naviNext();
                navigationCount -= navigationTimeGap;
            }
            else if ((Input.GetAxis("P1 Navigation Vertical") > 0.01f ||
                      Input.GetAxis("P2 Navigation Vertical") > 0.01f)
                     && navigationCount >= navigationTimeGap)
            {
                naviPrevious();
                navigationCount -= navigationTimeGap;
            }
            else if (Input.GetAxis("P1 Navigation Vertical") < -0.01f 
                     || Input.GetAxis("P2 Navigation Vertical") < -0.01f 
                     || Input.GetAxis("P1 Navigation Vertical") > 0.01f 
                     || Input.GetAxis("P2 Navigation Vertical") > 0.01f)
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
                switch (currentButton)
                {
                    case CoverButton.START:
                        LocalGame();
                        break;
                    case CoverButton.HOWTOPLAY:
                        NetworkGame();
                        break;
                    case CoverButton.EXIT:
                        Quit();
                        break;
                    case CoverButton.ABOUTUS:
                        About();
                        break;
                }
            }
        }
        else if (focus == UiFocus.HowToPlayPanel)
        {
            if (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit"))
            {
                audioManager.PlayOnceAudioByPath("audio/buttonOnEnter");
                HowToPlayPanelConfirm();
            }
        }
        if (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit")) Debug.Log(focus);
    }

    public void LocalGame()
    {
        SceneManager.LoadScene("LocalMapChoose");
    }

    public void NetworkGame()
    {
        comingSoonPanel.gameObject.SetActive(true);
        focus = UiFocus.HowToPlayPanel;
    }

    public void About()
    {
        UICamera.GetComponent<GetScreenShot>().GenerateObj();
        SceneManager.LoadScene("TeamInfo");
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator DelayDisappear(float delay)
    {
        yield return new WaitForSeconds(delay);
        comingSoonPanel.gameObject.SetActive(false);
    }

    public void HowToPlayPanelConfirm()
    {
        comingSoonPanel.gameObject.SetActive(false);
        focus = UiFocus.Main;
    }

    public void RefreshScaler()
    {
        startUiScaler.GamePadOut();
        onlineUiScaler.GamePadOut();
        exitUiScaler.GamePadOut();
        aboutUsUi.GamePadOut();
    }

    public void naviNext()
    {
        audioManager.PlayOnceAudioByPath("audio/buttonOnEnter");
        switch (currentButton)
        {
            case CoverButton.NULL:
                currentButton = CoverButton.START;
                startUiScaler.GamePadChoose();
                break;
            case CoverButton.START:
                startUiScaler.GamePadOut();
                currentButton = CoverButton.HOWTOPLAY;
                onlineUiScaler.GamePadChoose();
                break;
            case CoverButton.HOWTOPLAY:
                onlineUiScaler.GamePadOut();
                currentButton = CoverButton.EXIT;
                exitUiScaler.GamePadChoose();
                break;
            case CoverButton.EXIT:
                exitUiScaler.GamePadOut();
                currentButton = CoverButton.ABOUTUS;
                aboutUsUi.GamePadChoose();
                break;
            case CoverButton.ABOUTUS:
                aboutUsUi.GamePadOut();
                currentButton = CoverButton.NULL;
                break;
        }
    }

    public void naviPrevious()
    {
        audioManager.PlayOnceAudioByPath("audio/buttonOnEnter");
        switch (currentButton)
        {
            case CoverButton.NULL:
                currentButton = CoverButton.ABOUTUS;
                aboutUsUi.GamePadChoose();
                break;
            case CoverButton.START:
                startUiScaler.GamePadOut();
                currentButton = CoverButton.NULL;
                break;
            case CoverButton.HOWTOPLAY:
                onlineUiScaler.GamePadOut();
                currentButton = CoverButton.START;
                startUiScaler.GamePadChoose();
                break;
            case CoverButton.EXIT:
                exitUiScaler.GamePadOut();
                currentButton = CoverButton.HOWTOPLAY;
                onlineUiScaler.GamePadChoose();
                break;
            case CoverButton.ABOUTUS:
                aboutUsUi.GamePadOut();
                currentButton = CoverButton.EXIT;
                exitUiScaler.GamePadChoose();
                break;
        }
    }
}
