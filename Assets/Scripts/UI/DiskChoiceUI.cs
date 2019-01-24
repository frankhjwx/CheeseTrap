using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiskChoiceUI : MonoBehaviour
{
    public List<GameObject> miceChoicePrefab;
    public RectTransform imagePosition;
    
    private int miceKinds;
    private int currentChoice = 0;

    public int CurrentChoice => currentChoice;
    private float timeLeftToRecover = 0.0f;
    private bool choiceRolling = false;
    private GameObject currentChosenMiceInstance;
    
    void Start()
    {
        currentChosenMiceInstance = Instantiate(miceChoicePrefab[0], imagePosition);
        miceKinds = miceChoicePrefab.Count;
    }
    void Update()
    {
        if (choiceRolling)
        {
            StartCoroutine(DiskFadeOut(currentChosenMiceInstance));
            currentChosenMiceInstance = Instantiate(miceChoicePrefab[currentChoice], imagePosition);
            StartCoroutine(DiskFadeIn(currentChosenMiceInstance));
            choiceRolling = false;
        }
    }

    public void leftChoice()
    {
        if (currentChoice <= 0)
        {
            currentChoice = 0;
            
        }
        else
        {
            currentChoice--;
            choiceRolling = true;
        }
    }
    
    public void rightChoice()
    {
        if (currentChoice >= miceKinds - 1)
        {
            currentChoice = miceKinds - 1;
            
        }
        else
        {
            currentChoice++;
            choiceRolling = true;
        }
    }
    
    public void rightChoiceLoop()
    {
        if (currentChoice >= miceKinds - 1)
        {
            currentChoice = 0;
            choiceRolling = true;
        }
        else
        {
            currentChoice++;
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
