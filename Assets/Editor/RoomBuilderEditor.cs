using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(RoomBuilder))]
public class RoomEditor : Editor {

    [MenuItem("Tools/Room Builder/Create Workbench", false, 0)]
    public static void CreateWorkbench()
    {
        RoomBuilder workbench = new GameObject("Workbench").AddComponent<RoomBuilder>();
        Undo.RegisterCreatedObjectUndo(workbench, "Created room builder workbench");
        GameObject[] selection = { workbench.gameObject };
        Selection.objects = selection;
    }

    [MenuItem("Tools/Room Builder/Create Building Blocks/Enemies", false, 1)]
    public static void CeateEnemyBuildingBlocks()
    {
        createBuildingBlocks("Enemy");
    }

    [MenuItem("Tools/Room Builder/Create Building Blocks/Destructibles", false, 2)]
    public static void CreateDestructibleBuildingBlocks()
    {
        createBuildingBlocks("Destructible");
    }

    [MenuItem("Tools/Room Builder/Create Building Blocks/Decorations", false, 3)]
    public static void CreateDecorationBuildingBlocks()
    {
        createBuildingBlocks("Decoration");
    }

    override public void OnInspectorGUI()
    {
        RoomBuilder workbench = target as RoomBuilder;

        string roomName = EditorGUILayout.TextField("Room Name: ", workbench.roomName).Trim();
        workbench.roomName = roomName != null && roomName.Length > 0 ? roomName : "Room";

        GUILayout.Space(10);
        GUILayout.Label("Room Size");
        workbench.roomSize.x = EditorGUILayout.IntSlider("   Width", (int)workbench.roomSize.x, 1, workbench.roomUnits.GetLength(0));
        workbench.roomSize.y = EditorGUILayout.IntSlider("   Depth", (int)workbench.roomSize.y, 1, workbench.roomUnits.GetLength(1));

        GUILayout.Space(10);
        int index = EditorGUILayout.IntSlider("Room List", workbench.roomIndex, 0, RoomBuilder.list.Count);

        if (index != workbench.roomIndex) {
            if (workbench.roomModified)
            {
                switch(EditorUtility.DisplayDialogComplex(
                    "Room Was Modified",
                    "Do you want to save changes you made in: " + workbench.roomName + "?\n\nYour changes will be lost if you don't save them.",
                    "Save", "Don't Save", "Cancel"
                ))
                {
                    case 0:
                    case 1:
                        workbench.roomIndex = index;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                workbench.roomIndex = index;
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Commit"))
        {

        }
        else if (GUILayout.Button("Duplicate"))
        {

        }
        else if (GUILayout.Button("Remove") && EditorUtility.DisplayDialog(
            "Removing Room",
            "Are you sure you want to remove: " + workbench.roomName + "?\n\nThis CANNOT be undone.",
            "Remove", "Cancel"
        ))
        {
            
        }
    }

    private static void createBuildingBlocks(string tag)
    {
        Bounds bounds;
        GameObject prefab;
        GameObject parent = null;
        List<GameObject> objectList = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(c => c.gameObject.tag == tag).ToList();
        float offset = 0;

        for (int i = 0; i < objectList.Count; i++)
        {
            if (PrefabUtility.GetPrefabParent(objectList[i]) == null)
            {
                if (parent == null)
                {
                    parent = new GameObject(tag + " Building Blocks");
                    Undo.RegisterCreatedObjectUndo(parent, "Created parent for " + tag + " building blocks");
                    GameObject[] selection = { parent };
                    Selection.objects = selection;
                }

                prefab = PrefabUtility.InstantiatePrefab(objectList[i]) as GameObject;
                Undo.RegisterCreatedObjectUndo(prefab, "Created " + tag + " building blocks");
                bounds = prefab.GetComponent<MeshFilter>().sharedMesh.bounds;

                prefab.transform.position = new Vector3(offset, bounds.extents.y, 0);
                offset += bounds.extents.x + 1;
                prefab.transform.parent = parent.transform;
                BuildingBlock bb = prefab.AddComponent<BuildingBlock>();
                bb.prefab = objectList[i];
                PrefabUtility.DisconnectPrefabInstance(prefab);
            }
        }
    }
}
