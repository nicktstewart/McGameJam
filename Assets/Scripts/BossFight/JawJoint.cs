using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawJoint : MonoBehaviour
{
    HingeJoint2D hinge;
    Phase phase;

    MasterPhase masterPhase;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        phase = Phase.NoPhase;
    }

    void SetMasterPhase(MasterPhase newMasterPhase)
    {
        masterPhase = newMasterPhase;
        switch (masterPhase) {
            case MasterPhase.Idle:
                JointMotor2D motor = hinge.motor;
                motor.motorSpeed = 0;
                hinge.motor = motor;
                hinge.useMotor = false;
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

    // Update is called once per frame
    void Update()
    {     
        JointMotor2D motor = hinge.motor;
        if (phase == Phase.BiteUpPhase) {
            motor.motorSpeed -= 100;
        }
        else if (phase == Phase.BiteDownPhase) {
            motor.motorSpeed += 100;
        }
        hinge.motor = motor;
    }
}
