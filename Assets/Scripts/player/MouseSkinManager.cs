using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class MouseSkinManager : MonoBehaviour
{

    public enum Skin
    {
        Normal,
        Capitalist,
        Glasses,
        Sushi
    }

    public Animator playerAnimator;
    public RuntimeAnimatorController normalAnimator;
    public RuntimeAnimatorController capitalistAnimator;
    public RuntimeAnimatorController glassesAnimator;
    public RuntimeAnimatorController sushiAnimator;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetSkin(Skin skin)
    {
        switch (skin)
        {
            case Skin.Normal:
                playerAnimator.runtimeAnimatorController = normalAnimator;
                break;
            case Skin.Capitalist:
                playerAnimator.runtimeAnimatorController = capitalistAnimator;
                break;
            case Skin.Glasses:
                playerAnimator.runtimeAnimatorController = glassesAnimator;
                break;
            case Skin.Sushi:
                playerAnimator.runtimeAnimatorController = sushiAnimator;
                break;
        }
    }

    public void SetSkin(int skinId)
    {
        switch (skinId)
        {
            case 0:
                SetSkin(Skin.Normal);
                break;
            case 1:
                SetSkin(Skin.Capitalist);
                break;
            case 2:
                SetSkin(Skin.Glasses);
                break;
            case 3:
                SetSkin(Skin.Sushi);
                break;
        }
    }
}
