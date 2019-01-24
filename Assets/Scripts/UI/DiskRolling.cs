using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskRolling : MonoBehaviour
{

    public float angularVelocity = 100f;
    private RectTransform selfTransform; 
    // Start is called before the first frame update
    void Start()
    {
        selfTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        selfTransform.localEulerAngles += new Vector3(0.0f, 0.0f, angularVelocity * Time.deltaTime);
    }
}
