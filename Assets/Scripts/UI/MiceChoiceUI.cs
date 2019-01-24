using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MiceChoiceUI : MonoBehaviour
{

    public List<Sprite> miceChoice;
    public GameObject miceChoiceImagePrefab;
    public RectTransform imageCollection;
    public float scrollTime = 1.0f;
    
    private int miceKinds;
    private int currentChoice = 0;

    public int CurrentChoice => currentChoice;
    public float imageWidth = 400;
    private float timeLeftToRecover = 0.0f;
    private bool choiceRolling = false;

    // Start is called before the first frame update
    void Start()
    {
        float currentGeneratePos = 0;
        foreach (Sprite miceSprite in miceChoice)
        {
            GameObject imageInstance = Instantiate(miceChoiceImagePrefab, imageCollection, true);
            imageInstance.GetComponent<Image>().sprite = miceSprite;
            imageInstance.GetComponent<RectTransform>().localPosition = new Vector3(currentGeneratePos, 0.0f);
            currentGeneratePos += imageWidth;
        }
        miceKinds = miceChoice.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (choiceRolling)
        {
            if (timeLeftToRecover <= Time.deltaTime)
            {
                timeLeftToRecover = 0;
                choiceRolling = false;
                imageCollection.localPosition = new Vector3(-currentChoice * imageWidth, 0.0f);
            }
            else
            {
                var localPosition = imageCollection.localPosition;
                float deltaXMoving = (-currentChoice * imageWidth - localPosition.x) * Time.deltaTime / timeLeftToRecover;
                var deltaVector = new Vector3(deltaXMoving, 0);
                timeLeftToRecover -= Time.deltaTime;
                localPosition += deltaVector;
                imageCollection.localPosition = localPosition;
            }
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
            timeLeftToRecover = scrollTime;
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
            timeLeftToRecover = scrollTime;
            choiceRolling = true;
        }
    }

    private float TimeRemap(float time)
    {
        if (scrollTime < 0.01) return time;
        float scale = 1 - time / scrollTime;
        float scaleRemapped = scale * scale;
        return scrollTime * (1 - scaleRemapped);
    }

}
