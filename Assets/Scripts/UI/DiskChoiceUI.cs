using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiskChoiceUI : MonoBehaviour
{
    public List<GameObject> miceChoicePrefab;
    public RectTransform imagePosition;

    public LocalMapChoiceUI localMapChoiceUi;
    public MapStarDisplay mapStarDisplay;
    private int miceKinds;

    public int CurrentChoice
    {
        get => currentChoice;
        private set
        {
            lastMapChoice = currentChoice;
            currentChoice = value;
        }
    }
    private int currentChoice;

    private float timeLeftToRecover = 0.0f;
    private bool choiceRolling = false;
    private GameObject currentChosenMiceInstance;
    
    private readonly List<int> mapPreviewChoiceMap = new List<int>{1, 1, 2, 2, 3, 3, 4, 5, 5};
    private readonly List<int> mapStarChoiceMap = new List<int>{1, 2, 1, 2, 1, 3, 2, 2, 3};

    private int lastMapChoice = 0;
    void Start()
    {
        currentChosenMiceInstance = Instantiate(miceChoicePrefab[0], imagePosition);
        mapStarDisplay.StarCount = mapStarChoiceMap[CurrentChoice];
        miceKinds = mapPreviewChoiceMap.Count;
        currentChosenMiceInstance.transform.Find("CircleMap").GetComponent<Image>().material.SetFloat("_FadeAlpha", 1);
    }
    void Update()
    {
        if (choiceRolling)
        {
            localMapChoiceUi.Refresh();
            mapStarDisplay.StarCount = mapStarChoiceMap[CurrentChoice];
            if (mapPreviewChoiceMap[lastMapChoice] != mapPreviewChoiceMap[CurrentChoice])
            {
                StartCoroutine(DiskFadeOut(currentChosenMiceInstance));
                currentChosenMiceInstance = Instantiate(miceChoicePrefab[mapPreviewChoiceMap[CurrentChoice] - 1], imagePosition);
                StartCoroutine(DiskFadeIn(currentChosenMiceInstance));
            }

            choiceRolling = false;
        }
    }

    public void leftChoice()
    {
        if (CurrentChoice <= 0)
        {
            CurrentChoice = 0;
        }
        else
        {
            CurrentChoice--;
            choiceRolling = true;
        }
    }
    
    public void leftChoiceLoop()
    {
        if (CurrentChoice <= 0)
        {
            CurrentChoice = miceKinds - 1;
            choiceRolling = true;
        }
        else
        {
            CurrentChoice--;
            choiceRolling = true;
        }
    }
    
    public void rightChoice()
    {
        if (CurrentChoice >= miceKinds - 1)
        {
            CurrentChoice = miceKinds - 1;
            
        }
        else
        {
            CurrentChoice++;
            choiceRolling = true;
        }
    }
    
    public void rightChoiceLoop()
    {
        if (CurrentChoice >= miceKinds - 1)
        {
            CurrentChoice = 0;
            choiceRolling = true;
        }
        else
        {
            CurrentChoice++;
            choiceRolling = true;
        }
    }

    IEnumerator DiskFadeOut(GameObject disk)
    {
        Image diskImage = disk.transform.Find("CircleMap").GetComponent<Image>();
        Image cakeImage = disk.transform.Find("1").GetComponent<Image>();
        Material material = diskImage.material;
        material.SetFloat("_FadeAlpha", 1);
        SetColorAlpha(cakeImage.color, 1);
        float totalUseTime = 1.0f;
        float timer = 0;
        while (timer < totalUseTime)
        {
            material.SetFloat("_FadeAlpha", 1 - timer / totalUseTime);
            cakeImage.color = SetColorAlpha(cakeImage.color, 1 - timer / totalUseTime);
            timer += Time.deltaTime;
            yield return 0;
        }
        material.SetFloat("_FadeAlpha", 0);
        cakeImage.color = SetColorAlpha(cakeImage.color, 0);
        Destroy(disk);
    }
    
    IEnumerator DiskFadeIn(GameObject disk)
    {
        Image diskImage = disk.transform.Find("CircleMap").GetComponent<Image>();
        Image cakeImage = disk.transform.Find("1").GetComponent<Image>();
        Material material = diskImage.material;
        material.SetFloat("_FadeAlpha", 0);
        SetColorAlpha(cakeImage.color, 0);
        float totalUseTime = 1.0f;
        float timer = 0.0f;
        while (timer < totalUseTime)
        {
            material.SetFloat("_FadeAlpha", timer / totalUseTime);
            cakeImage.color = SetColorAlpha(cakeImage.color, timer / totalUseTime);
            timer += Time.deltaTime;
            yield return 0;
        }

        material.SetFloat("_FadeAlpha", 1);
        cakeImage.color = SetColorAlpha(cakeImage.color, 1);
    }

    Color SetColorAlpha(Color color, float transparent)
    {
        return new Color(color.r, color.g, color.b, transparent);
    }

}
