using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoverUIScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CoverUIController.CoverButton coverButton;
    public CoverUIController coverUiController;
    public RectTransform backgroundNote;
    private Vector3 initialScale;
    private Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
    private float scalerTime = 0.2f;
    private float timer = 0.0f;
    private bool pointerIn = false;
    private bool handlerChoosed = false;
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
        
        if (pointerIn || handlerChoosed)
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
        coverUiController.RefreshScaler();
        backgroundNote.SetSiblingIndex(2);
        coverUiController.currentButton = coverButton;
        pointerIn = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        coverUiController.RefreshScaler();
        backgroundNote.SetSiblingIndex(initialSiblingIndex);
        coverUiController.currentButton = CoverUIController.CoverButton.NULL;
        pointerIn = false;
    }

    public void GamePadChoose()
    {
        backgroundNote.SetSiblingIndex(2);
        handlerChoosed = true;
        pointerIn = false;
        coverUiController.currentButton = coverButton;
    }

    public void GamePadOut()
    {
        backgroundNote.SetSiblingIndex(initialSiblingIndex);
        handlerChoosed = false;
        pointerIn = false;
    }
}
