using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEater : MonoBehaviour
{
    private int holeID;
    private int cols = 10;
    private int rows = 6;
    private float maxRadius = 2.5f;
    private float timeStep = 0.1f;
    private float deltaRadius = 0.5f;
    private float currentRadius = 0;
    public GameObject holeManager;
    Vector2 position;
    // Start is called before the first frame update
    void Start()
    {
        holeManager.GetComponent<HoleManager>().InitializeLevel(0);
        StartCoroutine(EatAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EatAnimation(){
        for (int j = 0; j < rows; j++){
            for (int i = 0; i < cols; i++){
                position.x = 19.2f / (cols-1) * i;
                position.y = 10.8f / (rows-1) * j;
                while (currentRadius <= maxRadius){
                    holeManager.GetComponent<HoleManager>().CreateHole(position, currentRadius, 1);
                    currentRadius += deltaRadius;
                    yield return new WaitForSeconds(timeStep);
                }
                timeStep -= 0.005f;
                currentRadius = 0;
            }
        }
    }
}
