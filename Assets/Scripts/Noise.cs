using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{

    
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int layers, float lacunarity, float persistence, int seed, Vector2 offset, Vector2[] heightAdjustments) {
		
		
        if ( heightAdjustments.Length < 2 ) {
            heightAdjustments[0] = new Vector2(0, 0);
            heightAdjustments[1] = new Vector2(1, 1);
        }
        float[,] noiseMap = new float[mapWidth,mapHeight];
        System.Random rng = new System.Random(seed);
        Vector2[] layerOffsets = new Vector2[layers]; 
        for ( int i = 0; i < layers; i++ ) {
			float offsetY = rng.Next(-100000, 100000) + offset.y;
            float offsetX = rng.Next(-100000, 100000) + offset.x;
			layerOffsets[i] = new Vector2(offsetX, offsetY);
        }
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for (int i = 0; i < layers; i++ ) {
                    float sampleX = (x - halfWidth) / scale * frequency + layerOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + layerOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if ( noiseHeight > maxNoiseHeight ) {
                    maxNoiseHeight = noiseHeight;
                } 
                if ( noiseHeight < minNoiseHeight ) {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap [x, y] = noiseHeight;
                
				
			}
		}

        for (int y = 0; y < mapHeight; y++ ) {
            for ( int x = 0; x < mapWidth; x++ ) {
                float currentNoiseVal = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                noiseMap[x, y] = currentNoiseVal;
                for (int i = 1; i < heightAdjustments.Length; i++ ) {
                    if ( currentNoiseVal <= heightAdjustments[i].x ) {
                        currentNoiseVal = Mathf.Lerp(heightAdjustments[i-1].y, heightAdjustments[i].y, (currentNoiseVal - heightAdjustments[i-1].x) / (heightAdjustments[i].x - heightAdjustments[i-1].x));
                        noiseMap[x, y] = currentNoiseVal;
                        break;
                    }
                
                }
            }
        }

		return noiseMap;
	}
}
