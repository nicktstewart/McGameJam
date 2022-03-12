using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum Phase {
    NoPhase,
    BiteUpPhase,
    BiteDownPhase
}

public class SkullJoint : MonoBehaviour
{
    HingeJoint2D hinge;

    Phase phase;
    MasterPhase masterPhase;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        phase = Phase.NoPhase;
        masterPhase = MasterPhase.Idle;
    }

    void SetMasterPhase(MasterPhase newMasterPhase)
    {
        masterPhase = newMasterPhase;
        switch (masterPhase) {
            case MasterPhase.RoarTaunt: case MasterPhase.BiteRage:
                SendMessageUpwards("BiteDown");
                break;
        }
    }

    void BiteDownChild()
    {
        hinge.useMotor = true;
        phase = Phase.BiteDownPhase;
    }

    void BiteUpChild()
    {
        hinge.useMotor = true;
        phase = Phase.BiteUpPhase;
    }

    void GenericSkullAnim(int amp)
    {
        JointMotor2D motor = hinge.motor;
        if (phase == Phase.BiteUpPhase) {
            motor.motorSpeed -= amp;
            if (motor.motorSpeed <= -1000) {
                phase = Phase.BiteDownPhase;
                SendMessageUpwards("BiteDown");
            }
        }
        else if (phase == Phase.BiteDownPhase) {
            motor.motorSpeed += amp;
            if (motor.motorSpeed >= 1000) {
                phase = Phase.BiteUpPhase;
                SendMessageUpwards("BiteUp");
            }
        }
        hinge.motor = motor;
    }

    // Update is called once per frame
    void Update()
    {
        // print(Math.Abs(hinge.jointAngle - hinge.limits.min));
        // if (phase == Phase.BiteDownPhase && Math.Abs(hinge.jointAngle - hinge.limits.min) < 5) {
        //     SendMessageUpwards("BiteUp");
        // }
        // else if (phase == Phase.BiteUpPhase && Math.Abs(hinge.limits.max - hinge.jointAngle) < 5) {
        //     SendMessageUpwards("BiteDown");
        // }
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
