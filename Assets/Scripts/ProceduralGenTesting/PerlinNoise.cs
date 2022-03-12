using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{

    public int width = 256;
    public int height = 256;
    public int offsetX = 0;
    public int offsetY = 0;

    public float scale = 1f;
    void Update()
    {
       Renderer renderer = GetComponent<Renderer>();
       renderer.material.mainTexture = CreateTexture();
    }

    Texture2D CreateTexture(){
        Texture2D texture = new Texture2D(width,height);

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                Color color = GenerateColor(x+offsetX, y+offsetY);
                texture.SetPixel(x,y,color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color GenerateColor(int x, int y){
        float colorX = (float)x / width * scale;
        float colorY = (float)y / height * scale;
        
        float colorIntensity = Mathf.PerlinNoise(colorX, colorY);
        if(colorIntensity > 0.9) colorIntensity = 1;
        else if(colorIntensity < 0.1) colorIntensity = 0.5f;
        else colorIntensity = 0;
        return new Color(colorIntensity, colorIntensity, colorIntensity);
    }
}
