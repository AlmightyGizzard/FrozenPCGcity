using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistrictBuilder : MonoBehaviour
{
    public List<GameObject> generatorPositions;
    public GameObject buildingPrefab;
    public int density;
    public LineRenderer line;
    // Start is called before the first frame update

    
    void Start()
    { 
        line = GetComponent<LineRenderer>();
        // Run through every X points in the circle,
        // whhere X is density.
        for (int i = 1; i <= line.positionCount - 2; i += density)
        {
            Vector3 linePoint = line.GetPosition(i);
            foreach(GameObject g in generatorPositions)
            {
                Vector3 bannedPos = g.transform.position;
                // Compare the distance between 
                if((linePoint - bannedPos).sqrMagnitude >= line.GetPosition(0).z)
                {
                    GameObject building = Instantiate(buildingPrefab, this.transform, true);
                    // Create a temp Vec3 containing the position
                    // on the ring to place, then increase
                    // the y coordinate based on the height of 
                    // the building to ensure it's secured to the floor.

                    linePoint.y += building.transform.localScale.y / 2;
                    building.transform.position = linePoint+this.transform.position;

                    Quaternion targetRot = Quaternion.LookRotation(this.transform.position - building.transform.position);
                    building.transform.rotation = targetRot;
                    break;
                }
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
