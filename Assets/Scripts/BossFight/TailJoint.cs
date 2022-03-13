using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailJoint : MonoBehaviour
{
    private HingeJoint2D hinge;
    public GameObject drill;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        print("Collided");
        print(collision.gameObject);
        if (collision.gameObject == drill) {
            print("Collided drill");
            Whip();
        }
    }


    void FixedUpdate()
    {
        
    }
}
