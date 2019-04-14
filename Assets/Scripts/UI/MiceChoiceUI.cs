using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class MiceChoiceUI : MonoBehaviour
{

    public List<GameObject> miceChoicePrefab;
    public RectTransform imagePosition;
    //public float scrollTime = 1.0f;

    public Slider speedSlider;
    public Slider eatSpeedSlider;
    public Slider beingFatSpeedSlider;
    public LocalMapChoiceUI localMapChoiceUi;
    
    private int miceKinds;
    private int currentChoice = 0;

    public int CurrentChoice => currentChoice;
    //public float imageWidth = 400;
    //private float timeLeftToRecover = 0.0f;
    private bool choiceRolling = false;
    private GameObject currentChosenMiceInstance;
    private JSONNode miceInfoRoot;
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        float currentGeneratePos = 0;
        foreach (Sprite miceSprite in miceChoice)
        {
            GameObject imageInstance = Instantiate(miceChoiceImagePrefab, imageCollection, true);
            imageInstance.GetComponent<Image>().sprite = miceSprite;
            imageInstance.GetComponent<RectTransform>().localPosition = new Vector3(currentGeneratePos, 0.0f);
            currentGeneratePos += imageWidth;
        }
        */
        currentChosenMiceInstance = Instantiate(miceChoicePrefab[0], imagePosition);
        miceKinds = miceChoicePrefab.Count;

        TextAsset textAsset = Resources.Load<TextAsset>("MiceInfo");
        miceInfoRoot = JSON.Parse(textAsset.text);
        
        speedSlider.value = miceInfoRoot[currentChoice]["speedLevel"].AsFloat;
        eatSpeedSlider.value = miceInfoRoot[currentChoice]["eatSpeedLevel"].AsFloat;
        beingFatSpeedSlider.value = miceInfoRoot[currentChoice]["beingFatSpeedLevel"].AsFloat;
    }

    // Update is called once per frame
    void Update()
    {
        if (choiceRolling)
        {
            /*
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
            */
            
            Destroy(currentChosenMiceInstance);
            currentChosenMiceInstance = Instantiate(miceChoicePrefab[currentChoice], imagePosition);
            choiceRolling = false;
            localMapChoiceUi.Refresh();

            StartCoroutine(SliderSetNewValue(speedSlider, miceInfoRoot[currentChoice]["speedLevel"].AsFloat));
            StartCoroutine(SliderSetNewValue(eatSpeedSlider, miceInfoRoot[currentChoice]["eatSpeedLevel"].AsFloat));
            StartCoroutine(SliderSetNewValue(beingFatSpeedSlider, miceInfoRoot[currentChoice]["beingFatSpeedLevel"].AsFloat));
        }
    }

    public void leftChoice()
    {
        if (currentChoice <= 0)
        {
            currentChoice = miceKinds - 1;
            
        }
        else
        {
            currentChoice--;
            //timeLeftToRecover = scrollTime;
        }
        choiceRolling = true;
    }
    
    public void rightChoice()
    {
        if (currentChoice >= miceKinds - 1)
        {
            currentChoice = 0;
            
        }
        else
        {
            currentChoice++;
            //timeLeftToRecover = scrollTime;
        }
        choiceRolling = true;
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
            //timeLeftToRecover = scrollTime;
            choiceRolling = true;
        }
    }

    IEnumerator SliderSetNewValue(Slider slider, float newValue)
    {
        slider.value = 0;
        float totalUseTime = 0.5f;
        float timer = 0;
        while (timer < totalUseTime)
        {
            slider.value = newValue * timer / totalUseTime;
            timer += Time.deltaTime;
            yield return 0;
        }

        slider.value = newValue;
    }


}
