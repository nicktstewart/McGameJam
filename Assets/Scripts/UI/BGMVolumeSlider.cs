using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMVolumeSlider : MonoBehaviour
{
    public Slider bgmSlider;

    void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.1f);
        bgmSlider.onValueChanged.AddListener(delegate { updateVolume(); });
    }

    void updateVolume()
    {
        float bgm_volume = bgmSlider.value;
        PlayerPrefs.SetFloat("BGMVolume", bgm_volume);
        BGMController.bgmvolume = bgm_volume;
    }
}
