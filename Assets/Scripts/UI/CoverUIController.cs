using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoverUIController : MonoBehaviour
{
    public enum CoverButton
    {
        START,
        ONLINE,
        EXIT,
        ABOUTUS,
        NULL
    }
    
    public RectTransform comingSoonPanel;
    public CoverUIScaler startUiScaler;
    public CoverUIScaler onlineUiScaler;
    public CoverUIScaler exitUiScaler;
    public CoverMiceUpUI aboutUsUi;
    [HideInInspector]
    public CoverButton currentButton = CoverButton.NULL;

    public float navigationTimeGap = 0.3f;
    private float navigationCount = 0.0f;

    private void Start()
    {
        navigationCount = navigationTimeGap;
    }

    private void Update()
    {
        if ((Input.GetAxis("P1 Navigation Vertical") < -0.01f || Input.GetAxis("P2 Navigation Vertical") < -0.01f) 
            && navigationCount >= navigationTimeGap)
        {
            naviNext();
            navigationCount -= navigationTimeGap;
        }
        else if ((Input.GetAxis("P1 Navigation Vertical") > 0.01f || Input.GetAxis("P2 Navigation Vertical") > 0.01f)
            && navigationCount >= navigationTimeGap)
        {
            naviPrevious();
            navigationCount -= navigationTimeGap;
        }
        else if (Input.GetAxis("P1 Navigation Vertical") < -0.01f || Input.GetAxis("P2 Navigation Vertical") < -0.01f 
                 || Input.GetAxis("P1 Navigation Vertical") > 0.01f || Input.GetAxis("P2 Navigation Vertical") > 0.01f)
        {
            navigationCount += Time.deltaTime;
        }
        else
        {
            navigationCount = navigationTimeGap;
        }

        if (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit"))
        {
            switch (currentButton)
            {
                case CoverButton.START:
                    LocalGame();
                    break;
                case CoverButton.ONLINE:
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

    public void LocalGame()
    {
        SceneManager.LoadScene("LocalMapChoose");
    }

    public void NetworkGame()
    {
        comingSoonPanel.gameObject.SetActive(true);
        StartCoroutine(DelayDisappear(0.6f));
    }

    public void About()
    {
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

    public void RefreshScaler()
    {
        startUiScaler.GamePadOut();
        onlineUiScaler.GamePadOut();
        exitUiScaler.GamePadOut();
        aboutUsUi.GamePadOut();
    }

    public void naviNext()
    {
        switch (currentButton)
        {
            case CoverButton.NULL:
                currentButton = CoverButton.START;
                startUiScaler.GamePadChoose();
                break;
            case CoverButton.START:
                startUiScaler.GamePadOut();
                currentButton = CoverButton.ONLINE;
                onlineUiScaler.GamePadChoose();
                break;
            case CoverButton.ONLINE:
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
            case CoverButton.ONLINE:
                onlineUiScaler.GamePadOut();
                currentButton = CoverButton.START;
                startUiScaler.GamePadChoose();
                break;
            case CoverButton.EXIT:
                exitUiScaler.GamePadOut();
                currentButton = CoverButton.ONLINE;
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
