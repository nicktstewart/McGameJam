using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullJoint : MonoBehaviour
{
    HingeJoint2D hinge;

    // Start is called before the first frame update
    void Start()
    {
        hinge = GetComponent<HingeJoint2D>();
        hinge.useMotor = true;
    }

    void BiteDownChild()
    {
        // JointMotor2D motor = hinge.motor;
        // motor.motorSpeed = 1000;
        // hinge.motor = motor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
