using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject explotionSound;
    // Start is called before the first frame update
    // void Start()
    // {
    // }

    // Update is called once per frame
    void Update()
    {
        Vector3 nextPos = transform.position - transform.right*Time.deltaTime*6;
        bool noCollision = CheckNextBlock(nextPos - transform.right);
        if (noCollision) transform.position = nextPos;
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
                    TerrainGeneration.breakBlock(nextPos);
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
