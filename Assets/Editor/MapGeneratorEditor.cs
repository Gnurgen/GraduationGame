using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

    override public void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;

        DrawDefaultInspector();

        map.mapLevel = EditorGUILayout.IntSlider("Map Level", map.mapLevel, 1, RoomBuilder.MAX_LEVEL);

        if (GUILayout.Button("Reroll"))
        {
            map.Build();
        }
    }
}
