using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggingDrillAnimation : MonoBehaviour
{
    public Animator anim;

    void FixedUpdate()
    {
        if (PlayerMoveDigging.isDrilling)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
}
