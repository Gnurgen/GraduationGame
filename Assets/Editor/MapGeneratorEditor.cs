using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

    override public void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;

        DrawDefaultInspector();

        if (GUILayout.Button("Reroll"))
        {
            map.Build();
        }
    }
}
