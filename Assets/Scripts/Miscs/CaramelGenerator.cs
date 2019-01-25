using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaramelGenerator : MonoBehaviour
{
    public float pauseTime;
    public Vector2 startPoint1, endPoint1;
    public Vector2 startPoint2, endPoint2;
    public float oneStripeTime;
    private Vector2 currentPosition;
    private float timer;
    private int currentDirection;

    private enum MovingStatus {Pause, Moving};
    private MovingStatus currentStatus;
    public GameObject holeManager;
    private GameObject machine;
    private Vector2 dir;

    void Start(){
        timer = 0;
        currentPosition = Vector2.zero;
        currentDirection = 1;
        currentStatus = MovingStatus.Pause;
        machine = transform.GetChild(0).gameObject;
    }

    void Update(){
        timer += Time.deltaTime;
        if (currentStatus == MovingStatus.Pause){
            if (timer >= pauseTime) {
                currentStatus = MovingStatus.Moving;
                timer = 0;
            }
        }
        if (currentStatus == MovingStatus.Moving){
            if (timer >= oneStripeTime) {
                if (currentDirection == 1){
                    currentDirection = 2;
                } else
                {
                    currentDirection = 1;
                }
                currentStatus = MovingStatus.Pause;
                timer = 0;
            }
            if (currentDirection == 1){
                currentPosition = Vector2.Lerp(startPoint1, endPoint1, timer/oneStripeTime);
                machine.transform.position = new Vector2((float)currentPosition.x/50.0f + 3f, (float)currentPosition.y/50.0f + 2.7f);
                machine.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            } else {
                currentPosition = Vector2.Lerp(startPoint2, endPoint2, timer/oneStripeTime);
                machine.transform.position = new Vector2((float)currentPosition.x/50.0f - 3f, (float)currentPosition.y/50.0f + 2.7f);
                machine.transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
            }
            holeManager.GetComponent<HoleManager>().GenerateCaramelAtPoint(currentPosition, 20);
            
            
        }
    }


}