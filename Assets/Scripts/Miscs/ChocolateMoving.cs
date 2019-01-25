using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateMoving : MonoBehaviour
{
    public Vector2 StartPoint;
    public Vector2 EndPoint;
    public float circleTime = 2.0f;

    private float timer = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(StartPoint.x, StartPoint.y, StartPoint.y - 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float ratio = (timer % circleTime) / circleTime * 2;
        if (ratio > 1) ratio = 2 - ratio;
        Debug.Log(ratio);
        Vector2 deltaV = (EndPoint - StartPoint) * ratio;
        transform.localPosition = new Vector3(deltaV.x + StartPoint.x, deltaV.y + StartPoint.y, deltaV.y + StartPoint.y - 0.7f);
    }
}
