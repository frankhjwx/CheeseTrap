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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 delta1 = player1.transform.position - transform.position;
        Vector3 delta2 = player2.transform.position - transform.position;

        if (delta1.y > width / 2 && delta1.x > maxDist)
        {
            float factor1 = Mathf.Abs(delta1.y / width * 2 * delta1.x / maxDist);
            player1.transform.position += new Vector3(factor1 * maxSpeedAttached * Time.deltaTime, 0.0f, 0.0f);
        }
        if (delta2.y > width / 2 && delta2.x > maxDist)
        {
            float factor2 = Mathf.Abs(delta2.y / width * 2 * delta2.x / maxDist);
            player2.transform.position += new Vector3(factor2 * maxSpeedAttached * Time.deltaTime, 0.0f, 0.0f);
        }
    }
}
