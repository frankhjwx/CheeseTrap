using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSkinManager : MonoBehaviour
{

    public Animator playerAnimator;
    public RuntimeAnimatorController normalAnimator;
    public RuntimeAnimatorController capitalistAnimator;
    public RuntimeAnimatorController glassesAnimator;
    public RuntimeAnimatorController sushiAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator.runtimeAnimatorController = normalAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
