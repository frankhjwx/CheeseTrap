using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterSlider : MonoBehaviour
{
    public Slider leftSlider;
    public Slider rightSlider;
    public float leftValue = 0;
    public float rightValue = 0;

    private float currentPosition;
    private float truePosition;
    
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
        
    }
}
