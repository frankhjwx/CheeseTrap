using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoleState
{
    Hidden, 
    Starting,
    Tracking, 
    WaitingForExposing,
    Existing
}

public class TrackingMole : MonoBehaviour
{
    public MoleState state = MoleState.Hidden;

    public Animator moleAnimator;
    public SpriteRenderer moleSprite;
    public player targetPlayer;
    public player anotherPlayer;
    public float trackingSpeed = 1f;
    //开始时长，这段时间及时在范围内也不会撞
    public float startingWaitTime = 1.0f;
    //真正会撞击的追踪时长
    public float trackingTime = 5.0f;
    //撞击前的等待时长
    public float knockWaitingTime = 0.5f;
    //撞击后的存在时间
    public float existingTime = 1.0f;
    //判断追踪到的范围
    public float trackingPosDiff = 0.2f;
    //撞击范围
    public float knockRadius = 0.5f;

    public float appearTime1 = 10.0f;
    private bool appeared1 = false;
    public float appearTime2 = 30.0f;
    private bool appeared2 = false;
    public float appearTime3 = 45.0f;
    private bool appeared3 = false; 
    private float currentTime = 0.0f;
    
    private float startingTimer;
    private float trackingTimer;
    private float knockWaitingTimer;
    private float existingTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        startingTimer = startingWaitTime;
        trackingTimer = trackingTime;
        knockWaitingTimer = knockWaitingTime;
        existingTimer = existingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().isPlaying)
            return;
        if (targetPlayer == null) return;
        if (currentTime > appearTime1 && !appeared1)
        {
            state = MoleState.Starting;
            appeared1 = true;
            Appear();
        }
        if (currentTime > appearTime2 && !appeared2)
        {
            state = MoleState.Starting;
            appeared2 = true;
            Appear();
        }
        if (currentTime > appearTime3 && !appeared3)
        {
            state = MoleState.Starting;
            appeared3 = true;
            Appear();
        }

        currentTime += Time.deltaTime;
        
        if (state == MoleState.Starting || state == MoleState.Tracking)
        {
            Vector3 deltaPos3D = targetPlayer.transform.position - transform.position;
            Vector3 deltaPos = new Vector3(deltaPos3D.x, deltaPos3D.y);
            if (Vector3.Distance(deltaPos, Vector3.zero) < trackingPosDiff)
            {
                if (state == MoleState.Tracking)
                {
                    state = MoleState.WaitingForExposing;
                    moleAnimator.SetTrigger("KnockHit");
                }
            }
            else
            {
                Vector3 direction = Vector3.Normalize(new Vector3(deltaPos.x + 0.01f, deltaPos.y, 0.0f));
                transform.position += direction * trackingSpeed * Time.deltaTime;
            }
        }

        if (state == MoleState.Starting)
        {
            startingTimer -= Time.deltaTime;
            if (startingTimer <= 0.0f) state = MoleState.Tracking;
        }
        else if (state == MoleState.Tracking)
        {
            trackingTimer -= Time.deltaTime;
            if (trackingTimer <= 0.0f)
            {
                state = MoleState.WaitingForExposing;
                moleAnimator.SetTrigger("Knock");
            }
        }
        else if (state == MoleState.WaitingForExposing)
        {
            knockWaitingTimer -= Time.deltaTime;
            if (knockWaitingTimer <= 0.0f)
            {
                state = MoleState.Existing;
                knock();
            }
        }
        else if (state == MoleState.Existing)
        {
            existingTimer -= Time.deltaTime;
            if (existingTimer <= 0.0f)
            {
                state = MoleState.Hidden;
                Hide();
            }
        }
        var localPosition = transform.localPosition;
        localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.y);
        transform.localPosition = localPosition;
    }

    public void resetTimer()
    {
        startingTimer = startingWaitTime;
        trackingTimer = trackingTime;
        knockWaitingTimer = knockWaitingTime;
        existingTimer = existingTime;
    }
    
    public void knock()
    {
        Vector3 deltaPos3D = targetPlayer.transform.position - transform.position;
        Vector3 deltaPos = new Vector3(deltaPos3D.x, deltaPos3D.y);
        if (Vector3.Distance(deltaPos, Vector3.zero) < knockRadius)
        {
            targetPlayer.Vertigo();
        }
    }

    public void Appear()
    {
        transform.position = targetPlayer.transform.position;
        StartCoroutine(Fade(moleSprite, 0, 1, 0.5f));
    }

    public void Hide()
    {
        resetTimer();
        transform.position = new Vector3(-1.0f, -1.0f, 0.0f);
        StartCoroutine(Fade(moleSprite, 1, 0, 0.5f));
    }

    IEnumerator Fade(SpriteRenderer sprite, float initialAlpha, float finalAlpha, float fadeTime)
    {
        float timer = 0.0f;
        while (timer < fadeTime)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, (finalAlpha - initialAlpha) * timer / fadeTime + initialAlpha);
            timer += Time.deltaTime;
            yield return 0;
        }
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, finalAlpha);
    }
}
