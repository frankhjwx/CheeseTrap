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
        float totalUseTime = 1.0f;
        while (totalUseTime > 0.0f)
        {
            var color = diskImage.color;
            color -= new Color(0, 0,  0, color.a * Time.deltaTime / totalUseTime);
            diskImage.color = color;
            totalUseTime -= Time.deltaTime;
            yield return 0;
        }
        Destroy(disk);
    }
    
    IEnumerator DiskFadeIn(GameObject disk)
    {
        Image diskImage = disk.GetComponent<Image>();
        
        var color2 = diskImage.color;
        color2 = new Color(color2.r, color2.g, color2.b, 0);
        diskImage.color = color2;
        
        float totalUseTime = 1.0f;
        while (totalUseTime > 0.0f)
        {
            var color = diskImage.color;
            color += new Color(0, 0,  0, (1 - color.a) * Time.deltaTime / totalUseTime);
            diskImage.color = color;
            totalUseTime -= Time.deltaTime;
            yield return 0;
        }

        var color1 = diskImage.color;
        color1 = new Color(color1.r, color1.g, color1.b, 1);
        diskImage.color = color1;
    }


}
