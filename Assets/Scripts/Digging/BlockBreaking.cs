using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is put on a tile block
public class BlockBreaking : MonoBehaviour
{
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
        Debug.Log(breakTime);
        //kill the gameobject
        StartCoroutine(KillBlock(breakTime));
        return breakTime;
    }
    IEnumerator KillBlock(float waitTime){
        yield return new WaitForSeconds(waitTime);
        if(PlayerMoveDigging.isDrilling){
            posInGrid = transform.parent.GetComponent<Grid>().WorldToCell(transform.position);
            TerrainGeneration.gridMap[posInGrid] = new int[2] {-1,0};
            afterWaitTime();
            Destroy(gameObject);
        }
    }

    void nothing(){}

    void addHealth(){
        PlayerMoveDigging.noItemInBlock = false;
        TerrainGeneration.gridMap[posInGrid] = new int[2] {4,7};
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
        TerrainGeneration.gridMap[posInGrid] = new int[2] {3,6};
    }
    void foundFossil(){
        PlayerMoveDigging.noItemInBlock = false;
        if (DashboardController.hasLeg==0) TerrainGeneration.gridMap[posInGrid] = new int[2] {5,8};
        else if(DashboardController.hasLeg==1) TerrainGeneration.gridMap[posInGrid] = new int[2] {5,8};
        // else if(DashboardController.hasSonar) DashboardController.hasLeg ++;
        else if (DashboardController.hasArm<=1) TerrainGeneration.gridMap[posInGrid] = new int[2] {6,8};
        // else if (DashboardController.hasArm<=2) DashboardController.hasGrappling = true;
        else TerrainGeneration.gridMap[posInGrid] = new int[2] {7,8};
        
    }
}
