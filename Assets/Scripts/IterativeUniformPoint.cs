using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IterativeUniformPoint : MonoBehaviour
{
    public List<GameObject> rocks;
    public List<Vector3> bannedPositions;
    public List<float> bannedRadiuses;
    public Terrain world;
    // Number of points
    public int count;
    // Number of rocks per point
    public int secondCount;
    // Radius of the points
    public int clusterRadius;
    private int cityRadius;
    [SerializeField]
    private List<Vector3> points;
    

    public Vector3 RandomPointInRadius(Terrain terrain, Vector3 refPoint, float radius)
    {
        Vector3 result;
        Vector3 sample = refPoint + Random.insideUnitSphere * radius;
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

        float rX = Random.Range(0 - cityRadius, cityRadius);
        float rZ = Random.Range(0 - cityRadius, cityRadius);

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
        for(int i = 0; i < bannedPositions.Count; i++)
        {
            float dist = Vector3.Distance(sample, bannedPositions[i]);
            if(dist < bannedRadiuses[i])
            {
                result = RandomTerrainPosition(world);
            }
        }
        return result;
    }

    public void generate()
    {
        world = FindObjectOfType<Terrain>();
        cityRadius = world.gameObject.GetComponent<CaveGeneration>().radius - 5;
        for (int i = 0; i < count; i++)
        {
            points.Add(RandomTerrainPosition(world));
        }
        foreach (Vector3 point in points)
        {
            for (int i = 0; i < secondCount; i++)
            {
                int index = Random.Range(0, rocks.Count);
                Instantiate(rocks[index], RandomPointInRadius(world, point, clusterRadius), Quaternion.identity);
            }
        }
    }
    public void wipe()
    {
        points.Clear();
        bannedPositions.Clear();
        bannedRadiuses.Clear();
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Nature"))
        {
            Destroy(g);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //generate();
    }
}
