using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Music;
    AudioManager AudioManagerG;
    // Start is called before the first frame update
    void Start()
    {
        AudioManagerG = this.GetComponent<AudioManager>();
        
        if(AudioManagerG.tag==default)
        {
            AudioManagerG.tag = "add";
            DontDestroyOnLoad(AudioManagerG);
        }
    }


}
