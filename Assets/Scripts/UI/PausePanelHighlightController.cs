using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelHighlightController : MonoBehaviour
{
    public Image buttonImage;
    public GameObject highlightEffect;

    private void Start()
    {
        GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void Highlight()
    {
        buttonImage.enabled = true;
        highlightEffect.SetActive(true);
    }

    public void Unhighlight()
    {
        buttonImage.enabled = true;
        highlightEffect.SetActive(false);
    }
}
