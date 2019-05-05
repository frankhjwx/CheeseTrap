using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public player player1;
    public player player2;

    public float maxDist = 15;
    public float width = 4;
    public float maxSpeedAttached = 4;

    //风向1向右，-1向左
    public int direction = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().isPlaying)
            return;
        if (player1 == null || player2 == null) return;
        Vector3 delta1 = player1.transform.position - transform.position;
        Vector3 delta2 = player2.transform.position - transform.position;
        
        if (Mathf.Abs(delta1.y) < width / 2 && Mathf.Abs(delta1.x) < maxDist && !player1.GetDiggingState())
        {
            float factor1 = Mathf.Abs((width / 2 - Mathf.Abs(delta1.y)) / width * 2 * (maxDist - Mathf.Abs(delta1.x)) / maxDist);
            player1.transform.position += (new Vector3(factor1 * maxSpeedAttached * Time.deltaTime, 0.0f, 0.0f)) * direction;
        }
        if (Mathf.Abs(delta2.y) < width / 2 && Mathf.Abs(delta2.x) < maxDist && !player2.GetDiggingState())
        {
            float factor2 = Mathf.Abs((width / 2 - Mathf.Abs(delta2.y)) / width * 2 * (maxDist - Mathf.Abs(delta2.x)) / maxDist);
            player2.transform.position += (new Vector3(factor2 * maxSpeedAttached * Time.deltaTime, 0.0f, 0.0f)) * direction;
        }
    }
}
