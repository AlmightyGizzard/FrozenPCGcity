using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGeneration))]
public class TerrainGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TerrainGeneration terScript = (TerrainGeneration)target;
        if (GUILayout.Button("Generate"))
        {
            terScript.generate();
        }

    }
}