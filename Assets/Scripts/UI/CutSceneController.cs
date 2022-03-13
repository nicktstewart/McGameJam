using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    private int currentSlide;
    public GameObject[] Slides;
    public string NextSceneName;
    private Player inputMap;

    private bool canClick;

    void Start()
    {
        currentSlide = 0;
        Slides[currentSlide].SetActive(true);
        StartCoroutine(FadeFromBlack());
        canClick = true;
    }

    private void OnEnable()
    {
        if (inputMap == null)
        {
            inputMap = new Player();
        }
        inputMap.Enable();

        inputMap.PlayerControls.Shoot.performed += eventCtx => NextSlide();
    }

    void NextSlide()
    {
        if(currentSlide + 1 < Slides.Length)
        {
            Slides[currentSlide].SetActive(false);
            currentSlide++;
            Slides[currentSlide].SetActive(true);
        }
        else
        {
            if(canClick)
                StartCoroutine(FadeToBlack());
            canClick = false;
        }
    }

    IEnumerator FadeFromBlack()
    {
        for(int i = 0; i < 30; i++)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, (30 - i) / 30.0f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator FadeToBlack()
    {
        for (int i = 0; i < 30; i++)
        {
            GetComponent<Image>().color = new Color(1, 1, 1, i / 30.0f);
            yield return new WaitForSeconds(0.02f);
        }
        SceneManager.LoadScene(NextSceneName);
    }
}
