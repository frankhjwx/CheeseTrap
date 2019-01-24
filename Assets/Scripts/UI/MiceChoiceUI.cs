using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MiceChoiceUI : MonoBehaviour
{

    public List<Sprite> miceChoice;

    private int miceKinds;
    private int currentChoice = 0;

    public int CurrentChoice => currentChoice;
    private Vector2 currentChoicePos;
    
    public RectTransform miceShowArea1;
    public RectTransform miceShowArea2;
    private RectTransform currentMiceShowArea;

    public float imageWidth = 400;
    private float timeLeftToRecover = 0.0f;
    private bool choiceRolling = false;

    // Start is called before the first frame update
    void Start()
    {
        miceKinds = miceChoice.Count;
        currentMiceShowArea = miceShowArea1;
        currentChoicePos = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (choiceRolling)
        {
            if (timeLeftToRecover <= 0.01)
            {
                timeLeftToRecover = 0;
                choiceRolling = false;
                miceShowArea1.GetComponent<SpriteRenderer>().sprite = miceChoice[currentChoice];
                miceShowArea1.localPosition = new Vector2(0.0f, 0.0f);
                miceShowArea2.localPosition = new Vector2(imageWidth, 0.0f);
                currentMiceShowArea = miceShowArea1;
            }
            else
            {
                timeLeftToRecover -= Time.deltaTime;
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
            currentChoicePos = new Vector2(currentChoicePos.x - imageWidth, currentChoicePos.y);
            timeLeftToRecover = 1.0f;
            choiceRolling = true;
        }
    }
}
