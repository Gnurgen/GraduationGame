using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(BuildingBlock))]
[CanEditMultipleObjects]
public class BuildingBlockEditor : Editor {

    override public void OnInspectorGUI()
    {
        BuildingBlock bb;
        GameObject prefab;
        Object[] list = Selection.objects;
        GameObject[] selection = new GameObject[list.Length];

        if (GUILayout.Button("Clone Prefab", GUILayout.Height(100)))
        {
            for (int i = 0; i < list.Length; i++)
            {
                bb = (list[i] as GameObject).GetComponent<BuildingBlock>();
                prefab = PrefabUtility.InstantiatePrefab(bb.prefab) as GameObject;
                selection[i] = prefab;

                prefab.transform.position = bb.transform.position;
                prefab.transform.localScale = bb.transform.localScale;
                prefab.transform.rotation = bb.transform.rotation;
                Undo.RegisterCreatedObjectUndo(prefab, "Created prefab from building block");
            }

            Selection.objects = selection;
        }
    }
}
