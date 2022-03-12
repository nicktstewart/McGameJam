using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is put on a tile block
public class BlockBreaking : MonoBehaviour
{
    delegate void AfterWaitTime();
    AfterWaitTime afterWaitTime;
    //This is should be called when the player wants this block
    public float breakBlock(){
        float breakTime = 0.1f;

        //Stuff that happens when breaking the block
        switch(this.tag){
            case "Health":
                //Code executed when breaking a HEALTH block
                breakTime = 0.5f;
                afterWaitTime = addHealth;
                break;

            case "FossiFuel":
                //Code executed when breaking a FOSSIL FUEL block
                breakTime = 0.5f;
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
                breakTime = 1f;
                afterWaitTime = foundFossil;
                break;
        }
        //kill the gameobject
        StartCoroutine(KillBlock(breakTime));
        return breakTime;
    }
    IEnumerator KillBlock(float waitTime){
        yield return new WaitForSeconds(waitTime);
        afterWaitTime();
        Debug.Log("test");
        Destroy(this);
    }

    void addHealth(){
        DashboardController.hp ++;
    }
    void addFossilFuel(){
        //add fossil fuel
    }
    void spawnBomb(){
        //spawn Bomb

    }
    void spawnMonster(){
        //spawn Monster
    }
    void foundFossil(){
        if (DashboardController.hasLeg<=2){
            DashboardController.hasLeg ++;
        }
        else if (DashboardController.hasArm<=2) DashboardController.hasArm ++;
        else DashboardController.hasSkull ++;
        
    }
}
