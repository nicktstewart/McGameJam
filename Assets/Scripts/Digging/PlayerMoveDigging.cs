using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveDigging : MonoBehaviour
{
    public float speed;
    public static bool isDrilling;
    public static bool isMoving;
    private Player inputMap;
    private int coolDown = 0;
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
        isMoving = false;
        xReady = true;
        yReady = true;
    }


    void FixedUpdate()
    {
        if (coolDown > 0)
            coolDown--;

        // print(inputMap.PlayerControls.Mouvement.ReadValue<Vector2>());
        float moveX = inputMap.PlayerControls.Mouvement.ReadValue<Vector2>().x;
        float moveY = inputMap.PlayerControls.Mouvement.ReadValue<Vector2>().y;

        if (moveX > 0 && xReady)
            moveX = 1;
        if (moveX < 0 && xReady)
            moveX = -1;
        if (moveY > 0 && yReady)
            moveY = 1;
        if (moveY < 0 && yReady)
            moveY = -1;
        if (moveX = 0)

        if((moveX != 0 || moveY != 0) && coolDown == 0)
        {
            StartCoroutine(MoveBlock(transform.position.x + 3.8f * moveX, transform.position.y + 3.8f * moveY, 20));
            coolDown = 20;
        }

        if (moveX != 0 || moveY != 0)
            isMoving = true;
        else
            isMoving = false;

        if (moveX > 0 || moveY != 0)
            transform.localScale = new Vector3(1, 1, 1);
        if (moveX < 0 && moveY == 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (moveY > 0)
            transform.eulerAngles = new Vector3(0, 0, 90);
        else if (moveY < 0)
            transform.eulerAngles = new Vector3(0, 0, -90);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
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
        y = Mathf.Round(y * 100f) / 100f;
        x = Mathf.Round(x * 100f) / 100f;

        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, y, 0), 0.1f);
        GameObject block = null;
        foreach (var hitCollider in hitColliders)
        {
            block = hitCollider.gameObject;
        }
        if (block == null || block.tag == "Air")
        {
            transform.position = new Vector3(x, y, 0);
        }
        if(block.tag == "Rock")
        {
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector3(x, y, 0);
        }
        else
        {
            coolDown = 0;
        }
        yield return new WaitForSeconds(0.1f);
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
