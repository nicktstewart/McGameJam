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
    [SerializeField]
    public GameObject player;

    [SerializeField]
    public Camera MainCamera;

    [SerializeField]
    public AudioSource Music;
    [SerializeField]
    public AudioSource RoarAudio;

    private bool active = false;
    private const float playerDistanceMin = 20f;
    private const float cameraSizeMax = 15f;
    private const float cameraYMax = 9f;

    private float startTime = 0;
    private float duration = -1;

    void Start()
    {
        Music.Stop();
        RoarAudio.Stop();
    }

    void BiteDown()
    {
        BroadcastMessage("BiteDownChild");
    }

    void BiteUp()
    {
        BroadcastMessage("BiteUpChild");
    }

    void EndPhase()
    {
        BroadcastMessage("SetMasterPhase", MasterPhase.Idle);
    }

    void FixedUpdate()
    {
        if (!active && Vector3.Distance(transform.position, player.transform.position) <= playerDistanceMin) {
            active = true;
            startTime = Time.time;
            duration = 3f;
            BroadcastMessage("SetMasterPhase", MasterPhase.RoarTaunt);
            RoarAudio.Play();
            Music.loop = true;
            Music.Play();
        }
        if (active && MainCamera.orthographicSize < cameraSizeMax) MainCamera.orthographicSize *= 1.01f;
                
        if (duration != -1 && Time.time - startTime > duration) {
            duration = -1;
            EndPhase();
        }
    }
}
