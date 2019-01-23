using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private GameObject holeManagerObject;
    private Transform playerTransform;
    private Collider2D playerCollider;
    private Animator playerAnimator;

    public float playerSpeed = 7.0f;//跑动速度
    private float diggingTime;//挖坑计时器
    HoleManager holeManager;//挂载另一个脚本的物体
    bool digging=false;//挖掘状态
    bool running;//跑动状态
    public int playerID = 1;//用户ID
    public float radiusOfHole;//坑半径
    Vector2 holePosition;//坑位置
    int holeID;//坑的标号

    void Start()
    {
        playerTransform = gameObject.GetComponent<Transform>();
        playerCollider = gameObject.GetComponent<Collider2D>();
        playerAnimator = gameObject.GetComponent<Animator>();

        holeManagerObject = GameObject.Find("HoleManager");
        holeManager = holeManagerObject.GetComponent<HoleManager>();
    }

    void Update()
    {
        Dig();
        PlayerAnimation();
        Move();
    }

    /// <summary>
    /// 挖坑状态逻辑
    /// </summary>
    void Dig()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (!digging))//初次按下鼠标，初始化坑
        {
            digging = true;

            diggingTime = 0f;
            radiusOfHole = 0.05f;//半径初始化
            holePosition = dimentionChange(playerTransform.transform.position) - dimentionChange(playerTransform.up) * 0.3f;//坑的坐标在玩家面前

            holeID = holeManager.CreateHole(holePosition, radiusOfHole, playerID);//显示坑
        }

        if (Input.GetKey(KeyCode.Mouse0))//一直按下，持续增大
        {
            diggingTime += Time.deltaTime;
            radiusOfHole += diggingTime * 0.03f;//半径增大
            holePosition = holePosition - dimentionChange(playerTransform.up) * diggingTime * 0.03f;//坑坐标向前挪动

            holeManager.UpdateHole(holeID, holePosition, radiusOfHole, playerID);//坑刷新显示
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))//松开鼠标，停止挖掘
        {
            digging = false;
        }
    }

    /// <summary>
    /// 人物移动执行、跑动状态切换
    /// </summary>
    void Move()
    {
        float horizonD = Input.GetAxis("Horizontal");
        float vertiD = Input.GetAxis("Vertical");
        if (horizonD != 0 || vertiD != 0)
        {
            Vector3 directionMove = Vector3.Normalize(new Vector3(horizonD, vertiD));
            
            playerTransform.Translate(directionMove * playerSpeed * Time.deltaTime, Space.World); //结算并挪动
            playerTransform.transform.up = -directionMove; //只有移动了，玩家才会转向
            running = true; //有移动量，则在跑动
        }
        else
        {
            running = false;//没有移动量，则不跑动
        }
    }

    /// <summary>
    /// 控制状态机参数
    /// </summary>
    void PlayerAnimation()
    {
        if ((!running) && (!digging))
        {
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("dig", false);
        }
        if (running)
        {
            playerAnimator.SetBool("run", true);
            playerAnimator.SetBool("dig", false);
        }
        if (digging)
        {
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
