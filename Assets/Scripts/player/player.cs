﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private GameObject holeManagerObject;
    private Collider2D playerCollider;
    private Animator playerAnimator;

    public float playerSpeed1 = 5.0f, playerSpeed2 = 4.5f, playerSpeed3 = 4.0f, playerSpeed4 = 3.0f;
    
    public float PlayerSpeed
    {
        get
        {
            if (hungerState == 1) return playerSpeed1;
            else if (hungerState == 2) return playerSpeed2;
            else if (hungerState == 3) return playerSpeed3;
            else if (hungerState == 4) return playerSpeed4;
            else return playerSpeed4;
        }
    }//跑动速度
    private float diggingTime;//挖坑计时器
    private float radius;
    HoleManager holeManager;//挂载另一个脚本的物体
    bool digging=false;//挖掘状态
    bool running;//跑动状态
    bool canrun;//是否可跑动
    bool hori;
    bool up;
    int terrain;
    public int playerID = 1;//用户ID
    private float radiusOfHole;//坑半径
    public float maxRadiusOfHole = 1.25f;
    Vector2 holePosition;//坑位置
    Vector2 initiatePosition;
    int holeID;//坑的标号

    public InGameCountUI uiPresentation;
    public int hungerState = 1;

    GameObject InputManagerG;
    InputManager InputManager;
    GameObject AnimationManagerG;
    void Start()
    {
        canrun = true;
        playerCollider = gameObject.GetComponent<Collider2D>();

        AnimationManagerG= GameObject.Find("miceanimation");
        playerAnimator = AnimationManagerG.GetComponent<Animator>();

        holeManagerObject = GameObject.Find("HoleManager");
        holeManager = holeManagerObject.GetComponent<HoleManager>();
        InputManagerG = GameObject.Find("InputManager");
        InputManager = InputManagerG.GetComponent<InputManager>();

        hori = true;
    }

    void Update()
    {
        terrain = holeManager.getTerrainStatus(transform.position);
        if(terrain<0)
        {
            Debug.Log("youdie!");
        }
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
            radius = 0.05f;//半径初始化
            initiatePosition= dimentionChange(transform.position) + dimentionChange(transform.right) * 0.6f;//坑的坐标在玩家面前

            holeID = holeManager.CreateHole(initiatePosition, radiusOfHole, playerID);//显示坑
            hungerState = uiPresentation.SetEatAmount(playerID, holeManager.areas[playerID]);
        }

        if (InputManager.instance.GetDigKey(playerID) && digging)//一直按下，持续增大
        {
            diggingTime += Time.deltaTime;
            if (diggingTime >= 0.2f)
            {
                radius += 0.15f;
                if (radius >= maxRadiusOfHole) radius = maxRadiusOfHole;
                Debug.Log("refresh");
                holePosition = initiatePosition + dimentionChange(transform.right) * radius ;//坑坐标向前挪动
                holeManager.UpdateHole(holeID, holePosition, radius, playerID);//坑刷新显示
                diggingTime = 0;
            }

            holeManager.UpdateHole(holeID, holePosition, radiusOfHole, playerID);//坑刷新显示
            hungerState = uiPresentation.SetEatAmount(playerID, holeManager.areas[playerID]);
        }

        if (InputManager.instance.GetDigKeyUp(playerID) || Mathf.Abs(radius - maxRadiusOfHole) < 0.01f)//松开鼠标，停止挖掘
        {
            radius = 0.0f;
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
            transform.Translate(moveDirection * PlayerSpeed * Time.deltaTime, Space.World);//结算并挪动
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
            up = false;
        }

        if (transform.right.y>0)
        {
            hori = false;
            up = true;
        }

        if (transform.right.y < 0)
        {
            hori = false;
            up = false;
        }
        Debug.Log("up:"+up);
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

        if ((!running) && (!digging) && (!hori)&&up)
        {
            playerAnimator.SetBool("hori", false);
            playerAnimator.SetBool("run" , false );
            playerAnimator.SetBool("dig" , false);
            playerAnimator.SetBool("up", true);
        }

        if ((!running) && (!digging) && (!hori)&&(!up))
        {
            playerAnimator.SetBool("hori", false);
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("dig", false);
            playerAnimator.SetBool("up", false);
        }

        if ((running) && hori)
        {
            playerAnimator.SetBool("hori", true);
            playerAnimator.SetBool("run", true);
            playerAnimator.SetBool("dig", false);
        }

        if ((running) && !hori&&up)
        {
            playerAnimator.SetBool("hori", false);
            playerAnimator.SetBool("run", true);
            playerAnimator.SetBool("dig", false);
            playerAnimator.SetBool("up", true);
        }

        if ((running) && !hori&&!up)
        {
            playerAnimator.SetBool("hori", false);
            playerAnimator.SetBool("run", true);
            playerAnimator.SetBool("dig", false);
            playerAnimator.SetBool("up", false);
        }

        if ((digging) && hori)
        {
            playerAnimator.SetBool("hori", true);
            playerAnimator.SetBool("dig", true);
            playerAnimator.SetBool("run", false);
        }

        if ((digging) && !hori&&up)
        {
            playerAnimator.SetBool("hori", false);
            playerAnimator.SetBool("dig", true);
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("up", true);

        }

        if ((digging) && !hori&&!up)
        {
            playerAnimator.SetBool("hori", false);
            playerAnimator.SetBool("dig", true);
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("up", false);
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
