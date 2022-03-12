using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggingWheelAnimation : MonoBehaviour
{
    public Animator anim;

    void FixedUpdate()
    {
        if (PlayerMoveDigging.isMoving)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
}
