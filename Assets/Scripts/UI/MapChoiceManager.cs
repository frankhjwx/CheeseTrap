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
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        
    }

    public MiceBasicInfo GetMiceBasicInfo(int playerID)
    {
        if (playerID == 1 || playerID == 2) return basicInfo[playerID - 1];
        else return basicInfo[0];
    }
}
