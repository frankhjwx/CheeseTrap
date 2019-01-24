using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FourLevelSlider : MonoBehaviour
{
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider slider4;

    public float level1;
    public float level2;
    public float level3;
    public float level4;

    float testAmount;
    /// <summary>
    /// slider刷新显示
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="摄入总量"></param>

    private void Update()
    {
        testAmount += 0.1f;
        SliderReflesh(testAmount);
        Debug.Log(testAmount);
    }
    public void SliderReflesh(float eatAmount)
    {
        slider1.maxValue = level1;
        slider2.maxValue = level2-level1;
        slider3.maxValue = level3-level2;
        slider4.maxValue = level4-level3;
        
        if(eatAmount<=level1)
        {
            slider1.value = eatAmount;
        }

        if((eatAmount > level1)&&( eatAmount < level2))
        {

            slider2.value = eatAmount-level1;
        }
        if ((eatAmount > level2) && (eatAmount < level3))
        {

            slider3.value = eatAmount-level2;
        }
        if ((eatAmount > level3) && (eatAmount < level4))
        {

            slider4.value = eatAmount-level3;
        }
    }

}
