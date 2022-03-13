using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveDigging : MonoBehaviour
{
    public GameObject emptyBoi;
    public float speed;
    public static bool isDrilling;
    public static bool isMoving;
    public static bool noItemInBlock;
    private bool isBreaking;
    private Player inputMap;
    private int coolDown = 0;
    private GameObject particles;
    bool xReady;
    bool yReady;

    private void OnEnable()
    {
        if (inputMap == null)
        {
            inputMap = new Player();
        }
        inputMap.Enable();

        inputMap.PlayerControls.Shoot.performed += eventCtx => OnShoot();
        inputMap.PlayerControls.Shoot.canceled += eventCtx => StopShoot();
    }

    void Start()
    {
        isDrilling = false;
        isBreaking = false;
        isMoving = false;
        xReady = true;
        yReady = true;
        noItemInBlock = true;
        DashboardController.hp = 100;
        particles = transform.GetChild(4).gameObject;
        particles.SetActive(false);
    }


    void FixedUpdate()
    {
        // cooldown
        if (coolDown > 0)
            coolDown--;

        // print(inputMap.PlayerControls.Mouvement.ReadValue<Vector2>());
        float moveX = inputMap.PlayerControls.Mouvement.ReadValue<Vector2>().x;
        float moveY = inputMap.PlayerControls.Mouvement.ReadValue<Vector2>().y;

        if (coolDown == 0 && !isBreaking){
            // movemnet x
            if (moveX > 0 && xReady)
            {
                StartCoroutine(MoveBlock(transform.position.x + 3.8f, transform.position.y, 20));
                transform.eulerAngles = new Vector3(0, 0, 0);
                coolDown = 10;
                xReady = false;
            }
            else if (moveX < 0 && xReady)
            {
                StartCoroutine(MoveBlock(transform.position.x - 3.8f, transform.position.y, 20));
                transform.eulerAngles = new Vector3(0, 180, 0);
                coolDown = 10;
                xReady = false;
            }

            // movement y
            if (moveY > 0 && yReady)
            {
                StartCoroutine(MoveBlock(transform.position.x, transform.position.y + 3.8f, 20));
                transform.eulerAngles = new Vector3(0, 0, 90);
                coolDown = 10;
                yReady = false;
            }
            else if (moveY < 0 && yReady)
            {
                StartCoroutine(MoveBlock(transform.position.x, transform.position.y - 3.8f, 20));
                transform.eulerAngles = new Vector3(0, 180, -90);
                coolDown = 10;
                yReady = false;
            }
        }
        

        // reset keys
        if (moveX == 0)
        {
            xReady = true;
        }
        if (moveY == 0)
        {
            yReady = true;
        }
    }

    void OnShoot()
    {
        isDrilling = true;
    }

    void StopShoot()
    {
        isDrilling = false;
    }

    void Damaged (int damage){
        DashboardController.hp -= damage;
    }

    IEnumerator DigBlock(GameObject block)
    {
        yield return new WaitForSeconds(1);
    }

    IEnumerator MoveBlock(float x, float y, int duration)
    {
        isMoving = true;
        y = Mathf.Round(y * 100f) / 100f;
        x = Mathf.Round(x * 100f) / 100f;

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(new Vector2(x, y));
        
        if(hitColliders.Length == 0){
            transform.position = new Vector3(x, y, 0);
            yield return new WaitForSeconds(0.1f);
        }
        else if(hitColliders[0].CompareTag("Snake")){
            DashboardController.hp -= 10;
            yield return new WaitForSeconds(0.1f);

        }
        else if(hitColliders[0].CompareTag("Heart")){
            if(DashboardController.hp < (100 - 10)){
                DashboardController.hp += 10;
            }
            else if(DashboardController.hp!=100){
                DashboardController.hp = 100;
            }
            TerrainGeneration.gridMap[TerrainGeneration.ConvertToGridCoord(new Vector3(x,y,0))] = new int[2] {-1,0};
            Destroy(hitColliders[0].gameObject);
            transform.position = new Vector3(x, y, 0);
            yield return new WaitForSeconds(0.1f);
        }
        else if (isDrilling && hitColliders[0].gameObject != this.gameObject)
        {
            GameObject block = hitColliders[0].gameObject;
            isBreaking = true;
            noItemInBlock = true;
            particles.SetActive(true);
            yield return new WaitForSeconds(block.GetComponent<BlockBreaking>().breakBlock());
            if (isDrilling && noItemInBlock) transform.position = new Vector3(x, y, 0);
            particles.SetActive(false);
            isBreaking = false;
        }
        
        isMoving = false;
    }


    /*
     * 
     * Skull : fossileType 0
     * Arm   : fossileType 1
     * Leg   : fossileType 2
     * 
     */
    void FoundFossile(int fossileType)
    {
        if(fossileType == 0)
        {
            DashboardController.hasSkull++;
        }
        if (fossileType == 1)
        {
            DashboardController.hasArm++;
        }
        if (fossileType == 2)
        {
            DashboardController.hasLeg++;
        }
    }
}
