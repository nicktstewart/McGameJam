using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class MiniMap : MonoBehaviour
{
    public int width, height;
    public int step;
    public GameObject player;
    private float scale;
    public static int offset;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        scale = TerrainGeneration.scale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int playerPos = TerrainGeneration.ConvertToGridCoord(player.transform.position);
        spriteRenderer.sprite = Sprite.Create(GenerateTexture(playerPos), new Rect(0, 0, width, height), new Vector2(0, 0), (float)width);
        // thisRenderer.material.mainTexture = GenerateTexture(playerPos);
    }
    Texture2D GenerateTexture(Vector3Int gridPlayerPos){
        Texture2D texture = new Texture2D(width,height);
        // int Xmax = gridPlayerPos.x + (width)/2;
        int Xmin = gridPlayerPos.x*step - (width)/2;
        // int Ymax = gridPlayerPos.y + (height)/2;
        int Ymin = gridPlayerPos.y*step - (height)/2;
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                texture.SetPixel(x,y,generateRandomNoise((x+Xmin)/step,(Ymin+y)/step));
                if((x-width/2)/(step/2) == 0 && (y-height/2)/(step/2) == 0) texture.SetPixel(x,y,new Color(0.5f,0.2f,0.2f));
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
        if(randomNoise > 0.88) choice = 1;
        else if(randomNoise<0.88 && randomNoise>0.55) choice = 0.2f;
        // // else if(randomNoise<0.805 && randomNoise>0.795) choice = 5;
        // else if(randomNoise<0.25 && randomNoise>0.15) choice = 5;
        // // else if(randomNoise<0.37 && randomNoise>0.23) choice = 0;
        // else if(randomNoise < 0.15) choice = 4;
        return new Color(choice,choice,choice);
    }
}
