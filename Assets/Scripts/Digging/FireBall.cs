using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canMove) StartCoroutine(Move());
    }
    IEnumerator Move(){
        canMove = false;
        Vector3 nextPos = transform.position - transform.right*3.8f;
        yield return new WaitForSeconds(0.5f);
        bool noCollision = CheckNextBlock(nextPos);
        if (noCollision) transform.position = nextPos;
        canMove = true;
    }
    bool CheckNextBlock(Vector3 nextPos){
        Collider2D[] hitColliders = Physics2D.OverlapPointAll((Vector2)(nextPos));
        if (hitColliders.Length == 0) return true;
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
            Destroy(gameObject);
            return false;
        }
        
    }
}
