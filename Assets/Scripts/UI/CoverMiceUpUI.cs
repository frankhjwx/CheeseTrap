using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoverMiceUpUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CoverUIController.CoverButton coverButton;
    public CoverUIController coverUiController;
    private Vector3 initialPosition;
    public Vector3 enterDelta;
    public float floatTime = 1.0f;
    private float timer = 0;
    private bool pointIn = false;
    private bool handlerChoosed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (pointIn || handlerChoosed)
        {
            if (timer >= floatTime)
            {
                timer = floatTime;
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
        transform.localPosition = initialPosition + enterDelta * timer / floatTime;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        coverUiController.currentButton = coverButton;
        coverUiController.RefreshScaler();
        pointIn = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        pointIn = false;
        coverUiController.currentButton = CoverUIController.CoverButton.NULL;
    }
    
    public void GamePadChoose()
    {
        handlerChoosed = true;
        pointIn = false;
        coverUiController.currentButton = coverButton;
    }

    public void GamePadOut()
    {
        handlerChoosed = false;
        pointIn = false;
    }
}