using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictBuilder : MonoBehaviour
{
    public List<GameObject> generatorPositions;
    public GameObject buildingPrefab;
    public int density;
    public LineRenderer line;
    protected CityBuilder cb;


    public void createBuilding(GameObject prefab)
    {
        // Grab a random value for now, chuck in proc gen later
        int colour = Random.Range(0, 3);
        
        // Chuck in the meshfilter
        MeshFilter mesh = prefab.AddComponent<MeshFilter>();
        mesh.mesh = cb.meshFilters[colour].sharedMesh;

        // Chuck in a meshRenderer (materials)
        MeshRenderer meshR = prefab.AddComponent<MeshRenderer>();
        meshR.material = cb.meshRenderers[colour].sharedMaterial;

        // Note down the buildings colour, obv
        // this needs replaced with non-random system
        BuildingScript script = prefab.GetComponent<BuildingScript>();
        script.colour = colour;
    }
    
    // Start is called before the first frame update   
    void Start()
    {
        cb = GameObject.FindGameObjectWithTag("GameController").GetComponent<CityBuilder>();
        line = GetComponent<LineRenderer>();
        // Run through every X points in the circle,
        // whhere X is density.
        for (int i = 1; i <= line.positionCount - 2; i += density)
        {
            
            // Get the position of the point in the circle
            Vector3 linePoint = line.GetPosition(i);
            bool spawnable = true;
            foreach (GameObject g in generatorPositions)
            {
                Vector3 bannedPos = g.transform.position;
                // Compare the distance between linePoint
                // and the other generators
                if(!((bannedPos - (linePoint+this.transform.position)).sqrMagnitude >= Mathf.Round(Mathf.Pow(line.GetPosition(0).z, 2))))
                {
                    // if the building position is closer to the 
                    // generator than we'd like, then flag it.
                    spawnable = false;                      
                }
    
            }
            if (spawnable)
            {
                //make a building
                GameObject building = Instantiate(buildingPrefab, this.transform, true);
                createBuilding(building);
                // Create a temp Vec3 containing the position
                // on the ring to place, then increase
                // the y coordinate based on the height of 
                // the building to ensure it's secured to the floor.
                //linePoint.y += building.transform.localScale.y / 2;
                linePoint.y = 0f;
                building.transform.position = linePoint + this.transform.position;
                //Debug.Log("Building at position " + building.transform.position + " is " + Vector3.Distance(bannedPos, this.transform.position + linePoint) + " far away.");
                // Ensure the building is rotated to be facing the generator
                Quaternion targetRot = Quaternion.LookRotation(this.transform.position - building.transform.position);
                building.transform.rotation = targetRot;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
