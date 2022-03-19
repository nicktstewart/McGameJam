using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class MiniMap : MonoBehaviour
{
    static public int sideSize;
    public float initStep;
    static public float step;
    public static float maxStep;
    public GameObject player;
    private float scale;
    public static int offset;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        step = initStep;
        maxStep = 5;
        sideSize = 100;
        scale = TerrainGeneration.scale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int playerPos = TerrainGeneration.ConvertToGridCoord(player.transform.position);
        spriteRenderer.sprite = Sprite.Create(GenerateTexture(playerPos), new Rect(0, 0, sideSize, sideSize), new Vector2(0, 0), (float)sideSize);
        // thisRenderer.material.mainTexture = GenerateTexture(playerPos);
    }
    Texture2D GenerateTexture(Vector3Int gridPlayerPos){
        Texture2D texture = new Texture2D(sideSize,sideSize);
        // int Xmax = gridPlayerPos.x + (width)/2;
        float Xmin = gridPlayerPos.x*step - (sideSize)/2;
        // int Ymax = gridPlayerPos.y + (height)/2;
        float Ymin = gridPlayerPos.y*step - (sideSize)/2;
        
        for (int x = 0; x < sideSize; x++){
            for (int y = 0; y < sideSize; y++){
                int xMinimap = (int)Mathf.Round((x+Xmin)/step);
                int yMinimap = (int)Mathf.Round((y+Ymin)/step);
                texture.SetPixel(x,y,findPixelInGrid(xMinimap,yMinimap));
                if(xMinimap == gridPlayerPos.x && yMinimap == gridPlayerPos.y) texture.SetPixel(x,y,new Color(0.5f,0.2f,0.2f));
            }
        }
        texture.Apply();
        return texture;
    }
    Color generateRandomNoise(int x, int y){
        float perlinX = (float)(x+offset) / scale;
        float perlinY = (float)(y+offset) / scale;
        
        float randomNoise = Mathf.PerlinNoise(perlinX, perlinY);

        //0:Stone, 1:fossilFuel, 2:bomb, 3:fossil, 4:monsterBlock, 5:healthBlock
        float choice = 0f;
        // if(randomNoise > 0.88) choice = 1;
        // else if(randomNoise<0.88 && randomNoise>0.55) choice = 0.2f;
        if(randomNoise-0.5 > 0) choice = (randomNoise-0.5f)*2;
        return new Color(choice,choice,choice);
    }
    Color findPixelInGrid(int x, int y){
        Color pixelColor;
        int material = TerrainGeneration.gridMap[new Vector3Int(x, y, 0)][1];
        pixelColor = new Color(0,0,0);
        switch(material){
            case 1:
                pixelColor = new Color(0.3f, 0.3f, 0.3f);
                break;
            case 3:
                pixelColor = new Color(1f, 1f, 1f);
                break;
        }
        if(TerrainGeneration.gridMap[new Vector3Int(x, y, 0)][0] == -1) pixelColor = new Color(87f/255, 64f/255, 40f/255);
        return pixelColor;
    }
}
