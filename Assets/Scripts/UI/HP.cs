using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HP : MonoBehaviour
{

    public static int playerHp = 100;
    private float shownHp = 100;
    public RectTransform rectTransform;
    public int initialX;
    public int initialY;

    public static GameObject fadeBlack;


    void FixedUpdate()
    {
        if(playerHp < 0)
        {
            Death();
        }
        if(playerHp < shownHp)
        {
            shownHp -= 0.3f;
            rectTransform.sizeDelta = new Vector2(initialX * shownHp / 100, initialY);
        }
        if (playerHp > shownHp)
        {
            shownHp += 0.3f;
            rectTransform.sizeDelta = new Vector2(initialX * shownHp / 100, initialY);
        }
    }

    void Death()
    {
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}
