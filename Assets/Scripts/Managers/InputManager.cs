using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;
    private Vector2 player1Vector, player2Vector;
    public bool player1DigKeyDown, player2DigKeyDown;
    public bool player1DigKey, player2DigKey;
    public bool player1DigKeyUp, player2DigKeyUp;

    private KeyCode[] directionKeys1 = {KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D};
    private KeyCode[] directionKeys2 = {KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow};
    private KeyCode DigKey1 = KeyCode.Space, DigKey2 = KeyCode.Return;

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
	}

    // playerID = 1 or 2
    public Vector2 GetAxis(int playerID){
        if (playerID == 1) {
            return player1Vector;
        }
        if (playerID == 2) {
            return player2Vector;
        }
        throw new Exception("player ID can only be 1 or 2.");
    }

}