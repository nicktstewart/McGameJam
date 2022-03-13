using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailJoint : MonoBehaviour
{
    private HingeJoint2D hinge;
    public GameObject drill;

    private bool whipFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
    }

    void Halt()
    {
        hinge.useMotor = false;
    }

    void Whip()
    {
        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = -100;
        hinge.motor = motor;
        hinge.useMotor = true;
    }

    void DownWhip()
    {
        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = 100;
        hinge.motor = motor;
        hinge.useMotor = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!whipFlag && collision.gameObject == drill) {
            whipFlag = true;
            DownWhip();
        }
    }

    void FixedUpdate()
    {
        print(hinge.limitState);
        if (whipFlag && hinge.limitState == JointLimitState2D.LowerLimit) {
            DownWhip();
        }
        else if (whipFlag && hinge.limitState == JointLimitState2D.UpperLimit) {
            Whip();
        }
    }
}
