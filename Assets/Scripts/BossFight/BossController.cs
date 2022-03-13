using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum MasterPhase {
    NULL,
    Idle,
    BiteRage,
    RoarTaunt
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
    public AudioSource MusicPaused;
    [SerializeField]
    public AudioSource RoarAudio;

    private bool active = false;
    private const float playerDistanceMin = 20f;
    private const float cameraSizeMax = 15f;
    private const float cameraYMax = 9f;

    private float startTime = 0;
    private float duration = -1;

    private MasterPhase currentPhase;

    private MasterPhase[] attackPattern = new MasterPhase[]{
        MasterPhase.BiteRage, MasterPhase.RoarTaunt
    };
    private float[] attackPatternDurations = new float[]{ 5f, 3f };

    private int attackPatternIndex = 0;

    void Start()
    {
        Music.Stop();
        MusicPaused.Stop();
        RoarAudio.Stop();
        currentPhase = MasterPhase.NULL;
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
        startTime = Time.time;
        if (currentPhase != MasterPhase.Idle) {
            currentPhase = MasterPhase.Idle;
            BroadcastMessage("SetMasterPhase", MasterPhase.Idle);
            duration = 4f;
        }
        else {
            currentPhase = attackPattern[attackPatternIndex];
            BroadcastMessage("SetMasterPhase", currentPhase);
            duration = attackPatternDurations[attackPatternIndex];
            if (currentPhase == MasterPhase.RoarTaunt) RoarAudio.Play();
            attackPatternIndex = (attackPatternIndex + 1) % attackPattern.Length;
        }
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
            MusicPaused.loop = true;
            Music.Play();
            MusicPaused.Play();
        }
        if (active && MainCamera.orthographicSize < cameraSizeMax) MainCamera.orthographicSize *= 1.01f;
                
        if (duration != -1 && Time.time - startTime > duration) {
            EndPhase();
        }
    }
}
