using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public float parallaxFactor;

    [SerializeField]
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {        
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = player.transform.position.x * parallaxFactor;
        Vector3 newPosition = new Vector3(startpos + distance, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }
}
