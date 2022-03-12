using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class pointerController : MonoBehaviour
{
    public GameObject player;
    public Camera playerCamera;
    public GameObject hook;
    public SpriteRenderer hookSpriteRenderer;
    public LayerMask platformLayerMask;         //Filter to check for platforms
    public PlayerMouvement pm;
    public float chargeDelay = 1f;

    //program variables
    private Player inputMap;
    private Vector2 hitPointPos;
    private DistanceJoint2D joint;
    private Rigidbody2D rb;
    private float hitDistance;
    private Vector2 lauchVector;
    private Vector3 normalRVector;
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
        Vector3 worldPosition = playerCamera.ScreenToWorldPoint(mousePos);
        Vector3 playerPos = player.transform.position;
        
        float angle = Vector2.SignedAngle(Vector2.right, worldPosition-playerPos);
        transform.eulerAngles = new Vector3(0,0,angle);

        normalRVector = Vector3.Normalize(Vector3.Normalize(worldPosition - playerPos))*5;
        Vector3 newPos = playerPos + normalRVector;
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        if(isHooking){
            //Setting the position of the hook
            Vector2 midPoint = (hitPointPos + (Vector2)playerPos)/2;
            hook.transform.position = midPoint;
            //Setting the length of the hook
            hitDistance = Vector2.Distance(hitPointPos, (Vector2)playerPos);
            hook.transform.localScale = new Vector3(hitDistance, 1f, 0);

            //Good rotation of the hook
            //find the angle of the triangle made by center, mouse, (mouseX,centerY)
            float theta = Vector2.SignedAngle(Vector2.right, hitPointPos - (Vector2)playerPos);
            
            Vector2 lauchVector = (hitPointPos - (Vector2)(playerPos)).normalized;

            hook.transform.eulerAngles = new Vector3(0,0,theta);

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
        RaycastHit2D raycastHit2D = Physics2D.Raycast(player.transform.position, normalRVector, 10f, platformLayerMask);
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
            pm.Launch(lauchVector,(chargeRate/chargeDelay)*50);
        }
    }

}
