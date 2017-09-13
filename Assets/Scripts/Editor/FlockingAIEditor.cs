using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlockingAI))]
public class FlockingAIEditor : Editor {

    public override void OnInspectorGUI() {

        FlockingAI ai = (FlockingAI)target;

        DrawDefaultInspector();

        if (ai.debugMode) {
            EditorGUILayout.Vector3Field("Velocity", ai.Velocity);
            EditorGUILayout.HelpBox("White = velocity, Green = Allignment, Blue = Cohesion, Red = Seperation", MessageType.Info);
        }
    }
}
