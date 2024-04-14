using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MPE;
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
    public enum DrawMode {NoiseMap, ColorMap, ErosionMap, ContinentalnessMap, PeaksAndValleysMap};
	public DrawMode drawMode;
    public TerrainType[] regions;
    public Vector2[] continentalnessHeights;
    public Vector2[] erosionHeights;
    public Vector2[] peaksAndValleysHeights;
    public void GenerateMap() {
        
        // float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, scale, layers, lacunarity, persistence, seed, offset);
        // Color[] colorMap = new Color[mapWidth * mapHeight];
        // for ( int x = 0; x < mapWidth; x++ ) {
        //     for ( int y = 0; y < mapHeight; y++ ) {
        //         float currentHeight = noiseMap[x, y];
        //         for ( int i = 0; i < regions.Length; i++ ) {
        //             if ( currentHeight < regions[i].height) {
        //                 colorMap[y * mapWidth + x] = regions[i].color;
        //                 break;
        //             }
        //         }
        //     }
        // }
        // MapRender display = FindObjectOfType<MapRender>();
        // if ( drawMode == DrawMode.NoiseMap ) {
        //     display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        // } else if ( drawMode == DrawMode.ColorMap ) {
        //     display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        // }
		
        float[,] continentalness = Noise.GenerateNoiseMap(mapWidth, mapHeight, scale, layers, lacunarity, persistence, seed, offset, continentalnessHeights);
        float[,] erosion = Noise.GenerateNoiseMap(mapWidth, mapHeight, scale, layers, lacunarity, persistence, seed, offset, erosionHeights);
        float[,] peaksAndValleys = Noise.GenerateNoiseMap(mapWidth, mapHeight, scale, layers, lacunarity, persistence, seed, offset, peaksAndValleysHeights);
        float[,] noiseMap = new float[mapWidth, mapHeight];
        Color[] colorMap = new Color[mapWidth * mapHeight];
        for ( int x = 0; x < mapWidth; x++ ) {
            for ( int y = 0; y < mapHeight; y++ ) {
                float noiseVal = (continentalness[x, y] + erosion[x, y] + peaksAndValleys[x, y]) / 3f;
                float currentHeight = noiseVal;
                noiseMap[x, y] = noiseVal;
                
                // draw colors
                for ( int i = 0; i < regions.Length; i++ ) {
                    if ( currentHeight < regions[i].height) {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }
        MapRender display = FindObjectOfType<MapRender>();
        if ( drawMode == DrawMode.NoiseMap ) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        } else if ( drawMode == DrawMode.ContinentalnessMap ) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(continentalness));
        } else if ( drawMode == DrawMode.ErosionMap ) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(erosion));
        } else if ( drawMode == DrawMode.PeaksAndValleysMap ) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(peaksAndValleys));
        } else if ( drawMode == DrawMode.ColorMap ) {
            
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
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
        for ( int i = 0; i < continentalnessHeights.Length; i++ ) {
            if ( continentalnessHeights[i].x < 0) {
                continentalnessHeights[i].x = 0;
            } else if ( continentalnessHeights[i].x > 1) {
                continentalnessHeights[i].x = 1;
            }
            if ( continentalnessHeights[i].y < 0) {
                continentalnessHeights[i].y = 0;
            } else if ( continentalnessHeights[i].y > 1) {
                continentalnessHeights[i].y = 1;
            }
        }
        
    }
}
[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color color;
}
