using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGeneration : MonoBehaviour
{
    public Transform player;
    public float scale=1f;
    public int offset;
    public int renderDistanceX, renderDistanceY;
    private int maxWidth, maxHeight, minWidth, minHeight;
    private int newMaxWidth, newMaxHeight, newMinWidth, newMinHeight;
    public int width, height;
    public GameObject[] materials; //First dirt, then stone, finally gold
    private Vector3Int posInGrid = Vector3Int.zero;
    static private Grid thisGrid;
    [HideInInspector] public static Dictionary<Vector3Int,int[]> gridMap = new Dictionary<Vector3Int,int[]>();
    private string[] tagArray = new string[7] {"Untagged","FossilFuel","Bomb","Fossil","Monster","Health","Snake"};
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
        gridMap[new Vector3Int(0,0,0)] = new int[2] {-1,0};
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
        if(randomNoise > 0.85) choice = 2;
        else if(randomNoise<0.805 && randomNoise>0.795) choice = 5;
        else if(randomNoise<0.5002 && randomNoise>0.4998) choice = 3;
        else if(randomNoise<0.305 && randomNoise>0.295) choice = 1;
        else if(randomNoise < 0.15) choice = 4;
        return choice;
    }
    public static void addTileInMap(int tileIndex, int x, int y, int tag){
        gridMap.Add(new Vector3Int(x,y,0),new int[2] {tileIndex, tag});
    }
    private void spawnTile(int tileIndex, int x, int y, string tag){
        Vector3 posInWorld = thisGrid.CellToWorld(new Vector3Int(x,y,0));
        GameObject tile = materials[tileIndex];
        tile = Instantiate(tile, posInWorld, Quaternion.identity);
        tile.transform.parent = this.transform;
        tile.tag = tag;
    }

    private void calculateBoundery(){
        posInGrid = thisGrid.WorldToCell(player.position);
        newMaxWidth = posInGrid.x+renderDistanceX;
        newMaxHeight = posInGrid.y+renderDistanceY;
        newMinWidth = posInGrid.x-renderDistanceX;
        newMinHeight = posInGrid.y-renderDistanceY;
    }


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
                Vector3 worldPos = thisGrid.CellToWorld(new Vector3Int(x, y, 0));
                Collider2D[] hitColliders = Physics2D.OverlapPointAll((Vector2)worldPos);
                if(hitColliders.Length == 0){
                    int[] indexAndTag = gridMap[new Vector3Int(x, y, 0)];
                    if(indexAndTag[0] != -1) spawnTile(indexAndTag[0], x, y, tagArray[indexAndTag[1]]);
                }
            }
        }
    }
    void clearBlocks(int Xmin, int Ymin, int Xmax, int Ymax){
        Vector3 posBottomLeft = thisGrid.CellToWorld(new Vector3Int(Xmin, Ymin,0));
        Vector3 posTopRight = thisGrid.CellToWorld(new Vector3Int(Xmax, Ymax,0));
        Collider2D[] hitColliders = Physics2D.OverlapAreaAll((Vector2)posBottomLeft, (Vector2)posTopRight);
        for(int i = 0; i<hitColliders.Length; i++){
            Destroy(hitColliders[i].gameObject);
        }
    }
    public static Vector3Int ConvertToGridCoord(Vector3 WorldPos){
        return thisGrid.WorldToCell(WorldPos);
    }
}
