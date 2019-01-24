﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiceBasicInfo
{
    public int choiceID;
    public float speedState1;
    public float speedState2;
    public float speedState3;
    public float speedState4;
    public float eatThresholdMin;
    public float eatThresholdMid;
    public float eatThresholdMax;

    public MiceBasicInfo(int choiceID, float speedState1, float speedState2, float speedState3, float speedState4,
        float eatThresholdMin, float eatThresholdMid, float eatThresholdMax)
    {
        this.choiceID = choiceID;
        this.speedState1 = speedState1;
        this.speedState2 = speedState2;
        this.speedState3 = speedState3;
        this.speedState4 = speedState4;
        this.eatThresholdMin = eatThresholdMin;
        this.eatThresholdMid = eatThresholdMid;
        this.eatThresholdMax = eatThresholdMax;
    }
}
