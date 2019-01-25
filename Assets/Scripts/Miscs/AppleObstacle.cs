using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleObstacle : MonoBehaviour
{

    public HoleManager holeManager;
    public Vector2 disappearPoint;
    
    void Update()
    {
        if (holeManager.getTerrainStatus(disappearPoint) == -1)
        {
            Destroy(this.gameObject);
        }
    }

}
