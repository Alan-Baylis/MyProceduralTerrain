using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static int octaves;
    public static Vector2[] octaveOffsets;
    public static System.Random rng;
    static int seed = 0;

    public static float SampleNoiseTemp(Vector3 Coord)
    {
        OpenSimplexNoise noise = new OpenSimplexNoise(seed);

        return (float)noise.Evaluate(Coord.x,Coord.z);
    }
    //public static float SampleNoise(Vector3 Coord)
    //{
    //    //int biomeId = EndlessTerrain.SampleBiomeMap(Coord);

    //    OpenSimplexNoise noise = new OpenSimplexNoise(seed);
    //    float accumulatedNoiseValue = 0;
    //    for (int i = 0; i < EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers.Length; i++)
    //    {

    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].DisableLayer == true)
    //        {
    //            continue;
    //        }
    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].EnableMaxHeightForLayer && accumulatedNoiseValue > EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].MaximumHeightForThisLayerToBeApplied)
    //        {
    //            continue;
    //        }
    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].EnableMinHeightForLayer && accumulatedNoiseValue < EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].MinimumHeightForThisLayerToBeApplied)
    //        {
    //            continue;
    //        }
    //        float amplitude = 1;
    //        float frequency = 1;
    //        float noiseHeight = 0;
    //        for (int j = 0; j < EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].NumberOfOctaves; j++)
    //        {
    //            noiseHeight += (float)noise.Evaluate((Coord.x / EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].NoiseScale * frequency + octaveOffsets[j].x), (Coord.z / EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].NoiseScale * frequency + octaveOffsets[j].y)) * amplitude;
    //            amplitude *= EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].Persistence;
    //            frequency *= EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].Lacunarity;
    //        }
    //        noiseHeight += EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].NoiseAdjustment;
    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].IgnoreNegativeValues && noiseHeight < 0)
    //        {
    //            noiseHeight = 0;
    //        }
    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].IgnorePositiveValues && noiseHeight > 0)
    //        {
    //            noiseHeight = 0;
    //        }
    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].EnableMinimumThreshold && noiseHeight < EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].MinimumThreshold)
    //        {
    //            noiseHeight = EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].MinimumThreshold;
    //        }
    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].EnableMaximumThreshold && noiseHeight > EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].MaximumThreshold)
    //        {
    //            noiseHeight = EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].MaximumThreshold;
    //        }

    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].DisableCurve == false)
    //        {
    //            AnimationCurve curveCopy = new AnimationCurve(EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].Curve.keys);
    //            EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].Curve.Evaluate(noiseHeight);
    //        }

    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].ForceNegative)
    //        {
    //            if (noiseHeight > 0)
    //            {
    //                noiseHeight *= -1;
    //            }
    //        }
    //        if (EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].ForcePositive)
    //        {
    //            if (noiseHeight < 0)
    //            {
    //                noiseHeight *= -1;
    //            }
    //        }
    //        noiseHeight = noiseHeight * EndlessTerrain.terrainGenerator.biomes[biomeId].detailLayers[i].HeightMultiplier;
    //        accumulatedNoiseValue += noiseHeight;

    //    }

    //    return accumulatedNoiseValue;
    //}

    public static void Initialize(int Seed)
    {
        rng = new System.Random(Seed);
        octaveOffsets = new Vector2[15];
        for (int i = 0; i < octaveOffsets.Length; i++)
        {
            float offsetX = rng.Next(-100000, 100000);
            float offsetY = rng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

        }
    }







}