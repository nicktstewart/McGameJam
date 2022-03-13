using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXVolume : MonoBehaviour
{
    public AudioSource audioNormal;
    public static float sfxvolume;

    void Start()
    {
        sfxvolume = PlayerPrefs.GetFloat("SFXVolume", 0.1f);
    }

    void Update()
    {
        audioNormal.volume = sfxvolume;
    }
}
