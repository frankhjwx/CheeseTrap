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
    public Sprite cathand_ani0, cathand_ani1, cathand_ani2, cathand_ani3;

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
        StartCoroutine(Pat());
    }

    IEnumerator WaitMove(){
        catHand.GetComponent<SpriteRenderer>().sprite = cathand_ani0;
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
                catAlarm.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, waitTimer);
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
            yield return null;
        }
        while (waitTimer < waitTime - 0.2f) {
            waitTimer += Time.deltaTime;
            yield return null;
        }

        while (waitTimer < waitTime){
            float t = waitTime - waitTimer;
            if (t*4/0.2f >= 1f && t*4/0.2f < 2f) {
                catHand.GetComponent<SpriteRenderer>().sprite = cathand_ani1;
            } else if (t*4/0.2f >= 2f && t*4/0.2f < 3f) {
                catHand.GetComponent<SpriteRenderer>().sprite = cathand_ani2;
            } else if (t*4/0.2f >= 3f && t*4/0.2f < 4f) {
                catHand.GetComponent<SpriteRenderer>().sprite = cathand_ani2;
            }
            waitTimer += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    IEnumerator Pat(){
        float patTimer = 0;
        Color c = catHandDown.GetComponent<SpriteRenderer>().color;
        catHandDown.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 1);

        catHandDown.GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(0.2f);
        catHandDown.GetComponent<Collider2D>().isTrigger = false;
        yield return new WaitForSeconds(patTime - 1.2f);
        while (patTimer < 1.0f){
            catHandDown.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 1 - patTimer);
            patTimer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}