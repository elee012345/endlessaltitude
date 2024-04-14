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
    public int layers;
    public float lacunarity;
    public float persistence;
    public int seed;
    public bool autoUpdate;
    public Vector2 offset;

    public void GenerateNoiseMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, scale, layers, lacunarity, persistence, seed, offset);
        MapRender display = FindObjectOfType<MapRender>();
		display.RenderMap(noiseMap);
        
    }

    void OnValidate() {
        if (scale <= 0) {
			scale = 0.001f;
		}
        if ( mapWidth < 1 ) {
            mapWidth = 1;
        }
        if ( mapHeight < 1 ) {
            mapHeight = 1;
        }
        if ( lacunarity <= 0 ) {
            lacunarity = 0.001f;
        }
        if ( persistence <= 0 ) {
            persistence = 0.001f;
        }
        if ( layers < 1 ) {
            layers = 1;
        }
    }
}
