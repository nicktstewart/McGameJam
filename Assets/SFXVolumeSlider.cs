using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeSlider : MonoBehaviour
{
    public Slider sfxSlider;

    void Start()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.1f);
        sfxSlider.onValueChanged.AddListener(delegate { updateVolume(); });
    }

    void updateVolume()
    {
        float sfx_volume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", sfx_volume);
        SFXVolume.sfxvolume = sfx_volume;
    }
}
