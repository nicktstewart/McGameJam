using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public int hp = 3;
    private GameObject player;
    public GameObject fireBall;
    private bool readyToShoot;
    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerInSnakePos = player.transform.position - transform.position;
        //making the snake face the right direction
        if(Mathf.Abs(Vector3.Dot(Vector3.right, playerInSnakePos)) > Mathf.Abs(Vector3.Dot(Vector3.up, playerInSnakePos))){
            if(Vector3.Dot(Vector3.right, playerInSnakePos) > 0){
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
        else{
            if(Vector3.Dot(Vector3.up, playerInSnakePos) > 0){
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
            }
            else{
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
            }
        }
    }
    
    void FixedUpdate(){
        //Shoot fire balls
        if(readyToShoot){
            readyToShoot = false; // blocks snake from shooting
            StartCoroutine(Shoot());
        }

        if(hp == 0){
            TerrainGeneration.gridMap[TerrainGeneration.ConvertToGridCoord(transform.position)] = new int[2]{-1,0};
            Destroy(gameObject);
        }
    }

    private IEnumerator Shoot(){
        yield return new WaitForSeconds(3f);
        bool canShoot = Physics2D.OverlapPointAll((Vector2)(transform.position - transform.right*3.8f)).Length == 0;
        if(canShoot) Instantiate(fireBall, transform.position + transform.right*(-3.8f), transform.rotation);
        readyToShoot = true;
    }
}
