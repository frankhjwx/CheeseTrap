using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject audioPrefab;
    private Dictionary<int, GameObject> loopAudioObjects = new Dictionary<int, GameObject>();
    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // 根据音频存储路径获取AudioClip
    public AudioClip LoadAudioClip(string path){
        return Resources.Load(path) as AudioClip;
    }
    // 单次播放的音频
    public void PlayOnceAudio(AudioClip clip){
        GameObject newAudio = Instantiate(audioPrefab);
        DontDestroyOnLoad(newAudio);
        newAudio.GetComponent<AudioSource>().clip = clip;
        newAudio.GetComponent<AudioSource>().Play();
        DestroyObjectOnEnd(newAudio);
    }

    // 注册一个需要播放的循环音频（比如跑步声，吃东西声etc）
    // 返回值为该obj的key，调用stop把它干掉
    public int PlayLoopAudio(AudioClip clip){
        currentIndex += 1;
        loopAudioObjects[currentIndex] = Instantiate(audioPrefab);
        DontDestroyOnLoad(loopAudioObjects[currentIndex]);
        loopAudioObjects[currentIndex].GetComponent<AudioSource>().clip = clip;
        loopAudioObjects[currentIndex].GetComponent<AudioSource>().loop = true;
        loopAudioObjects[currentIndex].GetComponent<AudioSource>().Play();
        return currentIndex;
    }

    // 停止一个循环播放的音频
    public void StopLoopAudio(int audioIndex){
        if (loopAudioObjects[audioIndex] != null)
            Destroy(loopAudioObjects[audioIndex]);
    }

    // 在音频播完之后就把这个实例对象干掉
    private IEnumerator DestroyObjectOnEnd(GameObject gameObject){
        while (gameObject.GetComponent<AudioSource>().isPlaying){
            yield return null;
        }
        Destroy(gameObject);
    }

}
