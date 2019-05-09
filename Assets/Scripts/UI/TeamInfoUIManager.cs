using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamInfoUIManager : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake(){
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("P1 Cancel") || Input.GetButtonDown("P2 Cancel"))
        {
            audioManager.PlayOnceAudioByPath("audio/buttonOnClick");
            Back();
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("Cover");
    }
}
