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

    [MenuItem("Tools/Room Builder/Create Object List/Obstacles", false, 2)]
    public static void CreateDestructibleBuildingBlocks()
    {
        createBuildingBlocks("Obstacles");
    }

    [MenuItem("Tools/Room Builder/Create Object List/Doodads", false, 3)]
    public static void CreateDecorationBuildingBlocks()
    {
        createBuildingBlocks("Doodads");
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
        GameObject go;
        if (workbench != null)
        {
            Object[] objects = Selection.objects;
            for (int i = 0; i < objects.Length; i++)
            {
                go = objects[i] as GameObject;
                if (PrefabUtility.GetPrefabType(objects[i]) == PrefabType.PrefabInstance &&
                    go.GetComponentInParent<RoomBuilder>() == null
                )
                {
                    Undo.RecordObject(go.transform, "Adding object to room");
                    go.transform.parent = workbench.transform;
                }
            }
        }
    }

    private static void replace(GameObject original, GameObject replacement)
    {
        GameObject obj = Instantiate(replacement) as GameObject;
        obj.name = original.name;
        Undo.RegisterCreatedObjectUndo(obj, "Created replacement object");
        obj.transform.position = original.transform.position;
        obj.transform.rotation = original.transform.rotation;
        obj.transform.localScale = original.transform.localScale;

        obj.transform.parent = original.transform.parent;
        obj.transform.SetSiblingIndex(original.transform.GetSiblingIndex());
        Undo.DestroyObjectImmediate(original);
    }

    override public void OnInspectorGUI()
    {
        RoomBuilder workbench = target as RoomBuilder;

        Vector2 roomSize = workbench.roomSize;
        roomSize.x = EditorGUILayout.IntSlider("Room Size", (int)workbench.roomSize.x, 1, workbench.roomUnits.GetLength(0));
        roomSize.y = roomSize.x;

        workbench.roomLevel = EditorGUILayout.IntSlider("Room Level", workbench.roomLevel, 1, RoomBuilder.MAX_LEVEL);

        GUILayout.Space(10);
        GUI.enabled = roomSize.x == 1 && roomSize.y == 1;
        bool isBeaconRoom = EditorGUILayout.Toggle("Start/End Room", workbench.isBeaconRoom);
        workbench.isBeaconRoom = GUI.enabled && isBeaconRoom;

        workbench.isRotatable = EditorGUILayout.Toggle("Is Rotatable", workbench.isRotatable);
        GUI.enabled = true;

        if (workbench.roomSize != roomSize)
        {
            Undo.RecordObject(workbench, "Changed room size");
            workbench.roomSize = roomSize;
        }
    }

    private static void createBuildingBlocks(string tag)
    {
        GameObject prefab;
        List<GameObject> objectList = Resources.LoadAll(tag).Cast<GameObject>().ToList();
        Object[] selection = new Object[objectList.Count];

        for (int i = 0; i < objectList.Count; i++)
        {
            prefab = PrefabUtility.InstantiatePrefab(objectList[i]) as GameObject;
            Undo.RegisterCreatedObjectUndo(prefab, "Prefab created");
            prefab.transform.position = new Vector3((i % 5) * 10, 0, Mathf.Floor(i/5) * 10);
            selection[i] = prefab;
        }

        Selection.objects = selection;
    }

}
