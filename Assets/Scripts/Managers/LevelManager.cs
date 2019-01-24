using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int gameLevel;
    public Sprite[] foreground, shadow, background;
    public GameObject foregroundObject, holeshadowObject, backgroundObject;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameLevel(int level){
        gameLevel = level;
        UpdateSprites();
    }

    void UpdateSprites(){
        foregroundObject.GetComponent<SpriteRenderer>().sprite = foreground[gameLevel];
        holeshadowObject.GetComponent<SpriteRenderer>().sprite = shadow[gameLevel];
        backgroundObject.GetComponent<SpriteRenderer>().sprite = background[gameLevel];
    }

}
