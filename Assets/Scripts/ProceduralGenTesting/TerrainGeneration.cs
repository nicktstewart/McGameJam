using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGeneration : MonoBehaviour
{
    public Transform player;
    public float scale=1f;
    public int offset;
    public int renderDistance;
    public int maxWidth, maxHeight, minWidth, minHeight;
    public int newMaxWidth, newMaxHeight, newMinWidth, newMinHeight;
    public int width, height;
    public Tilemap stoneMap, fossilFuelMap, fossilMap, bombMap, monsterBlockMap, healthMap;
    public GameObject stone, fossilFuel, fossil, bomb, monsterBlock, health; 
    private Grid thisGrid;
    // Start is called before the first frame update
    void Start()
    {
        thisGrid = this.GetComponent<Grid>();
        calculateBoundery();
        maxHeight = newMaxHeight;
        maxWidth = newMaxWidth;
        minHeight = newMinHeight;
        minWidth = newMinWidth;

        Generation(minWidth, minHeight, maxWidth, maxHeight);
    }

    //Method that generates the map from the point (X,Y) to the point (Xo,Yo)
    void Generation(int Xo, int Yo, int X, int Y){
        for(int x=Xo; x<=Y; x++){
            for(int y=Yo; y<=Y; y++){
                int randomNoise = generateRandomNoise(x,y);
                switch (randomNoise)
                {
                    case 0:
                        spawnTile(stone,stoneMap,x,y);
                        break;
                    case 1:
                        spawnTile(fossilFuel,fossilFuelMap,x,y);
                        break;
                    case 2:
                        spawnTile(bomb,bombMap,x,y);
                        break;
                    case 3:
                        spawnTile(fossil,fossilMap,x,y);
                        break;
                    case 4:
                        spawnTile(monsterBlock,monsterBlockMap,x,y);
                        break;
                    case 5:
                        spawnTile(health,healthMap,x,y);
                        break;
                }
            }
        }
    }

    int generateRandomNoise(int x, int y){
        float perlinX = (float)(x+offset) / width * scale;
        float perlinY = (float)(y+offset) / height * scale;
        
        float randomNoise = Mathf.PerlinNoise(perlinX, perlinY);

        //0:Stone, 1:fossilFuel, 2:bomb, 3:fossil, 4:monsterBlock, 5:healthBlock
        int choice = 0;
        if(randomNoise > 0.85) choice = 2;
        else if(randomNoise<0.805 && randomNoise>0.795) choice = 5;
        else if(randomNoise<0.5002 && randomNoise>0.4998) choice = 3;
        else if(randomNoise<0.305 && randomNoise>0.295) choice = 1;
        else if(randomNoise < 0.15) choice = 4;
        return choice;
    }

    private void spawnTile(GameObject tile, Tilemap tilemap, int x, int y){
        Vector3 posInGrid = thisGrid.CellToWorld(new Vector3Int(x,y,0));
        tile = Instantiate(tile, posInGrid, Quaternion.identity);
        tile.transform.parent = tilemap.transform;
    }

    private void calculateBoundery(){
        Vector3Int posInGrid = thisGrid.WorldToCell(player.position);
        newMaxWidth = posInGrid.x+renderDistance;
        newMaxHeight = posInGrid.y+renderDistance;
        newMinWidth = posInGrid.x-renderDistance;
        newMinHeight = posInGrid.y-renderDistance;
    }


    // Update is called once per frame
    void Update()
    {
        calculateBoundery();
        if(newMaxHeight>maxHeight){
            Debug.Log("newMaxHeight>maxHeight");
            Generation(minWidth,maxHeight,maxWidth,newMaxHeight);
            maxHeight = newMaxHeight;
        }
        if(newMinHeight<minHeight){
            Debug.Log("newMinHeight<minHeight");
            Generation(minWidth,newMinHeight,maxWidth,minHeight);
            minHeight = newMinHeight;
        }
        if(newMaxWidth>maxWidth){
            Debug.Log("newMaxWidth>maxWidth");
            Generation(maxWidth,minHeight,newMaxWidth,maxHeight);
            maxWidth = newMaxWidth;
        }
        if(newMinWidth<minWidth){
            Debug.Log("newMinWidth<minWidth");
            Generation(newMinWidth,minHeight,minWidth,maxHeight);
            minWidth = newMinWidth;
        }
    }
}
