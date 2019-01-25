using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class MapChoiceManager : MonoBehaviour
{
    public int p1ChoiceIndex = 0;
    public int p2ChoiceIndex = 0;
    public int mapChoiceIndex = 0;

    private MiceBasicInfo[] basicInfo = new MiceBasicInfo[2];
    private JSONNode root;

    void Awake()
    {
        if (GameObject.FindWithTag("LocalMapChoiceManager"))
        {
            Destroy(this.gameObject);
        }
        this.tag = "LocalMapChoiceManager";
        DontDestroyOnLoad(this);
        
    }
    
    // Start is called before the first frame update
    void Start()
    {

        var miceInfo = Resources.Load<TextAsset>("MiceInfo").text;
        root = JSON.Parse(miceInfo);
        Debug.Log(miceInfo);
    }

    public MiceBasicInfo GetMiceBasicInfo(int playerID)
    {
        GetInfo();
        if (playerID == 1 || playerID == 2) return basicInfo[playerID - 1];
        else return basicInfo[0];
    }

    public int GetMapChosen()
    {
        return mapChoiceIndex + 1;
    }

    private void GetInfo()
    {
        Debug.Log(root.Count + " " + p1ChoiceIndex);
        var choice1Node = root[p1ChoiceIndex];
        basicInfo[0] = new MiceBasicInfo(choice1Node["choiceID"].AsInt, choice1Node["speedState1"].AsFloat, 
            choice1Node["speedState2"].AsFloat, choice1Node["speedState3"].AsFloat, choice1Node["speedState4"].AsFloat,
            choice1Node["eatThresholdMin"].AsFloat, choice1Node["eatThresholdMid"].AsFloat, choice1Node["eatThresholdMax"].AsFloat, 
            choice1Node["initialRadius"].AsFloat, choice1Node["deltaRadius"].AsFloat, choice1Node["timeStep"].AsFloat, choice1Node["maxRadius"].AsFloat);
        Debug.Log(root.Count + " " + p2ChoiceIndex);
        var choice2Node = root[p2ChoiceIndex];
        basicInfo[1] = new MiceBasicInfo(choice2Node["choiceID"].AsInt, choice2Node["speedState1"].AsFloat, 
            choice2Node["speedState2"].AsFloat, choice2Node["speedState3"].AsFloat, choice2Node["speedState4"].AsFloat,
            choice2Node["eatThresholdMin"].AsFloat, choice2Node["eatThresholdMid"].AsFloat, choice2Node["eatThresholdMax"].AsFloat, 
            choice2Node["initialRadius"].AsFloat, choice2Node["deltaRadius"].AsFloat, choice2Node["timeStep"].AsFloat, choice2Node["maxRadius"].AsFloat);
    }
}
