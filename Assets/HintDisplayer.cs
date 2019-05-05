using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintDisplayer : MonoBehaviour
{
    public GameObject gameController;
    public Texture2D[] hintTextures;
    // Start is called before the first frame update
    void Start()
    {
        int currentLevel = gameController.GetComponent<GameController>().gameLevel;
        int targetLevel = 0;
        if (currentLevel == 1 || currentLevel == 2) targetLevel = 0;
        if (currentLevel == 3 || currentLevel == 4) targetLevel = 1;
        if (currentLevel == 5 || currentLevel == 6) targetLevel = 2;
        if (currentLevel == 7) targetLevel = 3;
        if (currentLevel == 8 || currentLevel == 9) targetLevel = 4;
        Texture2D tex = hintTextures[targetLevel];
        GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
