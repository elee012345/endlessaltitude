using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float scale;

    public bool autoUpdate;

    public void GenerateNoiseMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, scale);
        MapRender display = FindObjectOfType<MapRender> ();
		display.RenderMap (noiseMap);
        
    }
}
