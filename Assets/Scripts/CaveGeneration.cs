using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

public class CaveGeneration : MonoBehaviour
{
    // frequency of the noise
    public float frequency = 1f;

    // lacunarity of the noise
    public float lacu = 1f;

    // number of octaves
    public int octaves = 1;

    // persistence of the noise
    public float persist = 1f;

    //the threshold to cut-off
    public float threshold = 0.5f;

    // falloff amount
    public float falloff = 0.25f;

    //radius of the unaffected area in the centre
    public int radius = 10;

    TerrainData terrainData;

    private float[,] getCircle(float[,] data, int xCentre, int yCentre, int radius)
    {
        int xSym;
        int ySym;
        for(int x = xCentre - radius; x <= xCentre; x++)
        {
            for(int y = yCentre - radius; y <= yCentre; y++)
            {
                if((x - xCentre)*(x - xCentre) + (y - yCentre) * (y - yCentre) <= radius * radius)
                {
                    xSym = xCentre - (x - xCentre);
                    ySym = yCentre - (y - yCentre);
                    data[x, y] = 0f;
                    data[x, ySym] = 0f;
                    data[xSym, y] = 0f;
                    data[xSym, ySym] = 0f;
                }
            }
        }

        
        return data;
    }

    public void generate()
    {
        terrainData = Terrain.activeTerrain.terrainData;
        //A new ridged multifractal generator
        var generator = new RidgedMultifractal(frequency, lacu, octaves, (int)(Random.value
        * 0xffffff), QualityMode.High);
        //The thresholded output -- choose either 0.0 or 1.0, based
        //on the output
        var clamped = new LibNoise.Operator.Select(new Const(0.0f), new Const(1.0f),
        generator);
        //Set the threshold and falloff rate
        clamped.SetBounds(0f, threshold);
        clamped.FallOff = falloff;
        //Create a 2D noise generator for the terrain heightmap, using the generator we just
        var noise = new Noise2D(terrainData.heightmapResolution, clamped);
        //Generate a plane from [0, 1] on x, [0, 1] on y
        noise.GeneratePlanar(0, 1, 0, 1);
        //Get the data in an array so we can use it to set the heights
        //var data = noise.GetData(true, 0, 0, true);
        var data = noise.GetNormalizedData();

        // Modify and flatten a circle of area in the centre
        data = getCircle(data, 256, 256, radius);
        //.. and actually set the heights
        terrainData.SetHeights(0, 0, data);
        
    }


}
