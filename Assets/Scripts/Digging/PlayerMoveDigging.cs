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
    }

    
    void FixedUpdate()
    {
        float moveX = inputMap.PlayerControls.Mouvement.ReadValue<Vector2>().x;
        print(inputMap.PlayerControls.Mouvement.ReadValue<Vector2>());

        float moveY = inputMap.PlayerControls.Mouvement.ReadValue<Vector2>().y;
        transform.position = new Vector3(transform.position.x + moveX * speed, transform.position.y + moveY * speed, 0);
        if (moveX != 0 || moveY != 0)
            isMoving = true;
        else
            isMoving = false;

        if(moveX > 0 || moveY != 0)
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
}
