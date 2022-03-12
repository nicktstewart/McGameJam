using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class JointController : MonoBehaviour
{
    bool flag = true;

    void Start()
    {
    }

    void BiteDown()
    {
        print("Bite");
        BroadcastMessage("BiteDownChild");
    }

    void Update()
    {
        if (flag) {
            BiteDown();
            flag = false;
        }
    }
}
