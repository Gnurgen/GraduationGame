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
        RoomBuilder workbench = FindObjectOfType<RoomBuilder>() as RoomBuilder;
        if (workbench == null)
        {
            GameObject obj = new GameObject("Workbench");
            workbench = obj.AddComponent<RoomBuilder>();
            workbench.transform.position = new Vector3(0.5f, 0.0f, 0.5f);
            Undo.RegisterCreatedObjectUndo(obj, "Created room builder workbench");
            Selection.objects = new Object[] { obj };
        }
        else
            Selection.objects = new Object[] { workbench.gameObject };
    }

    [MenuItem("Tools/Room Builder/Create Object List/Enemies", false, 1)]
    public static void CeateEnemyBuildingBlocks()
    {
        createBuildingBlocks("Enemy");
    }

    [MenuItem("Tools/Room Builder/Create Object List/Destructibles", false, 2)]
    public static void CreateDestructibleBuildingBlocks()
    {
        createBuildingBlocks("Destructible");
    }

    [MenuItem("Tools/Room Builder/Create Object List/Decorations", false, 3)]
    public static void CreateDecorationBuildingBlocks()
    {
        createBuildingBlocks("Decoration");
    }

    [MenuItem("Tools/Room Builder/Select Workbench %&w", false, 20)]
    public static void SelectWorkbench()
    {
        RoomBuilder workbench = FindObjectOfType<RoomBuilder>() as RoomBuilder;
        if (workbench != null)
            Selection.objects = new Object[] { workbench.gameObject };
    }

    [MenuItem("Tools/Room Builder/Add Selection %&e", false, 21)]
    public static void AddToRoom()
    {
        RoomBuilder workbench = FindObjectOfType<RoomBuilder>() as RoomBuilder;
        if (workbench != null)
        {
            Object[] objects = Selection.objects;
            for (int i = 0; i < objects.Length; i++)
            {
                if (PrefabUtility.GetPrefabType(objects[i]) == PrefabType.PrefabInstance)
                    workbench.AddRoomObject(objects[i] as GameObject);
            }
        }
    }

    [MenuItem("Tools/Room Builder/Update References", false, 22)]
    public static void UpdateRoomReferences()
    {
        List<GameObject> objectList = Resources.LoadAll("Room").Cast<GameObject>().ToList();
        // For each [decoration|enemy|destructible] 
        // if reference == prefab
        // replace object with prefab
        // end for
        // overwrite room
    }

    override public void OnInspectorGUI()
    {
        RoomBuilder workbench = target as RoomBuilder;

        string roomName = EditorGUILayout.TextField("Room Name: ", workbench.roomName).Trim();
        if (workbench.roomName != roomName)
        {
            Undo.RecordObject(workbench, "Changed room name");
            workbench.roomName = roomName != null && roomName.Length > 0 ? roomName : "Room";
        }

        GUILayout.Space(10);
        GUILayout.Label("Room Size");
        Vector2 roomSize = workbench.roomSize;
        roomSize.x = EditorGUILayout.IntSlider("   Width", (int)workbench.roomSize.x, 1, workbench.roomUnits.GetLength(0));
        roomSize.y = EditorGUILayout.IntSlider("   Depth", (int)workbench.roomSize.y, 1, workbench.roomUnits.GetLength(1));

        if (workbench.roomSize != roomSize)
        {
            Undo.RecordObject(workbench, "Changed room size");
            workbench.roomSize = roomSize;
        }

        /*
        EditorGUILayout.LabelField("Room Objects:",workbench.GetObjectsNum().ToString());
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
        if (GUILayout.Button("Select all objects in room"))
        {
            Selection.objects = workbench.GetObjects();
        }

        */

        /*
        GUILayout.Space(10);
        if (GUILayout.Button("Create Prefab"))
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
        */
    }

    private static void createBuildingBlocks(string tag)
    {
        ;

        Bounds bounds;
        GameObject prefab;
        GameObject parent = null;
        List<GameObject> objectList = Resources.LoadAll(tag).Cast<GameObject>().ToList();
        float offset = 0;

        for (int i = 0; i < objectList.Count; i++)
        {
            if (parent == null)
            {
                parent = new GameObject(tag + " Building Blocks");
                Undo.RegisterCreatedObjectUndo(parent, "Created parent for " + tag + " building blocks");
                Selection.objects = new Object[] { parent };
            }

            prefab = PrefabUtility.InstantiatePrefab(objectList[i]) as GameObject;
            Undo.RegisterCreatedObjectUndo(prefab, "Created " + tag + " building blocks");
            bounds = prefab.GetComponentInChildren<MeshFilter>().sharedMesh.bounds;

            prefab.transform.position = new Vector3(offset, bounds.extents.y, 0);
            offset += bounds.extents.x + 1;
            prefab.transform.parent = parent.transform;
//            BuildingBlock bb = prefab.AddComponent<BuildingBlock>();
//            bb.prefab = objectList[i];
//            PrefabUtility.DisconnectPrefabInstance(prefab);
        }
    }

}
