﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PockyRotating : MonoBehaviour
{
    public float rotateSpeed = 60.0f;

    public Transform pockyRelative;

    public Transform pockyShadow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("GameController").GetComponent<GameController>().isPlaying)
            return;
        pockyRelative.Rotate(new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime);
        pockyShadow.rotation = pockyRelative.rotation;
    }
}
