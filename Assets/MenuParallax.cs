using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float depth;
    private Player inputMap;
    public RectTransform rt;

    private void OnEnable()
    {
        if (inputMap == null)
        {
            inputMap = new Player();
        }
        inputMap.Enable();

    }

    void Update()
    {
        Vector2 mousePos = inputMap.PlayerControls.Look.ReadValue<Vector2>();
        Vector2 middle = new Vector2(Screen.width/2, Screen.height/2);
        Vector2 distance = new Vector2((mousePos.x - middle.x) / depth, (mousePos.y - middle.y) / depth);
        print(distance);
        rt.position = new Vector2(middle.x + distance.x, middle.y + distance.y);
        
    }
}
