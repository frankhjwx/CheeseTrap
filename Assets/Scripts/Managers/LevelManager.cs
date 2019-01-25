using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int gameLevel;
    public Sprite[] foreground, shadow, background;
    public GameObject foregroundObject, holeshadowObject, backgroundObject;

    public List<GameObject> level1Miscs;
    public List<GameObject> level2Miscs;
    public List<GameObject> level3Miscs;
    
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
        UpdateMisc();
    }

    void UpdateSprites(){
        foregroundObject.GetComponent<SpriteRenderer>().sprite = foreground[gameLevel];
        holeshadowObject.GetComponent<SpriteRenderer>().sprite = shadow[gameLevel];
        backgroundObject.GetComponent<SpriteRenderer>().sprite = background[gameLevel];
    }

    void UpdateMisc()
    {
        if (gameLevel == 1)
        {
            foreach (GameObject misc in level1Miscs)
            {
                misc.SetActive(true);
            }
        }
        if (gameLevel == 2)
        {
            foreach (GameObject misc in level2Miscs)
            {
                misc.SetActive(true);
            }
        }
        if (gameLevel == 3)
        {
            foreach (GameObject misc in level3Miscs)
            {
                misc.SetActive(true);
            }
        }
    }

}
