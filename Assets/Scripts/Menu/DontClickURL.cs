using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontClickURL : MonoBehaviour
{
    public AudioSource a;
    public void OnButtonPress()
    {
        print("RICKROLL");
        a.Play();
    }
}
