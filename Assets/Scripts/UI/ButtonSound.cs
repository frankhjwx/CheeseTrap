using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    // public AudioClip soundOnEnter;
    // public AudioClip soundOnClick;
    private AudioManager audioManager;

    private void Awake(){
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    public void OnPointerEnter(PointerEventData eventData){
        audioManager.PlayOnceAudioByPath("audio/buttonOnEnter");
    }

    public void OnPointerDown(PointerEventData eventData){
        audioManager.PlayOnceAudioByPath("audio/buttonOnClick");
    }
}
