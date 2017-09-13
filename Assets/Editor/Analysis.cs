using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(Analysis))]
public class IceButton : Editor
{

    private Analysis _is;

    void OnEnable()
    {

        _is = (Analysis)target;

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TextAsset XMLFile = _is.XMLFile;
        Analysis.playerInfo deserializedinfo = XMLHelper.Deserialize<Analysis.playerInfo>(Path.GetDirectoryName(AssetDatabase.GetAssetPath(XMLFile)), XMLFile.name);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Age: " + deserializedinfo.Age);
        EditorGUILayout.LabelField("PlayerType: " + deserializedinfo.PlayerType);
        EditorGUILayout.LabelField("Gender: " + deserializedinfo.Gender);
        EditorGUILayout.LabelField("Shots fired: " + deserializedinfo.ShotsFired);
        EditorGUILayout.LabelField("Shots hit: " + deserializedinfo.ShotsHit);
        EditorGUILayout.LabelField("Damage Taken: " + deserializedinfo.DamageTaken);
        EditorGUILayout.LabelField("Seconds Survived: " + deserializedinfo.SurvivedTime);

        if (GUILayout.Button("Display path"))
        {

            _is.GeneratePath();
        }
        if (GUILayout.Button("Remove path"))
        {

            _is.RemovePath();
        }
    }
}