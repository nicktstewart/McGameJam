using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is put on a tile block
//It is should be called when the player wants this block
public class BlockBreaking : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void breakBlock(){
        //Stuff that happens when breaking the block
        switch(this.tag){
            case "Health":
                //Code executed when breaking a HEALTH block
                break;

            case "FossiFuel":
                //Code executed when breaking a FOSSIL FUEL block
                break;

            case "Bomb":
                //Code executed when breaking a BOMB block
                break;

            case "Monster":
                //Code executed when breaking a MONSTER block
                break;

            case "Fossil":
                //Code executed when breaking a FOSSIL block
                break;
        }
        //kill the gameobject
        Destroy(this);
    }
}
