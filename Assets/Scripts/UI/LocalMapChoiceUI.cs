using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalMapChoiceUI : MonoBehaviour
{
    public MiceChoiceUI p1Choice;
    public MiceChoiceUI p2Choice;
    public DiskChoiceUI mapChoice;
    private MapChoiceManager mapChoiceManager;

    void Start()
    {
        mapChoiceManager = GameObject.FindWithTag("LocalMapChoiceManager").GetComponent<MapChoiceManager>();
        Refresh();
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
