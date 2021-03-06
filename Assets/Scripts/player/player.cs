﻿using System.Collections;
using UnityEngine;
using DG.Tweening;

public class player : MonoBehaviour
{
    public enum PlayerStatus{
        MovingNormal, // 在平地移动中
        MovingIce,
        MovingCream,
        MovingCaramel,
        MovingSwamp,
        Dashing, // 冲刺
        Digging, // 挖坑
        Idle, // 静止
        Die, // 死亡
        Vertigo, //眩晕
        Undefined
    }
    public GameController gameController;
    public PlayerStatus currentPlayerStatus;
    private PlayerStatus lastPlayerStatus;
    private Collider2D playerCollider;
    public Animator playerAnimator;
    private bool isVertigo = false;
    private int loopEffectIdx = -1;
    private Tween Dashtween = null;

    public float playerSpeed1 = 5.0f, playerSpeed2 = 4.5f, playerSpeed3 = 4.0f, playerSpeed4 = 3.0f;
    public float thresholdMin = 25000, thresholdMid = 45000, thresholdMax = 70000;
    public float initialRadius = 0.05f;
    public float deltaRadius = 0.15f;
    public float timeStep = 0.2f;
    public float maxRadius = 1.25f;
    public float vertigoTime = 1.0f;
    private MouseSkinManager skinManager;
    
    public float dashCD = 5f;
    private float dashRestTime = 0;
    public float dashDistance = 4f;
    private Color normalColor = new Color(0.5f, 0.5f, 0.5f, 1);
    private Color dashColor = new Color(162f/255, 60f/255, 60f/255, 1);

    // 用于判断老鼠是否死亡的判定区域
    private Vector2 judgeArea = new Vector2(0.3f, 0.2f);
    
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
    public HoleManager holeManager;//挂载另一个脚本的物体
    private bool digging=false;//挖掘状态
    private bool canDig = true;
    bool running;//跑动状态
    bool dashing;
    bool canrun;//是否可跑动
    bool hori;
    bool up;

    // terrain = -1 -> die
    // terrain = 0 -> idle
    // terrain = 1 -> ice
    // terrain = 2 -> cream
    // terrain = 3 -> caramel
    // terrain = 4 -> swamp
    int terrain;
    public int playerID = 1;//用户ID
    private float radiusOfHole;//坑半径
    Vector2 holePosition;//坑位置
    Vector2 initiatePosition;
    private Vector2 currentSpeed;
    private Vector2 acceleration;
    int holeID;//坑的标号

    public InGameCountUI uiPresentation;
    public int hungerState = 1;

    public InputManager InputManager;
    GameObject AnimationManagerG;
    
    public GameObject dustObject;
    public GameObject foodObject;
    public GameObject smokeObject;
    private ParticleSystem.EmissionModule dustEmission;
    private ParticleSystem.EmissionModule foodEmission;
    private ParticleSystem.EmissionModule smokeEmission;

    private float swampFactor = 1;
    private float swampToControlTime = 1;
    private float swampTimer = 0;
    private bool swampPunishmentOn = false;
    public GameObject swampPunishment;
    private AudioManager audioManager;
    public GameObject CDDisplay;
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        skinManager = GetComponent<MouseSkinManager>();
        if (GameObject.FindWithTag("LocalMapChoiceManager"))
        {
            var localMapChoice = GameObject.FindWithTag("LocalMapChoiceManager").GetComponent<MapChoiceManager>();
            var basicInfo = localMapChoice.GetMiceBasicInfo(playerID);
            playerSpeed1 = basicInfo.speedState1;
            playerSpeed2 = basicInfo.speedState2;
            playerSpeed3 = basicInfo.speedState3;
            playerSpeed4 = basicInfo.speedState4;

            thresholdMin = basicInfo.eatThresholdMin;
            thresholdMid = basicInfo.eatThresholdMid;
            thresholdMax = basicInfo.eatThresholdMax;
            dashCD = basicInfo.cd;

            initialRadius = basicInfo.initialRadius;
            deltaRadius = basicInfo.deltaRadius;
            timeStep = basicInfo.timeStep;
            maxRadius = basicInfo.maxRadius;
            
            skinManager.SetSkin(basicInfo.choiceID);
        }
        
        canrun = true;
        playerCollider = gameObject.GetComponent<Collider2D>();


