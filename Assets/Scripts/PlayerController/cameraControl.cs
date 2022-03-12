using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed = 1.5f;

    private Vector3 defaultDiff;
    // Start is called before the first frame update
    void Start()
    {
        defaultDiff = transform.position - player.transform.position; //orginal distance btw player and camera
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Camera with transition
        transform.position += (defaultDiff + player.transform.position - transform.position)*Time.deltaTime*cameraSpeed;

        //Camera without transition
        // transform.position = defaultDiff + player.transform.position;
    }
}
