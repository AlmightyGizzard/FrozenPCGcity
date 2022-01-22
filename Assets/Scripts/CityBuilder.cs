using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CityBuilder : MonoBehaviour
{
    public int noOfModels = 2;
    public GameObject ringPrefab;
    // Just a cube for now, but will ad in scripts to
    // procedurally choose from an array of asset,=s.
    public List<GameObject> buildingModels;
    [SerializeField]
    public MeshFilter[] meshFilters;
    [SerializeField]
    public MeshRenderer[] meshRenderers;
    public GameObject buildingPrefab;
    public GameObject generatorPrefab;
    public GameObject lampPrefab;
    public List<GameObject> generators;
    public List<GameObject> rings;
    

    // Starting density of the ring = the Z value
    // of the initial draw point
    public float ringDensity = 1;
    [Range(0, 360)]
    public int buildingDensity = 180;
    public int maxGenerators = 1;
    public int generatorOffset = 8;
    public int ringsPerGenerator;
    public int lampDensity;
    [SerializeField]
    private int numGenerators = 0;
    [SerializeField]
    private Vector3 generatorPosition = new Vector3(0, 0, 0);

    private CaveGeneration terrain;

    public GameObject uiPanel;
    public TextMeshProUGUI text;
    public Camera cam;
    public int timer = 10;

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
   
    Vector3 FindNewPosition()
    {
        Vector3 result = new Vector3(0, 0, 0);
        int startPos = Random.Range(0, generators.Count);
        result = generators[startPos].transform.position;
        int temp = Random.Range(0, 11);
        if (temp < 3)
        {
            result.x += generatorOffset;
            result.z += generatorOffset;
        }
        else if (temp >= 3 && temp < 6)
        {
            result.x -= generatorOffset;
            result.z += generatorOffset;
        }
        else if (temp >= 6 && temp < 9)
        {
            result.x += generatorOffset;
            result.z -= generatorOffset;
        }
        else
        {
            result.x -= generatorOffset;
            result.z -= generatorOffset;
        }
        //bool allowed = true;
        foreach(GameObject g in generators)
        {
            if(result == g.transform.position)
            {
                Debug.LogWarning("Recursing!, result = "+result);
                result = FindNewPosition();
            }
        }
        return result;
    }

    void placeGenerator(int numRings = 1)
    {
        int ld = lampDensity;
        GameObject newGen;
        if (numGenerators == 0)
        {
            newGen = Instantiate(generatorPrefab, generatorPosition, Quaternion.identity);
        }
        else
        {
            generatorPosition = FindNewPosition();
            newGen = Instantiate(generatorPrefab, generatorPosition, Quaternion.identity);
        }
        generators.Add(newGen);
        numGenerators++;

        for(int i = 1; i <= numRings; i++)
        {
            placeRing(newGen, i*ringDensity, ld);
            ld = ld / 2;
        }
    }

    void placeRing(GameObject generator, float radius, int lampDensity)
    {
        // Create a ring object under the generator,
        // then draw a circle w lineRenderer
        GameObject ring = Instantiate(ringPrefab, generator.transform.position, Quaternion.identity);
        ring.DrawCircle(radius, 0.02f);


        //Create a district out of the ring by placing 
        // buildings in a circle around it
        DistrictBuilder db = ring.AddComponent<DistrictBuilder>();
        // TODO - make this dynamic so I can modify it at runtime
        // in the editor + make density increase, not decrease
        db.density = buildingDensity;
        db.buildingPrefab = buildingPrefab;
        db.lampPrefab = lampPrefab;
        db.lampDistribution = lampDensity;
        db.generatorPositions = generators;
        rings.Add(ring);
    }

    public void BuildCity()
    {
        terrain.generate();
        int numRings = ringsPerGenerator;
        // Place a single generator, then flip a coin - on heads,
        // generate another generator - so long as we haven't
        // hit max.
        placeGenerator(numRings);
        while (CoinFlip() && numGenerators < maxGenerators)
        {
            if (numRings > 1) { numRings--; }
            placeGenerator(numRings);
        }
    }

    public void WipeCity()
    {
        generatorPosition = new Vector3(0, 0, 0);
        numGenerators = 0;
        foreach (GameObject g in generators)
        {
            Destroy(g);
        }
        foreach(GameObject r in rings)
        {
            Destroy(r);
        }
        generators.Clear(); rings.Clear();
    }

    private void Awake()
    {
        text = GameObject.Find("Text").GetComponentInChildren<TextMeshProUGUI>();
        cam = FindObjectOfType<Camera>();
        meshFilters = new MeshFilter[noOfModels];
        meshRenderers = new MeshRenderer[noOfModels];
        terrain = FindObjectOfType<CaveGeneration>();
        for (int index = 0; index < buildingModels.Count; index++)
        {
            //Debug.Log(buildingModels[index].GetComponent<MeshFilter>());
            meshFilters[index] = buildingModels[index].GetComponent<MeshFilter>();
            meshRenderers[index] = buildingModels[index].GetComponent<MeshRenderer>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        BuildCity();
    }

    // Update is called once per frame
    void Update()
    {
        timer--;
        if(timer < 0)
        {
            uiPanel.SetActive(false);
        }

        
        if (Input.GetKeyDown("space"))
        {
            WipeCity();
            BuildCity();
        }
    }
}
