using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class scarfController : MonoBehaviour
{
    public GameObject player;
    public Camera camera;
    public GameObject hook;
    public SpriteRenderer hookSpriteRenderer;
    public LayerMask platformLayerMask;         //Filter to check for platforms
    public PlayerMouvement pm;
    public float chargeDelay = 1f;

    private Player inputMap;
    private Vector2 hitPointPos;
    private DistanceJoint2D joint;
    private Rigidbody2D rb;
    private float hitDistance;
    private float theta;
    private float cos;
    private float sin;
    private float cosHook;
    private float sinHook;
    private float newX;
    private float newY;
    private bool isHooking;
    private float chargeRate;


    // Start is called before the first frame update
    void Start()
    {
        hook.SetActive(false);
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void OnEnable(){
        if (inputMap == null) {
            inputMap = new Player();
        }
        inputMap.Enable();

        inputMap.PlayerControls.Shoot.performed += eventCtx => OnShoot();
        inputMap.PlayerControls.Shoot.canceled += eventCtx => StopShoot();
        inputMap.PlayerControls.Launch.performed += eventCtx => OnLaunch();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = inputMap.PlayerControls.Look.ReadValue<Vector2>();
        Vector3 worldPosition = camera.ScreenToWorldPoint(mousePos);
        float playerx = player.transform.position.x;
        float playery = player.transform.position.y;

        Vector3 newPosition = NewPosition(playerx, playery, worldPosition.x ,worldPosition.y);

        transform.eulerAngles = new Vector3(0,0,(newPosition.z/(2*Mathf.PI))*360);

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        if(isHooking){
            //Setting the position of the hook
            Vector2 midPoint = (hitPointPos + (Vector2)player.transform.position)/2;
            hook.transform.position = midPoint;
            //Setting the length of the hook
            hitDistance = Vector2.Distance(hitPointPos, (Vector2)player.transform.position);
            hook.transform.localScale = new Vector3(hitDistance, 0.1f, 0);

            //Good rotation of the hook
            //find the angle of the triangle made by center, mouse, (mouseX,centerY)
            theta = Mathf.Atan((hitPointPos.y - playery)/(hitPointPos.x - playerx));
            //if the target is the left side of the center
            if ((hitPointPos.x - playerx)<0){
                theta = theta + Mathf.PI;
            }
            cosHook = Mathf.Cos(theta);
            sinHook = Mathf.Sin(theta);
            hook.transform.eulerAngles = new Vector3(0,0,(theta/(2*Mathf.PI))*360);

            chargeRate += Time.deltaTime;
            if (chargeRate < chargeDelay){
                hookSpriteRenderer.color = new Color(1-(chargeRate/chargeDelay),0f,(chargeRate/chargeDelay));
            }
        }
    }

    void FixedUpdate(){


        if (joint != null){
            if(pm.isGrounded && Vector2.Dot(joint.reactionForce, rb.velocity)>0){
                joint.enabled = false;
            }
            if (!joint.enabled && Vector2.Dot((hitPointPos - (Vector2)player.transform.position),rb.velocity)<0){
                joint.distance = Vector2.Distance(hitPointPos,player.transform.position);
                joint.enabled = true;
            }
        }
    }

    private void OnShoot(){
        chargeRate = 0f;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(player.transform.position, new Vector2(cos,sin),2f,platformLayerMask);
        if(raycastHit2D){
            hitPointPos = raycastHit2D.point;
            hitDistance = raycastHit2D.distance;

            joint = player.AddComponent<DistanceJoint2D>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hitPointPos;
            joint.enableCollision = true;

            hook.SetActive(true);
            isHooking = true;
        }
    }

    private void StopShoot(){
        hook.SetActive(false);
        isHooking = false;
        Destroy(joint);
    }

    private void OnLaunch(){
        if(isHooking){
            StopShoot();
            if (chargeRate>chargeDelay) chargeRate = chargeDelay;
            pm.Launch(new Vector2(cosHook,sinHook),(chargeRate/chargeDelay)*5);
        }
    }

    private Vector3 NewPosition(float CenterX,float CenterY,float TargetX,float TargetY){

        //find the angle of the triangle made by center, mouse, (mouseX,centerY)
        theta = Mathf.Atan((TargetY - CenterY)/(TargetX - CenterX));
        //if the target is the left side of the center
        if ((TargetX - CenterX)<0){
            theta = theta + Mathf.PI;
        }

        cos = Mathf.Cos(theta);
        sin = Mathf.Sin(theta);

        newX = player.transform.position.x + cos;
        newY = player.transform.position.y + sin;

        return new Vector3(newX,newY,theta);
    }

}
