using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct BackgroundMusic
{
    public string sceneName;
    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    public GameObject audioPrefab;
    private Dictionary<int, GameObject> loopAudioObjects = new Dictionary<int, GameObject>();
    public BackgroundMusic[] BGMList;
    private string currentScene;
    private GameObject BGMObject = null;
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameObject.FindWithTag("AudioManager") != null){
            Destroy(this.gameObject);
            return;
        }
        this.gameObject.tag = "AudioManager";
        currentScene = SceneManager.GetActiveScene().name;
        UpdateBackgroundMusic();
        DontDestroyOnLoad(this.gameObject);
    }

    // 根据音频存储路径获取AudioClip
    public AudioClip LoadAudioClip(string path){
        return Resources.Load(path) as AudioClip;
    }
    // 单次播放的音频
    public void PlayOnceAudio(AudioClip clip){
        GameObject newAudio = Instantiate(audioPrefab);
        newAudio.name = "Audio_"+clip.name;
        DontDestroyOnLoad(newAudio);
        newAudio.GetComponent<AudioSource>().clip = clip;
        newAudio.GetComponent<AudioSource>().Play();
        DestroyObjectOnEnd(newAudio);
    }

    private AudioClip FindBGMIndex(string sceneName){
        for (int i=0; i<BGMList.Length; i++){
            if (BGMList[i].sceneName == sceneName) return BGMList[i].audioClip;
        }
        return null;
    }
    private void UpdateBackgroundMusic(){
        if (BGMObject == null) {
            BGMObject = Instantiate(audioPrefab);
            BGMObject.name = "BackgroundMusic";
            BGMObject.GetComponent<AudioSource>().loop = true;
            DontDestroyOnLoad(BGMObject);
        }
        AudioClip newClip = FindBGMIndex(currentScene);
        if (newClip != null){
            if (newClip != BGMObject.GetComponent<AudioSource>().clip){
                BGMObject.GetComponent<AudioSource>().clip = newClip;
                BGMObject.GetComponent<AudioSource>().Play();
            }
        }
        else {
            BGMObject.GetComponent<AudioSource>().Pause();
            Debug.LogError("当前场景的BGM未配置！");
        }
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

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (SceneManager.GetActiveScene().name != currentScene) {
            currentScene = SceneManager.GetActiveScene().name;
            UpdateBackgroundMusic();
        }
    }
}
