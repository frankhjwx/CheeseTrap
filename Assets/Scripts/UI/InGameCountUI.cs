using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCountUI : MonoBehaviour
{

    public CounterSlider eatAmountComparison;
    public Slider weightA, weightB;
    //四个饱食度状态的变化时值。
    public float thresholdMin, thresholdMid, thresholdMax;
    
    // Start is called before the first frame update
    void Start()
    {
        weightA.value = 0;
        weightB.value = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="amountDelta"></param>
    /// <returns>
    /// 饱食度状态，默认初始1，越饱越高，取值1，2，3，4
    /// </returns>
    public int AddEatAmount(int playerID, float amountDelta)
    {
        if (playerID == 1)
        {
            eatAmountComparison.LeftValue += amountDelta;
            weightA.value += amountDelta;
            return GetHungerState(weightA.value);
        }
        if (playerID == 2)
        {
            eatAmountComparison.RightValue += amountDelta;
            weightB.value += amountDelta;
            return GetHungerState(weightB.value);
        }
        return 4;
    }

    private int GetHungerState(float weight)
    {
        if (weight < thresholdMin) return 1;
        else if (weight < thresholdMid) return 2;
        else if (weight < thresholdMax) return 3;
        else return 4;
    }
}