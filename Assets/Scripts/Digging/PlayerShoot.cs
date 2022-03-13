using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public FireBall bulletScript;
    public GameObject bullet;
    private Player inputMap;
    private bool readyToShoot;

    private void OnEnable()
    {
        if (inputMap == null)
        {
            inputMap = new Player();
        }
        inputMap.Enable();

        inputMap.PlayerControls.Launch.performed += eventCtx => OnShoot();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    void OnShoot()
    {
        if (readyToShoot) StartCoroutine(Shoot());
    }
    IEnumerator Shoot(){
        readyToShoot = false;
        yield return new WaitForSeconds(0.5f);
        Instantiate(bullet, transform.position + transform.right*2, transform.rotation);
    }
}
