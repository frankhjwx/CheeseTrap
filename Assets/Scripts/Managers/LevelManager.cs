using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int gameLevel;
    public Sprite[] foreground, shadow, background;
    public GameObject foregroundObject, holeshadowObject, backgroundObject;

    public List<GameObject> level0Miscs;
    public List<GameObject> level1Miscs;
    public List<GameObject> level2Miscs;
    public List<GameObject> level3Miscs;
    public List<GameObject> level4Miscs;
    public List<GameObject> level5Miscs;
    public List<GameObject> level6Miscs;
    public List<GameObject> level7Miscs;
    public List<GameObject> level8Miscs;
    public List<GameObject> level9Miscs;
    
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
        if (gameLevel == 0)
        {
            foreach (GameObject misc in level0Miscs)
            {
                misc.SetActive(true);
            }
        }
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

        if (gameLevel == 4)
        {
            foreach (GameObject misc in level4Miscs)
            {
                misc.SetActive(true);
            }
        }
        if (gameLevel == 5)
        {
            foreach (GameObject misc in level5Miscs)
            {
                misc.SetActive(true);
            }
        }

        if (gameLevel == 6)
        {
            foreach (GameObject misc in level6Miscs)
            {
                misc.SetActive(true);
            }
        }
        
        if (gameLevel == 7)
        {
            foreach (GameObject misc in level7Miscs)
            {
                misc.SetActive(true);
            }
        }
        
        if (gameLevel == 8)
        {
            foreach (GameObject misc in level8Miscs)
            {
                misc.SetActive(true);
            }
        }
        if (gameLevel == 9)
        {
            foreach (GameObject misc in level9Miscs)
            {
                misc.SetActive(true);
            }
        }
    }

}
