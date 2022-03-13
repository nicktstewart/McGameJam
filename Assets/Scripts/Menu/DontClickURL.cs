using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontClickURL : MonoBehaviour
{
    public AudioSource a;
    public void OnButtonPress()
    {
        float v = PlayerPrefs.GetFloat("BGMVolume", 0.1f);
        PlayerPrefs.SetFloat("BGMVolume", 0);
        BGMController.bgmvolume = 0;
        print("RICKROLL");
        a.volume = v;
        a.Play();
    }
}
