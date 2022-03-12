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
    bool flag2 = true;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
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
            source.Play();
            flag = false;
        }
        if (!source.isPlaying && flag2) {
            BroadcastMessage("SetMasterPhase", MasterPhase.Idle);
            flag2 = false;
        }
    }
}
