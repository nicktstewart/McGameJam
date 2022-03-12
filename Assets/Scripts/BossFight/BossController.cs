using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum MasterPhase {
    Idle,
    BiteRage,
    RoarTaunt,
    TailAttack
}

public class BossController : MonoBehaviour
{

    bool flag = true;

    void Start()
    {
    }

    void BiteDown()
    {
        BroadcastMessage("BiteDownChild");
    }

    void BiteUp()
    {
        BroadcastMessage("BiteUpChild");
    }

    void Update()
    {
        if (flag) {
            
        BroadcastMessage("SetMasterPhase", MasterPhase.RoarTaunt);
        flag = false;
        }
    }
}