        currentSpeed = Vector2.zero;
        acceleration = Vector2.zero;
        dustEmission = dustObject.GetComponent<ParticleSystem>().emission;
        foodEmission = foodObject.GetComponent<ParticleSystem>().emission;
        smokeEmission = smokeObject.GetComponent<ParticleSystem>().emission;

        hori = true;

        uiPresentation.GetSlider(playerID).level1 = thresholdMin;
        uiPresentation.GetSlider(playerID).level2 = thresholdMid;
        uiPresentation.GetSlider(playerID).level3 = thresholdMax;
        dashing = false;
        SetSelfColor(normalColor);
        CDDisplay.GetComponent<SpriteRenderer>().material.SetFloat("_Progress", 1);

        currentPlayerStatus = PlayerStatus.Undefined;
    }

    private void SetSelfColor(Color c){
        transform.Find("miceanimation").GetComponent<SpriteRenderer>().material.SetColor("_TintColor", c);
        transform.Find("miceanimation/head").GetComponent<SpriteRenderer>().material.SetColor("_TintColor", c);
        transform.Find("miceanimation/shadow").GetComponent<SpriteRenderer>().material.SetColor("_TintColor", c);
    }

    void Update()
    {
        if (gameController.currentStatus == GameController.gameStatus.DisplayHint || gameController.currentStatus == GameController.gameStatus.CountDown) {
            return;
        }

        dustEmission.rateOverTime = 0;
        foodEmission.rateOverTime = 0;
        smokeEmission.rateOverTime = 0;

        CDDisplay.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, -0.75f, 0);

        if (!dashing && (gameController.currentStatus == GameController.gameStatus.Play && gameController.currentStatus != GameController.gameStatus.TimeUpOver)) {
            if (transform.position.x < 0 || transform.position.x > 19.2 || transform.position.y < 0 || transform.position.y > 10.8) {
                gameController.MouseDieGameOver(playerID);
                Debug.Log("PositionDie");
                GameOver();
            } else {
                terrain = holeManager.getTerrainStatus(transform.position);
                if (terrain < 0)
                {
                    // 计算矩形四个顶点的状态，都掉进坑里就判定死亡
                    int terrain1 = holeManager.getTerrainStatus(new Vector2(transform.position.x - judgeArea.x, transform.position.y - judgeArea.y));
                    int terrain2 = holeManager.getTerrainStatus(new Vector2(transform.position.x + judgeArea.x, transform.position.y - judgeArea.y));
                    int terrain3 = holeManager.getTerrainStatus(new Vector2(transform.position.x + judgeArea.x, transform.position.y + judgeArea.y));
                    int terrain4 = holeManager.getTerrainStatus(new Vector2(transform.position.x - judgeArea.x, transform.position.y - judgeArea.y));
                    if (terrain1 + terrain2 + terrain3 + terrain4 == -4) {
                        gameController.MouseDieGameOver(playerID);
                        Debug.Log("HoleDie");
                        GameOver();
                    }
                }
                Dig();
                if(canrun)
                {
                    Move();
                }
                Dash();
            }
        }
        if (gameController.currentStatus == GameController.gameStatus.MouseDieOver || 
            gameController.currentStatus == GameController.gameStatus.TimeUpOver ||
            Time.timeScale == 0) {
            digging = false;
            running = false;
        }
        PlayerAnimation();
        UpdatePlayerStatus();
        
    }

    private bool isRunStatus(PlayerStatus status){
        return (status == PlayerStatus.MovingNormal || status == PlayerStatus.MovingIce || status == PlayerStatus.MovingCaramel ||
                status == PlayerStatus.MovingCream || status == PlayerStatus.MovingSwamp);
    }
    private void UpdateSoundEffect(){
        if (currentPlayerStatus == PlayerStatus.Idle && loopEffectIdx != -1){
            audioManager.StopLoopAudio(loopEffectIdx);
        }
        if (currentPlayerStatus == PlayerStatus.Digging) {
            if (loopEffectIdx != -1) {
                audioManager.StopLoopAudio(loopEffectIdx);
            }
            loopEffectIdx = audioManager.PlayLoopAudioByPath("audio/eat");
        }
        if (!isRunStatus(lastPlayerStatus) && isRunStatus(currentPlayerStatus)){
            if (loopEffectIdx != -1) {
                audioManager.StopLoopAudio(loopEffectIdx);
            }
            loopEffectIdx = audioManager.PlayLoopAudioByPath("audio/run");
        }
    }

    private void UpdatePlayerStatus(){
        lastPlayerStatus = currentPlayerStatus;
        if (running) {
            switch (terrain) {
                case 0:
                    currentPlayerStatus = PlayerStatus.MovingNormal;
                    break;
                case 1:
                    currentPlayerStatus = PlayerStatus.MovingIce;
                    break;
                case 2:
                    currentPlayerStatus = PlayerStatus.MovingCream;
                    break;
                case 3:
                    currentPlayerStatus = PlayerStatus.MovingCaramel;
                    break;
                case 4:
                    currentPlayerStatus = PlayerStatus.MovingSwamp;
                    break;
                default:
                    currentPlayerStatus = PlayerStatus.Undefined;
                    break;
            }
        }
        var moveDirection = new Vector2(Input.GetAxis("P" + playerID + " Horizontal"), Input.GetAxis("P" + playerID + " Vertical"));
        if (canrun && moveDirection == Vector2.zero) {
            currentPlayerStatus = PlayerStatus.Idle;
        }
        if (digging) {
            currentPlayerStatus = PlayerStatus.Digging;
        }
        if (isVertigo) {
            currentPlayerStatus = PlayerStatus.Vertigo;
        }
        if (lastPlayerStatus != currentPlayerStatus) {
            UpdateSoundEffect();
        }
    }

    /// <summary>
    /// 挖坑状态逻辑
    /// </summary>
    void Dig()
    {
        if (Input.GetButtonDown("P" + playerID + " Dig") && (!digging) && canDig)//初次按下鼠标，初始化坑
        {
            //AudioPlayer1.PlayAudioClips("eat");
            digging = true;
            canrun = false;
            diggingTime = 0f;
            radius = initialRadius;//半径初始化
            initiatePosition= dimentionChange(transform.position) + dimentionChange(transform.right) * 0.6f;//坑的坐标在玩家面前

            holeID = holeManager.CreateHole(initiatePosition, radiusOfHole, playerID);//显示坑
            uiPresentation.SetEatAmount(playerID, holeManager.areas[playerID]);
            RefreshHungerState();
            currentSpeed = Vector2.zero;
        }

        if (Input.GetButton(("P" + playerID + " Dig")) && digging && canDig)//一直按下，持续增大
        {
            diggingTime += Time.deltaTime;
            if (diggingTime >= timeStep)
            {
                //AudioPlayer1.PlayAudioClips("eat");
                radius += deltaRadius;
                if (radius >= maxRadius) radius = maxRadius;
                holePosition = initiatePosition + dimentionChange(transform.right) * radius ;//坑坐标向前挪动
                holeManager.UpdateHole(holeID, holePosition, radius, playerID);//坑刷新显示
                diggingTime = 0;
            }

            holeManager.UpdateHole(holeID, holePosition, radiusOfHole, playerID);//坑刷新显示
            uiPresentation.SetEatAmount(playerID, holeManager.areas[playerID]);
            RefreshHungerState();
        }

        if (Input.GetButtonUp("P" + playerID + " Dig") || Mathf.Abs(radius - maxRadius) < 0.01f)//松开鼠标，停止挖掘
        {
            radius = 0.0f;
            digging = false;
            canrun = true;
        }
        if (digging) {
            foodEmission.rateOverTime = 5;
        }
    }

    private void RefreshHungerState()
    {
        int originalHungerState = hungerState;
        if (holeManager.areas[playerID] < thresholdMin) hungerState = 1;
        else if (holeManager.areas[playerID] < thresholdMid) hungerState = 2;
        else if (holeManager.areas[playerID] < thresholdMax) hungerState = 3;
        else hungerState = 4;
        if (originalHungerState != hungerState)
        {
            StartCoroutine(FatterHint(1.5f, 1.0f));
        }
    }

    /// <summary>
    /// 人物移动执行、跑动状态切换
    /// </summary>
    void Move()
    {
        var moveDirection = new Vector2(Input.GetAxis("P" + playerID + " Horizontal"), Input.GetAxis("P" + playerID + " Vertical"));
        if (terrain == 0 || terrain == -1){

            if (moveDirection != Vector2.zero)
            {
                if (Mathf.Abs(moveDirection.y) <= 0f)
                {
                    hori = true;
                }
                else if (moveDirection.y > 0)
                {
                    hori = false;
                    up = true;
                }
                else if (moveDirection.y < 0)
                {
                    hori = false;
                    up = false;
                }
            }

            if (moveDirection != Vector2.zero && !digging)
            {
                transform.Translate(moveDirection * PlayerSpeed * Time.deltaTime, Space.World);//结算并挪动
                //transform.right = moveDirection;
                CorrectDirection(moveDirection);
                running = true;
            }
            else
            {
                running = false;//没有移动量，则不跑动
            }

        }
        // ice
        if (terrain == 1) {
            // having a direction force
            if (moveDirection != Vector2.zero && !digging)
            {
                acceleration = moveDirection.normalized * 5f;
                currentSpeed += acceleration * Time.deltaTime;
                if (currentSpeed.magnitude >= PlayerSpeed){
                    currentSpeed = currentSpeed.normalized * PlayerSpeed;
                }
                transform.Translate(currentSpeed * Time.deltaTime, Space.World);
                //transform.right = moveDirection;
                CorrectDirection(moveDirection);
                running = true;
            }
            else
            {
                running = false;
                // float
                if (currentSpeed.magnitude != 0) {
                    acceleration = -currentSpeed.normalized * 5f;
                    currentSpeed += acceleration * Time.deltaTime;
                    if (currentSpeed.normalized.x * acceleration.normalized.x > 0 || currentSpeed.normalized.y * acceleration.normalized.y > 0)
                        currentSpeed = Vector2.zero;
                    transform.Translate(currentSpeed * Time.deltaTime, Space.World);
                    //transform.right = currentSpeed.normalized;
                }
            }


            if(Mathf.Abs(transform.right.y)<=0.0f)
            {
                hori = true;
            }
            if (transform.right.y < 0)
            {
                hori = false;
                up = false;
            }
            if (transform.right.y > 0)
            {
                hori = false;
                up = true;
            }
        }
        if (terrain == 2){
            if (moveDirection != Vector2.zero && !digging)
            {
                transform.Translate(moveDirection * PlayerSpeed * Time.deltaTime * 0.25f, Space.World);//结算并挪动
                //transform.right = moveDirection;
                CorrectDirection(moveDirection);
                running = true;
                
            }
            else
            {
                running = false;//没有移动量，则不跑动
            }

            if(Mathf.Abs(transform.right.y)<=0.0f)
            {
                hori = true;
            }
            if(transform.right.y > 0)
            {
                hori = false;
                up = true;
            }

            if (transform.right.y < 0)
            {
                hori = false;
                up = false;
            }
        }
        if (terrain == 3){
            if (moveDirection != Vector2.zero && !digging)
            {
                transform.Translate(moveDirection * PlayerSpeed * Time.deltaTime * 2f, Space.World);//结算并挪动
                //transform.right = moveDirection;
                CorrectDirection(moveDirection);
                running = true;
                
            }
            else
            {
                running = false;//没有移动量，则不跑动
            }

            if(Mathf.Abs(transform.right.y)<=0.0f)
            {
                hori = true;
            }
            if(transform.right.y > 0)
            {
                hori = false;
                up = true;
            }

            if (transform.right.y < 0)
            {
                hori = false;
                up = false;
            }


        }

        //swamp
        if (terrain == 4)
        {
            if (swampFactor > 0)
            {
                swampFactor = (swampToControlTime - swampTimer) / swampToControlTime;
                if (moveDirection != Vector2.zero && !digging)
                {
                    transform.Translate(moveDirection * PlayerSpeed * Time.deltaTime * swampFactor, Space.World);//结算并挪动
                    //transform.right = moveDirection;
                    CorrectDirection(moveDirection);
                    running = true;
                
                }
                else
                {
                    running = false;//没有移动量，则不跑动
                }

                if(Mathf.Abs(transform.right.y)<=0.0f)
                {
                    hori = true;
                }
                if(transform.right.y > 0)
                {
                    hori = false;
                    up = true;
                }

                if (transform.right.y < 0)
                {
                    hori = false;
                    up = false;
                }
            }
            else
            {
                if (!swampPunishmentOn)
                {
                    swampFactor = 0;
                    canDig = false;
                    InputManager.instance.StartLeftRightClickCount(playerID, 10);
                    swampPunishmentOn = true;
                }
                else
                {
                    if (InputManager.GetLeftRightClickFinished(playerID))
                    {
                        canrun = true;
                        canDig = true;
                        swampPunishmentOn = false;
                        swampTimer = 0;
                        swampFactor = 1;
                    }
                }
            }

            swampTimer += Time.deltaTime;
            swampPunishment.SetActive(true);
        }
        else
        {
            swampFactor = 1;
            swampTimer = 0;
            swampPunishment.SetActive(false);
        }

        if (running) {
            if (terrain != 3)
                dustEmission.rateOverTime = 12;
            else
                smokeEmission.rateOverTime = 12;
        }

        var localPosition = transform.localPosition;
        localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.y);
        transform.localPosition = localPosition;
    }

    void Dash(){
        if (!digging && canrun && !dashing && dashRestTime == 0 && Input.GetButtonDown("P" + playerID + " Dash")) {
            dashing = true;
            SetSelfColor(dashColor);
            var currentDirection = new Vector2(Input.GetAxis("P" + playerID + " Horizontal"), Input.GetAxis("P" + playerID + " Vertical"));
            if (currentDirection.magnitude <= 0.05f)
            {
                StartCoroutine(DashCoroutine(dashDistance * transform.right));
                /*if (hori)
                {
                    if (transform.localEulerAngles.y == 0) StartCoroutine(DashCoroutine(new Vector2(dashDistance, 0)));
                    else StartCoroutine(DashCoroutine(new Vector2(-dashDistance, 0)));
                }
                else
                {
                    if (up) StartCoroutine(DashCoroutine(new Vector2(0, dashDistance)));
                    else StartCoroutine(DashCoroutine(new Vector2(0, -dashDistance)));
                }*/
            }
            else if (currentDirection.magnitude >= 1)
            {
                StartCoroutine(DashCoroutine(dashDistance * currentDirection.normalized));
            }
            else
            {
                StartCoroutine(DashCoroutine(dashDistance * currentDirection));
            }
        }
    }

    private IEnumerator DashCoroutine(Vector2 dashVector){
        var initPos = transform.localPosition;
        var targetPos = transform.localPosition + new Vector3(dashVector.x, dashVector.y, 0);
        Dashtween = transform.DOMove(targetPos, 0.25f);
        yield return new WaitForSeconds(0.25f);
        dashing = false; 
        SetSelfColor(normalColor);
        dashRestTime = dashCD;
        Dashtween = null;
        Material dashM = CDDisplay.GetComponent<SpriteRenderer>().material;
        dashM.SetFloat("_Progress", 0);
        while (dashRestTime > 0){
            dashRestTime -= Time.fixedDeltaTime;
            if (dashRestTime < 0)
                dashRestTime = 0;
            dashM.SetFloat("_Progress", (dashCD - dashRestTime) / dashCD);
            yield return new WaitForFixedUpdate();
        }
        dashM.SetFloat("_Progress", 1);
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

    /// <summary>
    /// 仓鼠死亡
    /// </summary>
    public void GameOver()
    {
        GetComponent<Collider2D>().enabled = false;
        playerAnimator.SetBool("special", true);
        playerAnimator.SetTrigger("die");
        StartCoroutine(miceDie());
    }

    public void Vertigo()
    {
        StartCoroutine(miceVertigo(vertigoTime));
    }

    IEnumerator miceVertigo(float time)
    {
        canrun = false;
        canDig = false;
        isVertigo = true;
        playerAnimator.SetBool("dizzy", true);
        yield return new WaitForSeconds(time);
        canrun = true;
        canDig = true;
        isVertigo = false;
        playerAnimator.SetBool("dizzy", false);
    }

    IEnumerator miceDie(){
        currentPlayerStatus = PlayerStatus.Die;
        UpdatePlayerStatus();
        yield return new WaitForSecondsRealtime(0.8f);
        Destroy(this.gameObject);
    }

    private void CorrectDirection(Vector3 speedDirection)
    {
        transform.right = speedDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (Dashtween != null) {
            Dashtween.Kill();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        
        if (collider.gameObject.name == "cat_hand_down") {
            StartCoroutine(miceVertigo(GameObject.Find("Cat").GetComponent<Cat>().patTime));
        }
    }

    IEnumerator FatterHint(float factor, float hintTime)
    {
        Vector3 originalScale = transform.localScale;
        float timer = 0.0f;
        while (timer < hintTime / 2)
        {
            transform.localScale = originalScale * (timer / hintTime * 2 * (factor - 1) + 1);
            timer += Time.deltaTime;
            yield return 0;
        }

        while (timer > 0)
        {
            transform.localScale = originalScale * (timer / hintTime * 2 * (factor - 1) + 1);
            timer -= Time.deltaTime;
            yield return 0;
        }

        transform.localScale = originalScale;
    }

    public bool GetDiggingState()
    {
        return digging;
    }
}
