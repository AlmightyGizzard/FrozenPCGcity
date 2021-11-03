using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBuilder : MonoBehaviour
{
    public GameObject ringPrefab;
    // Just a cube for now, but will ad in scripts to
    // procedurally choose from an array of asset,=s.
    public GameObject buildingPrefab;
    public GameObject generatorPrefab;
    public List<GameObject> generators;

    // Starting density of the ring = the Z value
    // of the initial draw point
    public float ringDensity = 1;
    [Range(0, 360)]
    public int buildingDensity = 180;
    public int maxGenerators = 1;
    public int generatorOffset = 8;
    private int numGenerators = 0;
    private Vector3 generatorPosition = new Vector3(0, 0, 0);

    bool CoinFlip()
    {
        int temp = Random.Range(0, 2);
        if(temp == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    void placeGenerator()
    {
        GameObject newGen = Instantiate(generatorPrefab, generatorPosition, Quaternion.identity);
        int temp = Random.Range(0, 11);
        if(temp < 3)
        {
            generatorPosition.x += generatorOffset;
            generatorPosition.z += generatorOffset;
        }
        else if(temp >= 3 && temp < 6)
        {
            generatorPosition.x -= generatorOffset;
            generatorPosition.z += generatorOffset;
        }
        else if (temp >= 6 && temp < 9)
        {
            generatorPosition.x += generatorOffset;
            generatorPosition.z -= generatorOffset;
        }
        else
        {
            generatorPosition.x -= generatorOffset;
            generatorPosition.z -= generatorOffset;
        }
        generators.Add(newGen);
        numGenerators++;
    }

    void placeRing(GameObject generator)
    {
        // Create a ring object under the generator,
        // then draw a circle w lineRenderer
        GameObject ring = Instantiate(ringPrefab, generator.transform.position, Quaternion.identity);
        ring.DrawCircle(ringDensity, 0.02f);

        //Create a district out of the ring by placing 
        // buildings in a circle around it
        DistrictBuilder db = ring.AddComponent<DistrictBuilder>();
        // TODO - make this dynamic so I can modify it at runtime
        // in the editor + make density increase, not decrease
        db.density = buildingDensity;
        db.buildingPrefab = buildingPrefab;
        db.generatorPositions = generators;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Place a single generator, then flip a coin - on heads,
        // generate another generator - so long as we haven't
        // hit max.
        placeGenerator();
        while (CoinFlip() && numGenerators < maxGenerators)
        {
            placeGenerator();
        }

        foreach(GameObject generator in generators)
        {
            placeRing(generator);
        }
        // TODO - sort this so that it's tied to List<generators>
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
