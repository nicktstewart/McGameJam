using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is put on a tile block
public class BlockBreaking : MonoBehaviour
{
    //This is should be called when the player wants this block
    public float breakBlock(){
        float breakTime = 0.1f;
        //Stuff that happens when breaking the block
        switch(this.tag){
            case "Health":
                //Code executed when breaking a HEALTH block
                breakTime = 0.5f;
                break;

            case "FossiFuel":
                //Code executed when breaking a FOSSIL FUEL block
                breakTime = 0.5f;
                break;

            case "Bomb":
                //Code executed when breaking a BOMB block
                breakTime = 1f;
                break;

            case "Monster":
                //Code executed when breaking a MONSTER block
                breakTime = 2f;
                break;

            case "Fossil":
                //Code executed when breaking a FOSSIL block
                breakTime = 1f;
                break;
        }
        //kill the gameobject
        StartCoroutine(KillBlock(breakTime));
        return breakTime;
    }
    IEnumerator KillBlock(float waitTime){
        yield return new WaitForSeconds(waitTime);
        Destroy(this);
    }
}
