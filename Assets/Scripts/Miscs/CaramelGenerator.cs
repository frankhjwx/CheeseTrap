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
    private Vector2 dir;

    void Start(){
        timer = 0;
        currentPosition = Vector2.zero;
        currentDirection = 1;
        currentStatus = MovingStatus.Pause;
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
            } else {
                currentPosition = Vector2.Lerp(startPoint2, endPoint2, timer/oneStripeTime);
            }
            holeManager.GetComponent<HoleManager>().GenerateCaramelAtPoint(currentPosition, 20);
            
        }
    }


}