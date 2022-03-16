using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right*Time.deltaTime*10;
        CheckNextBlock(transform.position+transform.right*2);
    }
    
    bool CheckNextBlock(Vector3 nextPos){
        Collider2D[] hitColliders = Physics2D.OverlapPointAll((Vector2)(nextPos));
        if (hitColliders.Length == 0) return true;
        else{
            for (int i = 0; i<hitColliders.Length; i++){
                if(hitColliders[i].CompareTag("Player")){
                    return true;
                    // DashboardController.hp -= 10;
                }
                if (hitColliders[i].CompareTag("Snake")){
                    hitColliders[i].GetComponent<Snake>().hp --;
                    // TerrainGeneration.gridMap[TerrainGeneration.ConvertToGridCoord(nextPos)] = new int[2]{-1,0};
                    // Destroy(hitColliders[i].gameObject);
                }
            }
            Destroy(gameObject);
            return false;
        }
        
    }
}
