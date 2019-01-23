using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private GameObject holeManagerObject;
    private Collider2D playerCollider;
    private Animator playerAnimator;

    public float playerSpeed = 7.0f;//跑动速度
    private float diggingTime;//挖坑计时器
    HoleManager holeManager;//挂载另一个脚本的物体
    bool digging=false;//挖掘状态
    bool running;//跑动状态
    bool canrun;//是否可跑动
    bool hori;

    public int playerID = 1;//用户ID
    public float radiusOfHole;//坑半径
    Vector2 holePosition;//坑位置
    int holeID;//坑的标号

    void Start()
    {
        canrun = true;
        playerCollider = gameObject.GetComponent<Collider2D>();
        playerAnimator = gameObject.GetComponent<Animator>();

        holeManagerObject = GameObject.Find("HoleManager");
        holeManager = holeManagerObject.GetComponent<HoleManager>();

        hori = true;
    }

    void Update()
    {
        Dig();
        PlayerAnimation();
        if(canrun)
        {
            Move();
        }
    }

    /// <summary>
    /// 挖坑状态逻辑
    /// </summary>
    void Dig()
    {
        if (InputManager.instance.GetDigKeyDown(playerID) && (!digging))//初次按下鼠标，初始化坑
        {
            digging = true;
            canrun = false;
            diggingTime = 0f;
            radiusOfHole = 0.05f;//半径初始化
            holePosition = dimentionChange(transform.position) + dimentionChange(transform.right) * 0.6f;//坑的坐标在玩家面前

            holeID = holeManager.CreateHole(holePosition, radiusOfHole, playerID);//显示坑
        }

        if (InputManager.instance.GetDigKey(playerID))//一直按下，持续增大
        {
            diggingTime += Time.deltaTime;
            radiusOfHole += diggingTime * 0.03f;//半径增大
            holePosition = holePosition + dimentionChange(transform.right) * diggingTime * 0.03f;//坑坐标向前挪动

            holeManager.UpdateHole(holeID, holePosition, radiusOfHole, playerID);//坑刷新显示
        }

        if (InputManager.instance.GetDigKeyUp(playerID))//松开鼠标，停止挖掘
        {
            digging = false;
            canrun = true;
        }
    }

    /// <summary>
    /// 人物移动执行、跑动状态切换
    /// </summary>
    void Move()
    {
        var moveDirection = InputManager.instance.GetAxis(playerID);
        if (moveDirection != Vector2.zero && !digging)
        {
            transform.Translate(moveDirection * playerSpeed * Time.deltaTime, Space.World);//结算并挪动
            transform.right = moveDirection;
            running = true;
        }
        else
        {
            running = false;//没有移动量，则不跑动
        }

        if(transform.right.y==0)
        {
            hori = true;
        }
        if(transform.right.y!=0)
        {
            hori = false;
        }
    }

    /// <summary>
    /// 控制状态机参数
    /// </summary>
    void PlayerAnimation()
    {
        if ((!running) && (!digging)&&hori)
        {
            playerAnimator.SetBool("hori",true);
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("dig", false);
        }

        if ((!running) && (!digging) && (!hori))
        {
            playerAnimator.SetBool("hori", false);
            playerAnimator.SetBool("run" , false );
            playerAnimator.SetBool("dig" , false);
        }
         
        if ((running) && hori)
        {
            playerAnimator.SetBool("hori", true);
            playerAnimator.SetBool("run", true);
            playerAnimator.SetBool("dig", false);
        }

        if ((running) && !hori)
        {
            playerAnimator.SetBool("hori", false);
            playerAnimator.SetBool("run", true);
            playerAnimator.SetBool("dig", false);
        }

        if ((digging) && hori)
        {
            playerAnimator.SetBool("hori", true);
            playerAnimator.SetBool("dig", true);
            playerAnimator.SetBool("run", false);
        }

        if ((digging) && !hori)
        {
            playerAnimator.SetBool("hori", false);
            playerAnimator.SetBool("dig", true);
            playerAnimator.SetBool("run", false);
        }
    }

    /// <summary>
    /// 3D坐标转2D
    /// </summary>
    /// <param name="3D坐标"></param>
    /// <returns>2D坐标</returns>
    public Vector2 dimentionChange(Vector3 d3Position)
    {
        Vector2 d2Position=new Vector2(d3Position.x,d3Position.y);
        return d2Position;
    }
}
