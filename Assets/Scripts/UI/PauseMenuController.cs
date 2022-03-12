using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private Player inputMap;
    private bool isOpen;

    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        isOpen = false;
        BGMController.isPaused = false;
        float bgm_volume = PlayerPrefs.GetFloat("BGMVolume", 0.1f);
        BGMController.bgmvolume = bgm_volume;
    }

    private void OnEnable()
    {
        if (inputMap == null)
        {
            inputMap = new Player();
        }
        inputMap.Enable();

        inputMap.PlayerControls.Escape.performed += eventCtx => OnPause();
    }


    void OnPause()
    {
        if (isOpen)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            Time.timeScale = 1;
            BGMController.isPaused = false;
            isOpen = false;
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            Time.timeScale = 0;
            BGMController.isPaused = true;
            isOpen = this;
        }
    }
}
