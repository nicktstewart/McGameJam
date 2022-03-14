using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject explotionSound;
    private bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 nextPos = transform.position - transform.right/5;
        bool noCollision = CheckNextBlock(nextPos - transform.right);
        if (noCollision) transform.position = nextPos;
        // if(canMove) StartCoroutine(Move());
    }
    bool CheckNextBlock(Vector3 nextPos){
        Collider2D[] hitColliders = Physics2D.OverlapPointAll((Vector2)(nextPos));
        if (hitColliders.Length == 0 || hitColliders[0].transform == transform.parent) return true;
        else{
            for (int i = 0; i<hitColliders.Length; i++){
                if(hitColliders[i].CompareTag("Player")){
                    DashboardController.hp -= 10;
                }
                else if (hitColliders[i].CompareTag("Snake")){
                    TerrainGeneration.gridMap[TerrainGeneration.ConvertToGridCoord(nextPos)] = new int[2]{-1,0};
                    Destroy(hitColliders[i].gameObject);
                }
            }
            GameObject SFX = Instantiate(explotionSound, transform.position, Quaternion.identity);
            Destroy(SFX, 2f);
            Destroy(gameObject);
            return false;
        }
        
    }
}
