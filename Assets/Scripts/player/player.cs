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
    /// <param ></param>
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
            Debug.Log("dig");
            radiusOfHole = 0.05f;
            diggingTime = 0f;
            digging = true;
            holePosition = new Vector2(playerTransform.transform.position.x, playerTransform.transform.position.y) - new Vector2(playerTransform.up.x, playerTransform.up.y) * 0.3f;
            holeID = holeManager.CreateHole(holePosition, radiusOfHole, playerID);
        }
        if (Input.GetKey(KeyCode.Mouse0))//一直按下，持续增大
        {
            digging = true;
            diggingTime += Time.deltaTime;
            radiusOfHole += diggingTime * 0.03f;
            holePosition = new Vector2(holePosition.x, holePosition.y) - new Vector2(playerTransform.up.x, playerTransform.up.y) * diggingTime * 0.03f;
            holeManager.UpdateHole(holeID, holePosition, radiusOfHole, playerID);
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
        float xm = 0;
        float ym = 0;//xy轴的移动量分别存储
        if (Input.GetKey(KeyCode.D) && (!digging))
        {
            xm += playerSpeed * Time.deltaTime;//存储移动量
        }
        if (Input.GetKey(KeyCode.A) && (!digging))
        {
            xm -= playerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) && (!digging))
        {
            ym += playerSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) && (!digging))
        {
            ym -= playerSpeed * Time.deltaTime;
        }

        playerTransform.Translate(new Vector3(xm, ym, 0), Space.World);//实现挪动
        if (Mathf.Abs(xm) >= 0.01f || Mathf.Abs(ym) >= 0.01f)
        {
            playerTransform.transform.up = new Vector3(-xm, -ym, 0);//只有移动了，玩家才会转向
            running = true;//有移动量，则在跑动
        }
        if (Mathf.Abs(xm) == 0f && Mathf.Abs(ym) == 0f)
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
            playerAnimator.SetBool("stay", true);
            playerAnimator.SetBool("dig", false);
        }
        if (running)
        {
            playerAnimator.SetBool("run", true);
            playerAnimator.SetBool("stay", false);
            playerAnimator.SetBool("dig", false);
        }
        if (digging)
        {
            playerAnimator.SetBool("dig", true);
            playerAnimator.SetBool("stay", false);
            playerAnimator.SetBool("run", false);
        }
    }

}
