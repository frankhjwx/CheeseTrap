using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalMapChoiceUI : MonoBehaviour
{
    public MiceChoiceUI p1Choice;
    public MiceChoiceUI p2Choice;
    public DiskChoiceUI mapChoice;
    public float axisChoosingTimeGap = 0.3f;
    private MapChoiceManager mapChoiceManager;
    private float p1NavigationHorizontalCount = 0.0f;
    private float p2NavigationHorizontalCount = 0.0f;
    private float mapNavigationCount = 0.0f;

    void Start()
    {
        p1NavigationHorizontalCount = axisChoosingTimeGap;
        p2NavigationHorizontalCount = axisChoosingTimeGap;
        mapNavigationCount = axisChoosingTimeGap;
        
        mapChoiceManager = GameObject.FindWithTag("LocalMapChoiceManager").GetComponent<MapChoiceManager>();
        Refresh();
    }

    private void Update()
    {
        if (Input.GetAxis("P1 Navigation Horizontal") < -0.01f && p1NavigationHorizontalCount >= axisChoosingTimeGap)
        {
            p1Choice.rightChoice();
            p1NavigationHorizontalCount -= axisChoosingTimeGap;
        }
        else if (Input.GetAxis("P1 Navigation Horizontal") > 0.01f && p1NavigationHorizontalCount >= axisChoosingTimeGap)
        {
            p1Choice.leftChoice();
            p1NavigationHorizontalCount -= axisChoosingTimeGap;
        }
        else if (Input.GetAxis("P1 Navigation Horizontal") < -0.01f || Input.GetAxis("P1 Navigation Horizontal") > 0.01f)
        {
            p1NavigationHorizontalCount += Time.deltaTime;
        }
        else
        {
            p1NavigationHorizontalCount = axisChoosingTimeGap;
        }
        
        if (Input.GetAxis("P2 Navigation Horizontal") < -0.01f && p2NavigationHorizontalCount >= axisChoosingTimeGap)
        {
            p2Choice.rightChoice();
            p2NavigationHorizontalCount -= axisChoosingTimeGap;
        }
        else if (Input.GetAxis("P2 Navigation Horizontal") > 0.01f && p2NavigationHorizontalCount >= axisChoosingTimeGap)
        {
            p2Choice.leftChoice();
            p2NavigationHorizontalCount -= axisChoosingTimeGap;
        }
        else if (Input.GetAxis("P2 Navigation Horizontal") < -0.01f || Input.GetAxis("P2 Navigation Horizontal") > 0.01f)
        {
            p2NavigationHorizontalCount += Time.deltaTime;
        }
        else
        {
            p2NavigationHorizontalCount = axisChoosingTimeGap;
        }

        if (Input.GetAxis("P1 Navigation Vertical") < -0.01f || Input.GetAxis("P2 Navigation Vertical") < -0.01f)
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
            BackCover();
        }
    }

    public void Refresh()
    {
        Debug.Log(p1Choice == null);
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
}
