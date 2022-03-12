using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource audioNormal;
    public AudioSource audioPaused;
    public static float bgmvolume;
    public static bool isPaused;

    void Update()
    {

        if (isPaused)
        {
            audioNormal.volume = 0;
            audioPaused.volume = bgmvolume;
        }
        else
        {
            audioNormal.volume = bgmvolume;
            audioPaused.volume = 0;
        }
    }
}
