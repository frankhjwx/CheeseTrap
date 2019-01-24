using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCountUI : MonoBehaviour
{

    public CounterSlider eatAmountComparison;
    public FourLevelSlider weightA, weightB;
    public float eatMax;
    
    // Start is called before the first frame update
    void Start()
    {
        weightA.value = 0;
        weightB.value = 0;
        weightA.maxValue = eatMax;
        weightB.maxValue = eatMax;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="amountDelta"></param>
    /// <returns>
    /// 饱食度状态，默认初始1，越饱越高，取值1，2，3，4
    /// </returns>
    public void AddEatAmount(int playerID, float amountDelta)
    {
        if (playerID == 1)
        {
            eatAmountComparison.LeftValue += amountDelta;
            weightA.value += amountDelta;
        }
        if (playerID == 2)
        {
            eatAmountComparison.RightValue += amountDelta;
            weightB.value += amountDelta;
        }
    }
    
    public void SetEatAmount(int playerID, float amount)
    {
        if (playerID == 1)
        {
            eatAmountComparison.LeftValue = amount;
            weightA.value = amount;
        }
        if (playerID == 2)
        {
            eatAmountComparison.RightValue = amount;
            weightB.value = amount;
        }
    }

    public FourLevelSlider GetSlider(int playerID)
    {
        if (playerID == 2) return weightB;
        else return weightA;
    }
}