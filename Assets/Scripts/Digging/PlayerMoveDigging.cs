using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveDigging : MonoBehaviour
{
    public GameObject emptyBoi;
    public GameObject moreHealthMusic;
    public GameObject miningSound;
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

    [SerializeField]
    public AudioSource boneCollectSound;

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
        DashboardController.hasSkull = 0;
        DashboardController.hasLeg = 0;
        DashboardController.hasArm = 0;
        boneCollectSound.Stop();
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
        Vector3 nextPos = new Vector3(x,y,0);

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(new Vector2(x, y));
        
        if(hitColliders.Length == 0){
            transform.position = nextPos;
            yield return new WaitForSeconds(0.1f);
        }
        else{
            int i = 0;
            for(int index = 0; index < hitColliders.Length; index++){
                if(!hitColliders[index].CompareTag("Blackout")) i=index;
            }
            if(hitColliders[i].CompareTag("Snake")){
                DashboardController.hp -= 10;
                yield return new WaitForSeconds(0.1f);

            }
            else if(hitColliders[i].CompareTag("Heart")){
                GameObject music = Instantiate(moreHealthMusic, nextPos, Quaternion.identity);
                if(DashboardController.hp < (100 - 10)){
                    DashboardController.hp += 10;
                }
                else if(DashboardController.hp!=100){
                    DashboardController.hp = 100;
                }
                TerrainGeneration.breakBlock(nextPos);
                Destroy(hitColliders[0].gameObject);
                transform.position = nextPos;
                Destroy(music, 5f);
                yield return new WaitForSeconds(0.1f);
            }
            else if (hitColliders[i].CompareTag("FossilPart")){
                if(DashboardController.hasLeg==0) DashboardController.hasLeg ++;
                else if(DashboardController.hasLeg==1) DashboardController.hasLeg ++;
                // else if(DashboardController.hasSonar) DashboardController.hasLeg ++;
                else if (DashboardController.hasArm<=1) DashboardController.hasArm ++;
                // else if (DashboardController.hasArm<=2) DashboardController.hasGrappling = true;
                else DashboardController.hasSkull ++;
                TerrainGeneration.breakBlock(nextPos);
                Destroy(hitColliders[i].gameObject);
                yield return new WaitForSeconds(0.1f);
            }
            else if (hitColliders[i].TryGetComponent<BlockBreaking>(out BlockBreaking blockBreakingComponent))
            {
                GameObject block = hitColliders[i].gameObject;
                isBreaking = true;
                noItemInBlock = true;
                particles.SetActive(true);
                GameObject breakingSound = Instantiate(miningSound,  nextPos, Quaternion.identity);
                yield return new WaitForSeconds(blockBreakingComponent.breakBlock());
                if (noItemInBlock){
                    transform.position = nextPos;
                    if(MiniMap.step-0.2f > MiniMap.maxStep) MiniMap.step -= 0.1f;
                    else MiniMap.step = MiniMap.maxStep+0.2f;
                }
                DashboardController.hp -= 1;
                particles.SetActive(false);
                Destroy(breakingSound);
                isBreaking = false;
            }
            else if (hitColliders[0].CompareTag("FossilPart")){
                if(DashboardController.hasLeg==0) DashboardController.hasLeg ++;
                else if(DashboardController.hasLeg==1) DashboardController.hasLeg ++;
                // else if(DashboardController.hasSonar) DashboardController.hasLeg ++;
                else if (DashboardController.hasArm<=1) DashboardController.hasArm ++;
                // else if (DashboardController.hasArm<=2) DashboardController.hasGrappling = true;
                else DashboardController.hasSkull ++;
                TerrainGeneration.breakBlock(nextPos);
                Destroy(hitColliders[0].gameObject);
                boneCollectSound.Play();
                yield return new WaitForSeconds(0.1f);
            }
            else if (hitColliders[0].gameObject != this.gameObject)
            {
                GameObject block = hitColliders[0].gameObject;
                isBreaking = true;
                noItemInBlock = true;
                particles.SetActive(true);
                GameObject breakingSound = Instantiate(miningSound,  nextPos, Quaternion.identity);
                yield return new WaitForSeconds(block.GetComponent<BlockBreaking>().breakBlock());
                if (noItemInBlock) transform.position = nextPos;
                DashboardController.hp -= 1;
                particles.SetActive(false);
                Destroy(breakingSound);
                isBreaking = false;
            }
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
