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
    public AudioClip goalSum;
    public AudioClip chocolate;
    public AudioClip runIce;
    public AudioClip runOil;
    public AudioClip die;
    public AudioClip start;
    public AudioClip end;
    public AudioClip click;
    public AudioClip dizzy;
    public AudioClip catClaw;
    public AudioClip wind;
    public AudioClip mole;
    public AudioClip countDown;
    // Start is called before the first frame update
    void Start()
    {
        AudioPrefab = (GameObject)Resources.Load("Prefabs/SmallAudio");
        Audio1 = Instantiate(AudioPrefab) as GameObject;
        Effect = Audio1.GetComponent<AudioSource>();
    }

    public void PlayAudio(string _type)
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
            case "chocolate":
                Effect.clip = chocolate;
                Effect.Play();
                break;
            case "goalSum":
                Effect.clip = goalSum;
                Effect.Play();
                break;
            case "runIce":
                Effect.clip = runIce;
                Effect.Play();
                break;
            case "runOil":
                Effect.clip = runIce;
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
            case "click":
                Effect.clip = click;
                Effect.Play();
                break;
            case "dizzy":
                Effect.clip = dizzy;
                Effect.Play();
                break;
            case "wind":
                Effect.clip = wind;
                Effect.Play();
                break;
            case "mole":
                Effect.clip = mole;
                Effect.Play();
                break;
            case "countDown":
                Effect.clip = countDown;
                Effect.Play();
                break;
        }
    }

}
