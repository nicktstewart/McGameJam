using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawJoint : MonoBehaviour
{
    private HingeJoint2D hinge;
    private HeadPhase phase;

    private MasterPhase masterPhase;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        phase = HeadPhase.NoPhase;
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

    void GenericJawAnim(int amp)
    {
        JointMotor2D motor = hinge.motor;
        if (phase == HeadPhase.BiteUpPhase) {
            motor.motorSpeed -= amp;
        }
        else if (phase == HeadPhase.BiteDownPhase) {
            motor.motorSpeed += amp;
        }
        hinge.motor = motor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {     
        switch (masterPhase) {
            case MasterPhase.RoarTaunt:
                GenericJawAnim(100);
                break;
            case MasterPhase.BiteRage:
                GenericJawAnim(100);
                break;
        }
    }
}
