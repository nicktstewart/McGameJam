using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour
{
    public GameObject fadeBlack;
    private bool hasClicked = false;
    public void OnButtonClick()
    {
        if(!hasClicked)
            StartCoroutine(FadeToBlack());
        hasClicked = true;
    }

    IEnumerator FadeToBlack()
    {
        fadeBlack.SetActive(true);
        for (int i = 0; i < 30; i++)
        {
            fadeBlack.GetComponent<Image>().color = new Color(1, 1, 1, i / 30.0f);
            yield return new WaitForSeconds(0.02f);
        }
        SceneManager.LoadScene("Menu");
    }
}
