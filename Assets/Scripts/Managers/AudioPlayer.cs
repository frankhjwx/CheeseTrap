using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource Effect;
    public GameObject AudioPrefab;
    public GameObject Audio1;
    public AudioClip eat;
    public AudioClip fat;
    public AudioClip runcheese;
    public AudioClip runice;
    public AudioClip runoil;
    public AudioClip die;
    public AudioClip start;
    public AudioClip end;
    public AudioClip touch;
    public AudioClip click;
    public AudioClip dizzy;
    // Start is called before the first frame update
    void Start()
    {
        AudioPrefab = (GameObject)Resources.Load("Prefabs/SmallAudio");
        Audio1 = Instantiate(AudioPrefab) as GameObject;
        Effect = Audio1.GetComponent<AudioSource>();
    }

    public void PlayAudioClips(string _type)
    {
        switch (_type)
        {
            case "end":
                Effect.Stop();
                break;
            case "eat":
                Effect.clip = eat;
                Effect.Play();
                break;
            case "fat":
                Effect.clip = fat;
                Effect.Play();
                break;
            case "runcheese":
                Effect.clip = runcheese;
                Effect.Play();
                break;
            case "runice":
                Effect.clip = runice;
                Effect.Play();
                break;
            case "runoil":
                Effect.clip = runice;
                Effect.Play();
                break;
          
            case "die":
                Effect.clip = die;
                Effect.Play();
                break;
            case "start":
                Effect.clip = start;
                Effect.Play();
                break;
            case "touch":
                Effect.clip = touch;
                Effect.Play();
                break;
            case "click":
                Effect.clip = click;
                Effect.Play();
                break;
            case "dizzy":
                Effect.clip = dizzy;
                Effect.Play();
                break;
        }
    }
    void Click()
    {
        Effect.clip = click;
        Effect.Play();
    }

}
