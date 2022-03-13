using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAnim : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0f, 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(0.2f, 0.2f, 0f);
        if (transform.localScale.x > 3f) 
            Destroy(this.gameObject, 0f);
    }
}
