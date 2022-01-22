using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformPointDistribution : MonoBehaviour
{
    public List<GameObject> rocks;
    public List<Vector3> bannedPositions;
    [SerializeField]
    private Terrain world;
    public int count;
    private int radius;
    

    public Vector3 RandomTerrainPosition(Terrain terrain)
    {
        Vector3 result;
        //Get the terrain size
        Vector3 temp = terrain.terrainData.size;
        Vector3 terrainSize = new Vector3(temp.x, temp.y, temp.z) / 2f;

        //Choose a uniformly random x and z to sample y
        // MODIFIED - take points from the center, +- the radius 
        //float rX = Random.Range(terrainSize.x - radius, terrainSize.x + radius);
        //float rZ = Random.Range(terrainSize.z - radius, terrainSize.z + radius);
        // this would work, but would produce results in a square, not a circle.

        float rX = Random.Range(0 - radius, radius);
        float rZ = Random.Range(0 - radius, radius);

        Vector3 sample = new Vector3(rX, 0, rZ);
        sample.y = terrain.SampleHeight(sample);
        result = sample;
        Debug.Log(result);
        // If the terrains height is 0, then it must be in the circle - 
        // in theory this will cut the corners out, recursing should the position be in said corners
        if (sample.y != 0)
        {
            result = RandomTerrainPosition(world);
        }
        return result;
    }
   
    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<Terrain>();
        radius = world.gameObject.GetComponent<CaveGeneration>().radius - 5;
        for(int i = 0; i < count; i++)
        {
            int index = Random.Range(0, rocks.Count);
            Instantiate(rocks[index], RandomTerrainPosition(world), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
