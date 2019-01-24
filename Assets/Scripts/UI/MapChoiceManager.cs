using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class MapChoiceManager : MonoBehaviour
{

    public MiceChoiceUI p1Choice;
    public MiceChoiceUI p2Choice;
    public MiceChoiceUI mapChoice;

    private MiceBasicInfo[] basicInfo = new MiceBasicInfo[2];
    private JSONNode root;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindWithTag("LocalMapChoiceManager"))
        {
            Destroy(this.gameObject);
        }

        this.tag = "LocalMapChoiceManager";
        DontDestroyOnLoad(this);
        var miceInfo = Resources.Load<TextAsset>("MiceInfo").text;
        root = JSON.Parse(miceInfo);
        
    }

    public MiceBasicInfo GetMiceBasicInfo(int playerID)
    {
        if (playerID == 1 || playerID == 2) return basicInfo[playerID - 1];
        else return basicInfo[0];
    }

    public int GetMapChosen()
    {
        return mapChoice.CurrentChoice + 1;
    }

    private void GetInfo()
    {
        var choice1Node = root[p1Choice.CurrentChoice];
        basicInfo[0] = new MiceBasicInfo(choice1Node["choiceID"].AsInt, choice1Node["speedState1"].AsFloat, 
            choice1Node["speedState2"].AsFloat, choice1Node["speedState3"].AsFloat, choice1Node["speedState4"].AsFloat,
            choice1Node["eatThresholdMin"].AsFloat, choice1Node["eatThresholdMid"].AsFloat, choice1Node["eatThresholdMax"].AsFloat, 
            choice1Node["initialRadius"].AsFloat, choice1Node["deltaRadius"].AsFloat, choice1Node["timeStep"].AsFloat, choice1Node["maxRadius"].AsFloat);
        var choice2Node = root[p2Choice.CurrentChoice];
        basicInfo[1] = new MiceBasicInfo(choice2Node["choiceID"].AsInt, choice2Node["speedState1"].AsFloat, 
            choice2Node["speedState2"].AsFloat, choice2Node["speedState3"].AsFloat, choice2Node["speedState4"].AsFloat,
            choice2Node["eatThresholdMin"].AsFloat, choice2Node["eatThresholdMid"].AsFloat, choice2Node["eatThresholdMax"].AsFloat, 
            choice2Node["initialRadius"].AsFloat, choice2Node["deltaRadius"].AsFloat, choice2Node["timeStep"].AsFloat, choice2Node["maxRadius"].AsFloat);
    }
}
