using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class donkillme : MonoBehaviour
{
    AudioSource audiosource1;
    // Start is called before the first frame update
    void Start()
    {
        audiosource1 = this.GetComponent<AudioSource>();
        DontDestroyOnLoad(audiosource1);
    }

}
