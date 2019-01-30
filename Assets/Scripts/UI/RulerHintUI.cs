using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RulerHintPage
{
    Page1, 
    Page2, 
    Disappear
}

public class RulerHintUI : MonoBehaviour
{
    [HideInInspector]
    public RulerHintPage currentPage = RulerHintPage.Page1;
    public Image OperstionHint;
    public Image TerrainHint;
    public GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("HintShown", 0) == 0)
        {
            OperstionHint.gameObject.SetActive(true);
            gameController.ShowRulerPause();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (currentPage == RulerHintPage.Page1)
            {
                OperstionHint.gameObject.SetActive(false);
                TerrainHint.gameObject.SetActive(true);
                currentPage = RulerHintPage.Page2;
            }else if (currentPage == RulerHintPage.Page2)
            {
                TerrainHint.gameObject.SetActive(false);
                gameObject.SetActive(false);
                PlayerPrefs.SetInt("HintShown", 1);
                currentPage = RulerHintPage.Disappear;
                gameController.ShowRulerResume();
            }
        }
    }
}
