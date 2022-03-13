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
    public GameObject fadeBlack;
    private Image fadeImage;
    private string sceneName;


    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Menu")
            fadeBlack.SetActive(false);
        isOpen = false;
        BGMController.isPaused = false;
        float bgm_volume = PlayerPrefs.GetFloat("BGMVolume", 0.1f);
        BGMController.bgmvolume = bgm_volume;
        if (sceneName == "Menu")
            fadeImage = fadeBlack.GetComponent<Image>();
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
        print("Click");
        if(!isOpen && SceneManager.GetActiveScene().name == "Menu")
        {
            StartCoroutine(FadeToBlack());
        }
    }

    IEnumerator FadeToBlack()
    {
        fadeBlack.SetActive(true);
        OnDisable();
        
        for (int i = 0; i < 30; i++)
        {
            fadeImage.color = new Color(1, 1, 1, i / 30.0f);
            yield return new WaitForSeconds(0.02f);
        }
        SceneManager.LoadScene("Lore1", LoadSceneMode.Single);
    }

    void OnDisable()
    {
        inputMap.Disable();
        inputMap.PlayerControls.Shoot.performed -= eventCtx => Clicked();
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
