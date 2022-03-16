using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is put on a tile block
public class BlockBreaking : MonoBehaviour
{
    public GameObject snakeSound;
    delegate void AfterWaitTime();
    AfterWaitTime afterWaitTime;
    Vector3Int posInGrid;
    //This is should be called when the player wants this block
    public float breakBlock(){
        float breakTime = 0.25f;
        
        afterWaitTime = nothing;
        //Stuff that happens when breaking the block
        switch(tag){
            case "Health":
                //Code executed when breaking a HEALTH block
                breakTime = 1.5f;
                afterWaitTime = addHealth;
                break;

            case "FossilFuel":
                //Code executed when breaking a FOSSIL FUEL block
                breakTime = 1f;
                afterWaitTime = addFossilFuel;
                break;

            case "Bomb":
                //Code executed when breaking a BOMB block
                breakTime = 1f;
                afterWaitTime = spawnBomb;
                break;

            case "Monster":
                //Code executed when breaking a MONSTER block
                breakTime = 2f;
                afterWaitTime = spawnMonster;
                break;

            case "Fossil":
                //Code executed when breaking a FOSSIL block
                breakTime = 1.5f;
                afterWaitTime = foundFossil;
                break;
        }
        //kill the gameobject
        StartCoroutine(KillBlock(breakTime));
        return breakTime;
    }
    IEnumerator KillBlock(float waitTime){
        yield return new WaitForSeconds(waitTime);
        posInGrid = transform.parent.GetComponent<Grid>().WorldToCell(transform.position);
        TerrainGeneration.breakBlock(posInGrid);
        afterWaitTime();
        Destroy(gameObject);
    }

    void nothing(){}

    void addHealth(){
        PlayerMoveDigging.noItemInBlock = false;
        TerrainGeneration.gridMap[posInGrid] = new int[3] {4,7,0};
    }
    void addFossilFuel(){
        //add fossil fuel
    }
    void spawnBomb(){
        //spawn Bomb

    }
    void spawnMonster(){
        //spawn Monster
        PlayerMoveDigging.noItemInBlock = false;
        TerrainGeneration.gridMap[posInGrid] = new int[3] {3,6,0};
        GameObject SFX = Instantiate(snakeSound, transform.position, Quaternion.identity);
        Destroy(SFX, 4f);
    }
    void foundFossil(){
        PlayerMoveDigging.noItemInBlock = false;
        if (DashboardController.hasLeg==0) TerrainGeneration.gridMap[posInGrid] = new int[3] {5,8,0};
        else if(DashboardController.hasLeg==1) TerrainGeneration.gridMap[posInGrid] = new int[3] {5,8,0};
        // else if(DashboardController.hasSonar) DashboardController.hasLeg ++;
        else if (DashboardController.hasArm<=1) TerrainGeneration.gridMap[posInGrid] = new int[3] {6,8,0};
        // else if (DashboardController.hasArm<=2) DashboardController.hasGrappling = true;
        else TerrainGeneration.gridMap[posInGrid] = new int[3] {7,8,0};
        
    }
}
