using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    AudioClipsManager AudioClipsManagerScript;
    GameObject AudioPrefab;
    public AudioClip eat;
    public AudioClip fat;
    public AudioClip runcheese;
    public AudioClip runice;
    public AudioClip runoil;
    public AudioClip choco;
    public AudioClip die;
    public AudioClip start;
    public AudioClip end;
    public AudioClip touch;
    public AudioClip click;
    // Start is called before the first frame update
    void Start()
    {
        AudioPrefab = (GameObject)Resources.Load("Prefabs/SmallAudio");
    }

    /// <summary>
    /// 音乐播放以及播放器销毁
    /// </summary>
    /// <param name="_type"></param>

    public void PlayAudioClips(string _type)
    {
        switch (_type)
        {
            case "eat":
                Effect.PlayOneShot(eat);
                Debug.Log("eat");
                break;
            case "fat":
                Effect.PlayOneShot(fat);
                break;
            case "runcheese":
                Effect.PlayOneShot(runcheese);
                break;
            case "runice":
                Effect.PlayOneShot(runice);
                break;
            case "runoil":
                Effect.PlayOneShot(runoil);
                break;
            case "choco":
                Effect.PlayOneShot(choco);
                break;
            case "die":
                Effect.PlayOneShot(die);
                break;
            case "start":
                Effect.PlayOneShot(start);
                break;
            case "end":
                Effect.PlayOneShot(end);
                break;
            case "touch":
                Effect.PlayOneShot(touch);
                break;
            case "click":
                Effect.PlayOneShot(click);
                break;
        }
    }

}
