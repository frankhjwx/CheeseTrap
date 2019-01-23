using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCountUI : MonoBehaviour
{

    public Slider eatAmountComparison;
    public Slider weightA, weightB;
    
    // Start is called before the first frame update
    void Start()
    {
        eatAmountComparison.value = 0.5f;
        weightA.value = 0;
        weightB.value = 0;
    }

    public void AddEatAmount(int playerID, float amountDelta)
    {
        
    }
}
