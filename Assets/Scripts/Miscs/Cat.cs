using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cat : MonoBehaviour
{
    public float xMin, xMax, moveSpeed;
    public float restTime;
    public float waitTime;
    public float patTime;
    private enum CatStatus{Rest, Wait, Pat};
    private CatStatus currentStatus;
    private float timer;
    public GameObject catHand, catAlarm, catHandDown;
    private float posX;

    void Start(){
        currentStatus = CatStatus.Rest;
        timer = Random.Range(0, restTime - 3f);
        SetRest();
    }

    void Update(){
        timer += Time.deltaTime;
        if (currentStatus == CatStatus.Rest && timer > restTime) {
            SetWait();
            timer = 0;
        }
        if (currentStatus == CatStatus.Wait && timer > waitTime) {
            SetPat();
            timer = 0;
        }
        if (currentStatus == CatStatus.Pat && timer > patTime) {
            SetRest();
            timer = Random.Range(0, restTime - 5f);
        }
        Debug.Log(posX);
        transform.position = new Vector3(posX, 5.4f, 0);
    }

    void SetRest(){
        currentStatus = CatStatus.Rest;
        catHand.SetActive(false);
        catAlarm.SetActive(false);
        catHandDown.SetActive(false);
    }

    void SetWait(){
        currentStatus = CatStatus.Wait;
        catHand.SetActive(true);
        catAlarm.SetActive(true);
        catHandDown.SetActive(false);
        StartCoroutine(WaitMove());
    }

    void SetPat(){
        currentStatus = CatStatus.Pat;
        catHand.SetActive(false);
        catAlarm.SetActive(false);
        catHandDown.SetActive(true);
    }

    IEnumerator WaitMove(){
        posX = Random.Range(xMin, xMax);
        bool moveDirection;
        if (Random.value > 0.5) {
            moveDirection = true;
        } else {
            moveDirection = false;
        }
        float waitTimer = 0;
        while (waitTimer < waitTime - 1.5f) {
            if (waitTimer <= 1.0f){
                Color c = catHand.GetComponent<SpriteRenderer>().color;
                catHand.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, waitTimer);
                c = catAlarm.GetComponent<SpriteRenderer>().color;
                catHand.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, waitTimer);
            }
            if (moveDirection) {
                posX += moveSpeed * Time.deltaTime;
                if (posX > xMax) {
                    posX = xMax;
                    moveDirection = false;
                }
            } else {
                posX -= moveSpeed * Time.deltaTime;
                if (posX < xMin) {
                    posX = xMin;
                    moveDirection = true;
                }
            }
            waitTimer += Time.deltaTime;
            yield return 0;
        }
        yield return 0;
    }
}