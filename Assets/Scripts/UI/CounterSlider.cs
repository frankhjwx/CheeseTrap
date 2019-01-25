using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterSlider : MonoBehaviour
{
    public Slider leftSlider;
    public Slider rightSlider;

    private float leftValue = 0;
    public float LeftValue
    {
        get { return leftValue; }
        set
        {
            leftValue = value;
            RefreshTruePosition();
        }
    }

    private float rightValue = 0;
    public float RightValue
    {
        get { return rightValue; }
        set
        {
            rightValue = value;
            RefreshTruePosition();
        }
    }

    private float currentPosition = 0.5f;
    private float truePosition = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        leftSlider.minValue = 0;
        leftSlider.maxValue = 1;
        rightSlider.minValue = 0;
        rightSlider.maxValue = 1;
        leftSlider.value = 0.5f;
        rightSlider.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(currentPosition - truePosition) > 0.001f)
        {
            currentPosition = Mathf.Lerp(currentPosition, truePosition, 0.3f);
        }
        else
        {
            currentPosition = truePosition;
        }
        RefreshSliderPosition();
    }

    private void RefreshTruePosition()
    {
        if (LeftValue < 0.01 && RightValue < 0.01) truePosition = 0.5f;
        else if (LeftValue < 0.01 && RightValue >= 0.01) truePosition = 0.0f;
        else if (LeftValue >= 0.01 && RightValue < 0.01) truePosition = 1.0f;
        else truePosition = LeftValue / (LeftValue + RightValue);
    }
    
    private void RefreshSliderPosition()
    {
        leftSlider.value = currentPosition;
        rightSlider.value = 1 - currentPosition;
    }
}
