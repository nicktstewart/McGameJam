using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private Player inputMap;
    private bool isOpen;
    public Image fadeImage;

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
        inputMap.PlayerControls.Shoot.performed += eventCtx => Clicked();
    }

    void Clicked()
    {
        if(!isOpen && SceneManager.GetActiveScene().name == "Menu")
        {
            StartCoroutine(FadeToBlack());
        }
    }

    IEnumerator FadeToBlack()
    {
        for (int i = 0; i < 30; i++)
        {
            fadeImage.color = new Color(1, 1, 1, i / 30.0f);
            yield return new WaitForSeconds(0.02f);
        }
        SceneManager.LoadScene("Lore1");
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
