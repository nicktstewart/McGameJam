using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    //initializing instance variables
    private bool lastTouchedIsIce = false;      //if last surface touched is ice
    private int isFacingRight = 1;          //if the sprite is facing the right
    private float crouching;
    private Rigidbody2D rb;                     //Rigidbody component of the player
    private BoxCollider2D boxCollider;  //CapsuleCollider component of the player
    private Player inputMap;

    public LayerMask platformLayerMask;         //Filter to check for platforms
    public float jumpForce = 1f;                //jump force
    public float walkSpeed = 1f;                //walk speed on normal ground
    public float iceWalkSpeed = 0.5f;           //walk speed on ice
    public float crouchMultiplier = 2f;
    public float multiplier = 0.5f;
    // public Animator animator;
    public GameObject hook;

    [HideInInspector] public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        //get player components
        rb = transform.GetComponent<Rigidbody2D>();
        boxCollider = transform.GetComponent<BoxCollider2D>();
        crouching = inputMap.PlayerControls.Crouch.ReadValue<float>();
    }

    private void OnEnable(){
        if (inputMap == null) {
            inputMap = new Player();
        }
        inputMap.Enable();

    }

    void FixedUpdate()
    {   
        
        //=-=-=-=-=-=- Movement -=-=-=-=-=-=//
        //If the player is on normal ground, //
        //then movement is created by setting//
        //the mouvement vector to walkSpeed. //
        //      If the player os on ice      //
        // then mouvement is created by using//
        //     AddForce on the rigidbody.    //
        //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=//

        float jumping = inputMap.PlayerControls.Jump.ReadValue<float>();
        float moveDelta = -inputMap.PlayerControls.Mouvement.ReadValue<Vector2>().y;
        Debug.Log(moveDelta);
        isGrounded = IsGrounded();

        if(isGrounded){
            if (jumping == 1){
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                // animator.SetBool("isJumping", true);
            }
            else{
                // animator.SetBool("isJumping", false);
            }
        }
    
        if(lastTouchedIsIce || hook.activeSelf){
            rb.AddForce(new Vector2(moveDelta*iceWalkSpeed*(1-crouching/crouchMultiplier),0));
        }
        else{
            rb.velocity = new Vector2(moveDelta*walkSpeed*(1-crouching/crouchMultiplier), rb.velocity.y);
        }
        //check if changing direction is needed
        if (isFacingRight == -moveDelta) {
            transform.Rotate(0,180,0);
            isFacingRight = -isFacingRight;
        }

        if(moveDelta == 0){
            if(!lastTouchedIsIce && !hook.activeSelf){
                rb.velocity = new Vector2(0, rb.velocity.y);                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        crouching = inputMap.PlayerControls.Crouch.ReadValue<float>();

        // animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        // animator.SetFloat("Vy", rb.velocity.y);
        // animator.SetFloat("Crouching", crouching);
    }
    
    /**
     * Method that checks if the player is on the ground and updates the lastTouchedIce variable.
     * It is done by casting a capsule with the same size as the players hitbox and lowering it
     * by -.1 on the y axis, then checking if any colliders are on it. 
     * 
     * returns a bool (true if on ground else false) 
     */
    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.CapsuleCast(boxCollider.bounds.center, boxCollider.size, 0f, .1f, Vector2.down,.15f,platformLayerMask);
        
        if (raycastHit2D.collider != null){
            // lastTouchedIsIce = raycastHit2D.transform.CompareTag("ice");
            return true;
        }
        return false;
    }

    public void Launch(Vector2 LaunchVec, float launchForce){
        lastTouchedIsIce = true;
        rb.velocity += LaunchVec*launchForce;
    }
}
