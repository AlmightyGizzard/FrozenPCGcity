using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CaveGeneration))]
public class CaveGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CaveGeneration genScript = (CaveGeneration)target;
        if (GUILayout.Button("Generate")) 
        {
            genScript.generate();
        }
        
    }
}
