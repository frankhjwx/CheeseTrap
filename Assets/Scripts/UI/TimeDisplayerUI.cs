using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplayerUI : MonoBehaviour
{
    public GameObject gameController;
    private int currentTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = gameController.GetComponent<GameController>().GetGameTime();
        GetComponent<Image>().material.SetInt("_GameTime", (int)gameController.GetComponent<GameController>().maxTime - currentTime);
    }
}
