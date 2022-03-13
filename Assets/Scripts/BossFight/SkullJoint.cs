using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum HeadPhase {
    NoPhase,
    BiteUpPhase,
    BiteDownPhase
}

public class SkullJoint : MonoBehaviour
{
    private HingeJoint2D hinge;

    private HeadPhase phase;
    private MasterPhase masterPhase;

    [SerializeField]
    public AudioSource ClackAudio;

    [SerializeField]
    public Rigidbody2D projectile;

    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        phase = HeadPhase.NoPhase;
        masterPhase = MasterPhase.Idle;
        ClackAudio.Stop();
    }

    void ResetHinge()
    {
        JointMotor2D motor = new JointMotor2D();
        motor.maxMotorTorque = 10000;
        hinge.motor = motor;
    }

    void SetMasterPhase(MasterPhase newMasterPhase)
    {
        masterPhase = newMasterPhase;
        switch (masterPhase) {
            case MasterPhase.Idle:
                hinge.useMotor = false;
                break;
            case MasterPhase.RoarTaunt: case MasterPhase.BiteRage:
                ResetHinge();
                BiteDownChild();
                break;
        }
    }

    void BiteDownChild()
    {
        hinge.useMotor = true;
        phase = HeadPhase.BiteDownPhase;
    }

    void BiteUpChild()
    {
        hinge.useMotor = true;
        phase = HeadPhase.BiteUpPhase;
    }

    void ThrowBomb()
    {        
        Rigidbody2D clone = (Rigidbody2D) Instantiate(projectile, spawnPoint.position, projectile.transform.rotation);
        clone.transform.Rotate(0f, 0f, UnityEngine.Random.Range(0f, 360f));
        clone.velocity = spawnPoint.TransformDirection(2 * Vector3.left);
    }

    void GenericSkullAnim(int amp)
    {
        JointMotor2D motor = hinge.motor;

        if (phase == HeadPhase.BiteUpPhase) {
            motor.motorSpeed -= amp;
            if (motor.motorSpeed <= -1000) {
                phase = HeadPhase.BiteDownPhase;
                SendMessageUpwards("BiteDown");
            }
        }
        else if (phase == HeadPhase.BiteDownPhase) {
            motor.motorSpeed += amp;
            if (motor.motorSpeed >= 1000) {
                phase = HeadPhase.BiteUpPhase;
                SendMessageUpwards("BiteUp");
                if (masterPhase == MasterPhase.BiteRage) {
                    ClackAudio.Play();
                    ThrowBomb();
                }
            }
        }
        hinge.motor = motor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (masterPhase) {
            case MasterPhase.RoarTaunt:
                GenericSkullAnim(900);
                break;
            case MasterPhase.BiteRage:
                GenericSkullAnim(100);
                break;
        }
    }
}
