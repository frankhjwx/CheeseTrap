using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int gameLevel;
    public Sprite[] foreground, shadow, background;
    GameObject foregroundObject, holeshadowObject, backgroundObject;
    // Start is called before the first frame update
    void Start()
    {
        foregroundObject = transform.GetChild(0).gameObject;
        holeshadowObject = transform.GetChild(1).gameObject;
        backgroundObject = transform.GetChild(2).gameObject;
        UpdateSprites();
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
