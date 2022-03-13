using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicPersistent : MonoBehaviour
{

    void Awake()
    {
        string name = SceneManager.GetActiveScene().name;

        if (name == "Lore1" || name == "Menu")
        {
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        string name = SceneManager.GetActiveScene().name;
        if (name == "Lore1" || name == "Menu")
        {
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
