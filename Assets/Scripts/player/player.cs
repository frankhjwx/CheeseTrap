using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class player : MonoBehaviour
{
    private Rigidbody2D playerRigid;
    private GameObject holeManagerG;
    private Transform playerTransform;
    private Collider2D playerCollider;
    private Animator playerAnimator;
    public float playerSpeed = 7.0f;
    private float diggingtime;
    HoleManager Holemanager1;
    bool digging=false;
    bool running;
    public int playerID = 1;
    public float radiusofhole;
    Vector2 holeposition;
    int holeID;
    public enum ControlType//控制方式
    {
        key,
        controller
    }

    ControlType type = ControlType.key;

    void Start()
    {
        playerTransform = gameObject.GetComponent<Transform>();
        playerCollider = gameObject.GetComponent<Collider2D>();
        playerRigid = gameObject.GetComponent<Rigidbody2D>();
        playerAnimator = gameObject.GetComponent<Animator>();
        holeManagerG = GameObject.Find("HoleManager");
        Holemanager1 = holeManagerG.GetComponent<HoleManager>();
    }

    void Update()
    {
        StateManage();
        playAnimator();
        PlayerMove();
    }

    void PlayerMove()//根据控制方式移动
    {
        switch (type)
        {
            case ControlType.key:
                KeyMove();
                break;
            case ControlType.controller:
                ControllerMove();
                break;
        }
    }

    void ControllerMove()
    {

    }
    void KeyMove()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)&&(digging == false))
        {
            Debug.Log("dig");
            radiusofhole = 0.05f;
            diggingtime = 0f;
            digging = true;
            holeposition = new Vector2(playerTransform.transform.position.x, playerTransform.transform.position.y)-new Vector2(playerTransform.up.x, playerTransform.up.y)* 0.3f;
            holeID = Holemanager1.CreateHole(holeposition, radiusofhole, playerID);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            digging = true;
            diggingtime += Time.deltaTime;
            radiusofhole += diggingtime * 0.03f;
            holeposition = new Vector2(holeposition.x, holeposition.y) - new Vector2(playerTransform.up.x, playerTransform.up.y) * diggingtime * 0.03f;
            Holemanager1.UpdateHole(holeID, holeposition, radiusofhole, playerID);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            digging = false;
        }
        float xm = 0;
        float ym = 0;
        if (Input.GetKey(KeyCode.D) && (digging == false))
        {
            xm += playerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) && (digging == false))
        {
            xm -= playerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) && (digging == false))
        {
            ym += playerSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) && (digging == false))
        {
            ym -= playerSpeed * Time.deltaTime;
        }

        playerTransform.Translate(new Vector3(xm, ym, 0), Space.World);
        if (Mathf.Abs(xm) >= 0.01f || Mathf.Abs(ym) >= 0.01f)
        {
            playerTransform.transform.up = new Vector3(-xm, -ym, 0);
        }
    }

    void StateManage()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))&&(digging==false))
        {
            running = true;

        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            running = false;
        }
    }

    void playAnimator()
    {
        if ((running == false) && (digging == false))
        {
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("stay", true);
            playerAnimator.SetBool("dig", false);
        }
        if (running == true)
        {
            playerAnimator.SetBool("run", true);
            playerAnimator.SetBool("stay", false);
            playerAnimator.SetBool("dig", false);
        }
        if (digging == true)
        {
            playerAnimator.SetBool("dig", true);
            playerAnimator.SetBool("stay", false);
            playerAnimator.SetBool("run", false);
        }
    }

}
