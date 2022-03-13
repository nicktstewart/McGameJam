using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIxedPart : MonoBehaviour
{

    private Vector3 position = new Vector3(0f,0f,0f);
    private Vector3 empty = new Vector3(0f,0f,0f);

    void FixedUpdate()
    {
        if (position != empty)
            transform.position = position;
        position = transform.position;
    }
}
