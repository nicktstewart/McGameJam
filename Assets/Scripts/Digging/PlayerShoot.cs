using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
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

        inputMap.PlayerControls.Shoot.performed += eventCtx => OnShoot();
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
        // bool canShoot = Physics2D.OverlapPointAll((Vector2)(transform.position + transform.right*3.8f)).Length == 0;
        if (readyToShoot) StartCoroutine(Shoot());
    }
    IEnumerator Shoot(){
        readyToShoot = false;
        Instantiate(bullet, transform.position,transform.rotation);
        yield return new WaitForSeconds(0.5f);
        readyToShoot = true;
    }
}
