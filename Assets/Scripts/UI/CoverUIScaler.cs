using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoverUIScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform backgroundNote;
    private Vector3 initialScale;
    private Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
    private float scalerTime = 0.2f;
    private float timer = 0.0f;
    private bool pointerIn = false;
    private int initialSiblingIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        initialScale = backgroundNote.localScale;
        initialSiblingIndex = backgroundNote.GetSiblingIndex();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (pointerIn)
        {
            if (timer >= scalerTime)
            {
                timer = scalerTime;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            if (timer <= 0)
            {
                timer = 0;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        backgroundNote.localScale = initialScale + (targetScale - initialScale) * timer / scalerTime;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIn = true;
        backgroundNote.SetSiblingIndex(2);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        pointerIn = false;
        backgroundNote.SetSiblingIndex(initialSiblingIndex);
    }
}
