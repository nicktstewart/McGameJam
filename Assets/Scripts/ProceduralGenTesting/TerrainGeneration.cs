using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGeneration : MonoBehaviour
{
    public bool randomSeed; //Make the game random ,should be turned on (used for debugging)
    public Transform player;
    public float scale=1f;
    public int offset;
    public int renderDistanceX, renderDistanceY;
    private int maxWidth, maxHeight, minWidth, minHeight;
    private int newMaxWidth, newMaxHeight, newMinWidth, newMinHeight;
    public int width, height; //these variables could be taken out with a bit of code tweking
    public GameObject[] materials; //First dirt, then stone, finally gold (and more...)
    private Vector3Int posInGrid = Vector3Int.zero;
    static private Grid thisGrid;

    //should make it private and create setter and getter methods
    [HideInInspector] public static Dictionary<Vector3Int,int[]> gridMap = new Dictionary<Vector3Int,int[]>();
    private string[] tagArray = new string[10] {"Untagged","FossilFuel","Bomb","Fossil","Monster","Health","Snake","Heart","FossilPart","Blackout"};
    // Start is called before the first frame update
    void Start()
    {
        if(randomSeed) offset = Random.Range(0,800000);
        gridMap = new Dictionary<Vector3Int,int[]>(); //initiate the Map
        thisGrid = this.GetComponent<Grid>();
        calculateBoundery();
        maxHeight = newMaxHeight;
        maxWidth = newMaxWidth;
        minHeight = newMinHeight;
        minWidth = newMinWidth;

        Generation(minWidth, minHeight, maxWidth, maxHeight);
        breakBlock(new Vector3Int(0,0,0));
    }

    //Method that generates the map from the point (X,Y) to the point (Xo,Yo)
    void Generation(int Xo, int Yo, int X, int Y){
        for(int x=Xo; x<X; x++){
            for(int y=Yo; y<Y; y++){
                int randomNoise = generateRandomNoise(x,y);
                int randomIndex = Random.Range(0, 3);
                switch (randomNoise)
                {
                    case 0:
                        addTileInMap(0,x,y,randomNoise);
                        break;
                    case 1:
                        addTileInMap(1,x,y,randomNoise);
                        break;
                    case 2:
                        addTileInMap(randomIndex,x,y,randomNoise);
                        break;
                    case 3:
                        addTileInMap(2,x,y,randomNoise);
                        break;
                    case 4:
                        addTileInMap(randomIndex,x,y,randomNoise);
                        break;
                    case 5:
                        addTileInMap(2,x,y,randomNoise);
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
        if(randomNoise > 0.88) choice = 3;
        else if(randomNoise<0.88 && randomNoise>0.55) choice = 1;
        // else if(randomNoise<0.805 && randomNoise>0.795) choice = 5;
        else if(randomNoise<0.25 && randomNoise>0.15) choice = 5;
        // else if(randomNoise<0.37 && randomNoise>0.23) choice = 0;
        else if(randomNoise < 0.15) choice = 4;
        return choice;
    }
    public static void addTileInMap(int tileIndex, int x, int y, int tag){
        gridMap.Add(new Vector3Int(x,y,0),new int[3] {tileIndex, tag, 1});
    }
    private void spawnTile(int tileIndex, int x, int y, string tag){
        Vector3 posInWorld = thisGrid.CellToWorld(new Vector3Int(x,y,0));
        GameObject tile = materials[tileIndex];
        tile = Instantiate(tile, posInWorld, Quaternion.identity);
        tile.transform.parent = this.transform;
        tile.tag = tag;
    }

    //Method that checks if 
    private void calculateBoundery(){
        posInGrid = thisGrid.WorldToCell(player.position);
        newMaxWidth = posInGrid.x+renderDistanceX;
        newMaxHeight = posInGrid.y+renderDistanceY;
        newMinWidth = posInGrid.x-renderDistanceX;
        newMinHeight = posInGrid.y-renderDistanceY;
    }

    // TODO: could clean this up by rendering the whole screen (render distance), and checking if the key is in the dictionnary
    // Update is called once per frame
    void Update()
    {
        //Rendering new blocks out of the renderDistance
        calculateBoundery();
        if(newMaxHeight>maxHeight){
            Generation(minWidth,maxHeight,maxWidth,newMaxHeight);
            maxHeight = newMaxHeight;
        }
        if(newMinHeight<minHeight){
            Generation(minWidth,newMinHeight,maxWidth,minHeight);
            minHeight = newMinHeight;
        }
        if(newMaxWidth>maxWidth){
            Generation(maxWidth,minHeight,newMaxWidth,maxHeight);
            maxWidth = newMaxWidth;
        }
        if(newMinWidth<minWidth){
            Generation(newMinWidth,minHeight,minWidth,maxHeight);
            minWidth = newMinWidth;
        }
        RenderBlocks(posInGrid);
    }

    //Method that takes out all the blocks not in render distance and spawn all the blocks that should be on the screen (without making doublicates)
    void RenderBlocks (Vector3Int playerPos){
        //Detect if any blocks are out of the render distance of the player
        //left side
        clearBlocks(maxWidth,minHeight,newMaxWidth,maxHeight);
        //right side
        clearBlocks(newMinWidth,minHeight,minWidth,maxHeight);
        //top side
        clearBlocks(maxWidth,newMaxHeight,minWidth,maxHeight);
        //bottom side
        clearBlocks(maxWidth,minHeight,minWidth,newMinHeight);
        
        for(int x=newMinWidth; x<newMaxWidth; x++){
            for(int y=newMinHeight; y<newMaxHeight; y++){
                int[] indexAndTag = gridMap[new Vector3Int(x, y, 0)];
                Vector3 worldPos = thisGrid.CellToWorld(new Vector3Int(x, y, 0));
                Collider2D[] hitColliders = Physics2D.OverlapPointAll((Vector2)worldPos);
                if(hitColliders.Length == 0){
                    if(indexAndTag[0] != -1) spawnTile(indexAndTag[0], x, y, tagArray[indexAndTag[1]]);
                }
                else{
                    bool blackoutPresent = false;
                    for (int i = 0; i < hitColliders.Length; i++){
                        if (hitColliders[i].CompareTag("Blackout")){
                            blackoutPresent = true;
                            if(indexAndTag[2] == 0){
                                Debug.Log("detected");
                                Destroy(hitColliders[i].gameObject);
                            }
                        }
                    }
                    if(!blackoutPresent && indexAndTag[2] == 1) spawnTile(8,x,y,tagArray[9]);
                }
                
            }
        }
    }
    
    //Method that take out anything with a collider between (Xmin, Ymin) and (Xmax, Ymax)
    void clearBlocks(int Xmin, int Ymin, int Xmax, int Ymax){
        Vector3 posBottomLeft = thisGrid.CellToWorld(new Vector3Int(Xmin, Ymin,0));
        Vector3 posTopRight = thisGrid.CellToWorld(new Vector3Int(Xmax, Ymax,0));
        Collider2D[] hitColliders = Physics2D.OverlapAreaAll((Vector2)posBottomLeft, (Vector2)posTopRight);
        for(int i = 0; i<hitColliders.Length; i++){
            Destroy(hitColliders[i].gameObject);
        }
    }

    //Method that converts the world coordinates to the grid coordinates
    public static Vector3Int ConvertToGridCoord(Vector3 WorldPos){
        return thisGrid.WorldToCell(WorldPos);
    }

    //Being able to call this function with worldPos and gridPos is just easier in the rest of the code
    public static void breakBlock(Vector3 worldPos){
        Vector3Int gridPos = ConvertToGridCoord(worldPos);
        breakInGrid(gridPos);
    }
    public static void breakBlock(Vector3Int gridPos){
        breakInGrid(gridPos);
    }
    private static void breakInGrid(Vector3Int gridPos){
        //set the center block to void
        gridMap[gridPos] = new int[] {-1,0,0};
        //Take out the blackout
        // Debug.Log(gridPos);
        // Debug.Log(gridPos.x-1);
        // Debug.Log(gridPos.x+1);
        // Debug.Log(gridPos.y);
        for (int X = gridPos.x-1 ; X < gridPos.x+2; X++){
            for(int Y = gridPos.y-1; Y < gridPos.y+2; Y++){
                int[] intValues = gridMap[new Vector3Int(X,Y,0)];
                gridMap[new Vector3Int(X,Y,0)] = new int[3] {intValues[0],intValues[1],0};
            }
        }
    }
}
