using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontClickURL : MonoBehaviour
{
    public void OnButtonPress()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }
}
