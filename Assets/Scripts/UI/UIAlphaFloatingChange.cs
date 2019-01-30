using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class UIAlphaFloatingChange : MonoBehaviour
{

    public float alphaFrom = 1.0f;
    public float alphaTo = 0.5f;
    public float onewayTime = 1.0f;
    public bool round = true;
    private Image hintImage; 
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        hintImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentAlpha = 1;
        float factor = (timer % (onewayTime * 2)) / onewayTime;
        if (factor < 1)
        {
            currentAlpha = alphaFrom + (alphaTo - alphaFrom) * factor;
        }
        else
        {
            if (round)
            {
                currentAlpha = alphaFrom + (alphaTo - alphaFrom) * (2 - factor);
            }
            else
            {
                currentAlpha = alphaFrom + (alphaTo - alphaFrom) * (factor - 1);
            }
        }

        var color = hintImage.color;
        color = new Color(color.r, color.g, color.b, currentAlpha);
        hintImage.color = color;
        timer += Time.deltaTime;
    }
}
