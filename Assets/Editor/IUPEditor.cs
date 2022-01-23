using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IterativeUniformPoint))]
public class IUPEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        IterativeUniformPoint genScript = (IterativeUniformPoint)target;
        if (GUILayout.Button("Generate"))
        {
            genScript.wipe();
            genScript.generate();
        }

    }
}