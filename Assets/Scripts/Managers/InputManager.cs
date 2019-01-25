using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;
    private Vector2 player1Vector, player2Vector;
    [HideInInspector]
    private bool player1DigKeyDown = false, player2DigKeyDown = false;
    [HideInInspector]
    private bool player1DigKey = false, player2DigKey = false;
    [HideInInspector]
    private bool player1DigKeyUp = false, player2DigKeyUp = false;

    private KeyCode[] directionKeys1 = {KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D};
    private KeyCode[] directionKeys2 = {KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow};
    private KeyCode DigKey1 = KeyCode.Space, DigKey2 = KeyCode.Return;

    private int leftRightClickCountRequest = 0;
    private int leftRightClickCounter = 0;
    private bool leftRightClickMode = false;
    //-1为左， 1 为右，0为未开始按
    private int leftRightClickLatestKey = 0;
    public bool leftRightClickFinished = true;
    
    private void Awake()
    {
        instance = this;

    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(directionKeys1[2]) && !Input.GetKey(directionKeys1[3])) {
            player1Vector.x = -1;
        } else if (Input.GetKey(directionKeys1[3]) && !Input.GetKey(directionKeys1[2])) {
            player1Vector.x = 1;
        } else {
            player1Vector.x = 0;
        }
        if (Input.GetKey(directionKeys1[0]) && !Input.GetKey(directionKeys1[1])) {
            player1Vector.y = 1;
        } else if (Input.GetKey(directionKeys1[1]) && !Input.GetKey(directionKeys1[0])) {
            player1Vector.y = -1;
        } else {
            player1Vector.y = 0;
        }
        
        if (Input.GetKey(directionKeys2[2]) && !Input.GetKey(directionKeys2[3])) {
            player2Vector.x = -1;
        } else if (Input.GetKey(directionKeys2[3]) && !Input.GetKey(directionKeys2[2])) {
            player2Vector.x = 1;
        } else {
            player2Vector.x = 0;
        }
        if (Input.GetKey(directionKeys2[0]) && !Input.GetKey(directionKeys2[1])) {
            player2Vector.y = 1;
        } else if (Input.GetKey(directionKeys2[1]) && !Input.GetKey(directionKeys2[0])) {
            player2Vector.y = -1;
        } else {
            player2Vector.y = 0;
        }
        

        if (Input.GetKeyUp(DigKey1))
            player1DigKeyUp = true;
        else
            player1DigKeyUp = false;
        
        if (Input.GetKey(DigKey1))
            player1DigKey = true;
        else
            player1DigKey = false;

        if (Input.GetKeyDown(DigKey1))
            player1DigKeyDown = true;
        else
            player1DigKeyDown = false;
        

        if (Input.GetKeyUp(DigKey2))
            player2DigKeyUp = true;
        else
            player2DigKeyUp = false;
        
        if (Input.GetKey(DigKey2))
            player2DigKey = true;
        else
            player2DigKey = false;

        if (Input.GetKeyDown(DigKey2))
            player2DigKeyDown = true;
        else
            player2DigKeyDown = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow) && leftRightClickMode && (leftRightClickLatestKey == 1 || leftRightClickLatestKey == 0))
        {
            leftRightClickCounter++;
            leftRightClickLatestKey = -1;
            if (leftRightClickCounter >= leftRightClickCountRequest)
            {
                leftRightClickMode = false;
                leftRightClickFinished = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && leftRightClickMode && (leftRightClickLatestKey == -1 || leftRightClickLatestKey == 0))
        {
            leftRightClickCounter++;
            leftRightClickLatestKey = 1;
            if (leftRightClickCounter >= leftRightClickCountRequest)
            {
                leftRightClickMode = false;
                leftRightClickFinished = true;
            }
        }
	}

    // playerID = 1 or 2
    public Vector2 GetAxis(int playerID){
        if (playerID == 1) {
            return Vector3.Normalize(player1Vector);
        }
        if (playerID == 2) {
            return Vector3.Normalize(player2Vector);
        }
        return Vector2.zero;
    }

    public bool GetDigKeyDown(int playerID)
    {
        if (playerID == 1)
        {
            return player1DigKeyDown;
        }

        if (playerID == 2)
        {
            return player2DigKeyDown;
        }

        return false;
    }

    public bool GetDigKey(int playerID)
    {
        if (playerID == 1) return player1DigKey;
        if (playerID == 2) return player2DigKey;
        return false;
    }

    public bool GetDigKeyUp(int playerID)
    {
        if (playerID == 1) return player1DigKeyUp;
        if (playerID == 2) return player2DigKeyUp;
        return false;
    }

    public bool GetRestart(){
        return Input.GetKey(KeyCode.Escape);
    }

    public void StartLeftRightClickCount(int countTime)
    {
        leftRightClickCountRequest = countTime;
        leftRightClickMode = true;
        leftRightClickLatestKey = 0;
        leftRightClickCounter = 0;
        leftRightClickFinished = false;
    }

}