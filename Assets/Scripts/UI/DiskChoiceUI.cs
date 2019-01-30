using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiskChoiceUI : MonoBehaviour
{
    public List<GameObject> miceChoicePrefab;
    public RectTransform imagePosition;

    public LocalMapChoiceUI localMapChoiceUi;
    private int miceKinds;

    public int CurrentChoice { get; private set; } = 0;

    private bool choiceRolling = false;
    private GameObject currentChosenMiceInstance;
    
    void Start()
    {
        currentChosenMiceInstance = Instantiate(miceChoicePrefab[0], imagePosition);
        miceKinds = miceChoicePrefab.Count;
        currentChosenMiceInstance.GetComponent<Image>().material.SetFloat("_FadeAlpha", 1);
    }
    void Update()
    {
        if (choiceRolling)
        {
            localMapChoiceUi.Refresh();
            StartCoroutine(DiskFadeOut(currentChosenMiceInstance));
            currentChosenMiceInstance = Instantiate(miceChoicePrefab[CurrentChoice], imagePosition);
            StartCoroutine(DiskFadeIn(currentChosenMiceInstance));
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
        Image diskImage = disk.GetComponent<Image>();
        Material material = diskImage.material;
        Debug.Log(disk.name);
        material.SetFloat("_FadeAlpha", 1);
        float totalUseTime = 1.0f;
        float timer = 0;
        while (timer < totalUseTime)
        {
            material.SetFloat("_FadeAlpha", 1 - timer / totalUseTime);
            timer += Time.deltaTime;
            yield return 0;
        }
        material.SetFloat("_FadeAlpha", 0);
        Destroy(disk);
    }
    
    IEnumerator DiskFadeIn(GameObject disk)
    {
        Image diskImage = disk.GetComponent<Image>();

        Material material = diskImage.material;
        material.SetFloat("_FadeAlpha", 0);
        
        float totalUseTime = 1.0f;
        float timer = 0.0f;
        while (timer < totalUseTime)
        {
            material.SetFloat("_FadeAlpha", timer / totalUseTime);
            timer += Time.deltaTime;
            yield return 0;
        }

        material.SetFloat("_FadeAlpha", 1);
    }


}
