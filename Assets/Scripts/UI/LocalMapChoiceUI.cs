using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalMapChoiceUI : MonoBehaviour
{
    enum MapChoiceState
    {
        PlayerChoosing,
        Idle,
        MapChoosing
    }
    
    public MiceChoiceUI p1Choice;
    public MiceChoiceUI p2Choice;
    public DiskChoiceUI mapChoice;
    public RectTransform backgroundTransform;
    public Vector2 playerChoosePos;
    public Vector2 mapChoosePos;
    public float moveDuration = 1.0f;
    private MapChoiceManager mapChoiceManager;
    public float axisChoosingTimeGap = 0.3f;
    private float p1NavigationHorizontalCount = 0.0f;
    private float p2NavigationHorizontalCount = 0.0f;
    private float mapNavigationCount = 0.0f;
    private MapChoiceState state = MapChoiceState.Idle;
    private int mouseChooseConfirmCount = 0;
    public GameObject p1MouseSelected;
    public GameObject p2MouseSelected;

    void Start()
    {
        p1NavigationHorizontalCount = axisChoosingTimeGap;
        p2NavigationHorizontalCount = axisChoosingTimeGap;
        mapNavigationCount = axisChoosingTimeGap;
        
        mapChoiceManager = GameObject.FindWithTag("LocalMapChoiceManager").GetComponent<MapChoiceManager>();
        Refresh();

        state = MapChoiceState.PlayerChoosing;
    }

    private void Update()
    {
        if (state == MapChoiceState.PlayerChoosing)
        {
            if (Input.GetAxis("P1 Navigation Horizontal") < -0.01f &&
                p1NavigationHorizontalCount >= axisChoosingTimeGap)
            {
                p1Choice.leftChoice();
                p1NavigationHorizontalCount -= axisChoosingTimeGap;
            }
            else if (Input.GetAxis("P1 Navigation Horizontal") > 0.01f &&
                     p1NavigationHorizontalCount >= axisChoosingTimeGap)
            {
                p1Choice.rightChoice();
                p1NavigationHorizontalCount -= axisChoosingTimeGap;
            }
            else if (Input.GetAxis("P1 Navigation Horizontal") < -0.01f ||
                     Input.GetAxis("P1 Navigation Horizontal") > 0.01f)
            {
                p1NavigationHorizontalCount += Time.deltaTime;
            }
            else
            {
                p1NavigationHorizontalCount = axisChoosingTimeGap;
            }

            if (Input.GetAxis("P2 Navigation Horizontal") < -0.01f &&
                p2NavigationHorizontalCount >= axisChoosingTimeGap)
            {
                p2Choice.leftChoice();
                p2NavigationHorizontalCount -= axisChoosingTimeGap;
            }
            else if (Input.GetAxis("P2 Navigation Horizontal") > 0.01f &&
                     p2NavigationHorizontalCount >= axisChoosingTimeGap)
            {
                p2Choice.rightChoice();
                p2NavigationHorizontalCount -= axisChoosingTimeGap;
            }
            else if (Input.GetAxis("P2 Navigation Horizontal") < -0.01f ||
                     Input.GetAxis("P2 Navigation Horizontal") > 0.01f)
            {
                p2NavigationHorizontalCount += Time.deltaTime;
            }
            else
            {
                p2NavigationHorizontalCount = axisChoosingTimeGap;
            }

            if (Input.GetButtonDown("P1 Submit"))
            {
                ConfirmP1Mouse();
            }

            if (Input.GetButtonDown("P2 Submit"))
            {
                ConfirmP2Mouse();
            }
            if (Input.GetButtonDown("P1 Cancel") || Input.GetButtonDown("P2 Cancel"))
            {
                BackCover();
            }
        }

        if (state == MapChoiceState.MapChoosing)
        {
            if (Input.GetAxis("P1 Navigation Horizontal") < -0.01f || Input.GetAxis("P2 Navigation Horizontal") < -0.01f)
            {
                if (mapNavigationCount >= axisChoosingTimeGap)
                {
                    mapChoice.leftChoiceLoop();
                    mapNavigationCount -= axisChoosingTimeGap;
                }
                else
                {
                    mapNavigationCount += Time.deltaTime;
                }
            }
            else if (Input.GetAxis("P1 Navigation Horizontal") > 0.01f || Input.GetAxis("P2 Navigation Horizontal") > 0.01f)
            {
                if (mapNavigationCount >= axisChoosingTimeGap)
                {
                    mapChoice.rightChoiceLoop();
                    mapNavigationCount -= axisChoosingTimeGap;
                }
                else
                {
                    mapNavigationCount += Time.deltaTime;
                }
            }
            else
            {
                mapNavigationCount = axisChoosingTimeGap;
            }

            if (Input.GetButtonDown("P1 Submit") || Input.GetButtonDown("P2 Submit"))
            {
                StartGame();
            }
            if (Input.GetButtonDown("P1 Cancel") || Input.GetButtonDown("P2 Cancel"))
            {
                ToChoosePlayer();
            }
        }
    }

    public void Refresh()
    {
        mapChoiceManager.p1ChoiceIndex = p1Choice.CurrentChoice;
        mapChoiceManager.p2ChoiceIndex = p2Choice.CurrentChoice;
        mapChoiceManager.mapChoiceIndex = mapChoice.CurrentChoice;
    }
    
    public void StartGame()
    {
        GameObject mapChoice = GameObject.FindWithTag("LocalMapChoiceManager");
        if (mapChoice != null)
        {
            MapChoiceManager mapChoiceManager = mapChoice.GetComponent<MapChoiceManager>();
            SceneManager.LoadScene("LocalGame");
        }
    }

    public void BackCover()
    {
        SceneManager.LoadScene("Cover");
    }

    IEnumerator MoveTo(Vector2 targetPos, MapChoiceState finalState)
    {
        state = MapChoiceState.Idle;
        Vector2 originalPos = backgroundTransform.localPosition;
        var timer = 0f;
        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            backgroundTransform.localPosition = originalPos + (targetPos - originalPos) * timer / moveDuration;
            yield return 0;
        }
        backgroundTransform.localPosition = targetPos;
        state = finalState;
    }

    public void ToChooseMap()
    {
        StartCoroutine(MoveTo(mapChoosePos, MapChoiceState.MapChoosing));
    }

    private void ToChoosePlayer()
    {
        StartCoroutine(MoveTo(playerChoosePos, MapChoiceState.PlayerChoosing));
    }

    public void ConfirmP1Mouse()
    {
        mouseChooseConfirmCount++;
        p1MouseSelected.SetActive(false);
        if (mouseChooseConfirmCount >= 2)
        {
            ToChooseMap();
        }
    }
    
    public void ConfirmP2Mouse()
    {
        mouseChooseConfirmCount++;
        p2MouseSelected.SetActive(false);
        if (mouseChooseConfirmCount >= 2)
        {
            ToChooseMap();
        }
    }
    
    public void BackButton()
    {
        if (state == MapChoiceState.MapChoosing)
        {
            ToChoosePlayer();
        }
        else
        {
            BackCover();
        }
    }
}
