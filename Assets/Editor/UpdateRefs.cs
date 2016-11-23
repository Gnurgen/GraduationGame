using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UpdateRefs : EditorWindow{

    private static Resource resource = Resource.Default;
    private int selectionIndex = 0;

    private enum Resource{
        Default,
        Tiles,
        Walls,
        Enemies,
        Obstacles,
        Doodads
    }

    [MenuItem("Tools/Room Builder/Update Room Resources/Tiles", false, 22)]
    public static void ShowTiles()
    {
        resource = Resource.Tiles;
        EditorWindow.GetWindow(typeof(UpdateRefs));
    }

    [MenuItem("Tools/Room Builder/Update Room Resources/Walls", false, 23)]
    public static void ShowWalls()
    {
        resource = Resource.Walls;
        EditorWindow.GetWindow(typeof(UpdateRefs));
    }

    [MenuItem("Tools/Room Builder/Update Room Resources/Enemies", false, 24)]
    public static void ShowEnemies()
    {
        resource = Resource.Enemies;
        EditorWindow.GetWindow(typeof(UpdateRefs));
    }

    [MenuItem("Tools/Room Builder/Update Room Resources/Obstacles", false, 25)]
    public static void ShowObstacles()
    {
        resource = Resource.Obstacles;
        EditorWindow.GetWindow(typeof(UpdateRefs));
    }

    [MenuItem("Tools/Room Builder/Update Room Resources/Doodads", false, 26)]
    public static void ShowDoodads()
    {
        resource = Resource.Doodads;
        EditorWindow.GetWindow(typeof(UpdateRefs));
    }

    void OnGUI()
    {
        int i;
        int j;
        int l;
        int count = 0;
        string[] strList;
        Transform[] transformList;
        GameObject room;
        GameObject go;
        List<GameObject> prefabs;
        List<GameObject> rooms = Resources.LoadAll("Room").Cast<GameObject>().ToList();

        switch (resource)
        {
            case Resource.Doodads:
                prefabs = Resources.LoadAll("Doodads").Cast<GameObject>().ToList();
                strList = toStringList(prefabs);
                selectionIndex = EditorGUILayout.Popup("Doodads:", selectionIndex, strList);

                if (GUILayout.Button("Replace Doodads"))
                {
                    go = selectionIndex > 0 ? prefabs[selectionIndex - 1] : null;

                    for (i = 0; i < rooms.Count; i++)
                    {
                        room = PrefabUtility.InstantiatePrefab(rooms[i]) as GameObject;
                        transformList = room.GetComponentsInChildren<Transform>();

                        for (j = 0; j < transformList.Length; j++)
                        {
                            if (go == null)
                            {
                                for (l = 0; l < prefabs.Count; l++)
                                {
                                    if (transformList[j] != null && transformList[j].childCount > 0 && transformList[j].GetChild(0).gameObject.name == prefabs[l].transform.GetChild(0).gameObject.name)
                                    {
                                        replace(transformList[j].gameObject, prefabs[l]);
                                        count++;
                                    }
                                }
                            }
                            else
                            {
                                if (transformList[j] != null && transformList[j].childCount > 0 && transformList[j].GetChild(0).gameObject.name == go.transform.GetChild(0).gameObject.name)
                                {
                                    replace(transformList[j].gameObject, go);
                                    count++;
                                }
                            }
                        }

                        PrefabUtility.ReplacePrefab(room, rooms[i], ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
                        DestroyImmediate(room);
                    }
                    Debug.Log("Updated " + count + " doodads");
                }
                break;
            case Resource.Obstacles:
                prefabs = Resources.LoadAll("Obstacles").Cast<GameObject>().ToList();
                strList = toStringList(prefabs);
                selectionIndex = EditorGUILayout.Popup("Obstacles:", selectionIndex, strList);

                if (GUILayout.Button("Replace Obstacles"))
                {
                    go = selectionIndex > 0 ? prefabs[selectionIndex - 1] : null;

                    for (i = 0; i < rooms.Count; i++)
                    {
                        room = PrefabUtility.InstantiatePrefab(rooms[i]) as GameObject;
                        transformList = room.GetComponentsInChildren<Transform>();

                        for (j = 0; j < transformList.Length; j++)
                        {
                            if (go == null)
                            {
                                for (l = 0; l < prefabs.Count; l++)
                                {
                                    if (transformList[j] != null && transformList[j].childCount > 0 && transformList[j].GetChild(0).gameObject.name == prefabs[l].transform.GetChild(0).gameObject.name)
                                    {
                                        replace(transformList[j].gameObject, prefabs[l]);
                                        count++;
                                    }
                                }
                            }
                            else
                            {
                                if (transformList[j] != null && transformList[j].childCount > 0 && transformList[j].GetChild(0).gameObject.name == go.transform.GetChild(0).gameObject.name)
                                {
                                    replace(transformList[j].gameObject, go);
                                    count++;
                                }
                            }
                        }

                        PrefabUtility.ReplacePrefab(room, rooms[i], ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
                        DestroyImmediate(room);
                    }
                    Debug.Log("Updated " + count + " obstacles");
                }
                break;
            case Resource.Enemies:
                Object[] roomEnemies;
                prefabs = Resources.LoadAll("Enemy").Cast<GameObject>().ToList();
                strList = toStringList(prefabs);
                selectionIndex = EditorGUILayout.Popup("Enemy:", selectionIndex, strList);

                if (GUILayout.Button("Replace Enemies"))
                {
                    Object melee = null;
                    Object ranged = null;

                    go = selectionIndex > 0 ? prefabs[selectionIndex - 1] : null;

                    if (go)
                    {
                        if (go.GetComponent<MeleeAI>() != null)
                            melee = go;
                        else if (go.GetComponent<RangedAI>() != null)
                            ranged = go;
                    }
                    else
                    {
                        for (i = 0; i < prefabs.Count; i++)
                        {
                            if (melee == null && prefabs[i].GetComponent<MeleeAI>() != null)
                                melee = go;
                            else if (ranged == null && prefabs[i].GetComponent<RangedAI>() != null)
                                ranged = go;
                        }
                    }

                    for (i = 0; i < rooms.Count; i++)
                    {
                        room = PrefabUtility.InstantiatePrefab(rooms[i]) as GameObject;

                        if (melee != null)
                        {
                            roomEnemies = room.GetComponentsInChildren<MeleeAI>();
                            for (j = 0; j < roomEnemies.Length; j++)
                            {
                                replace((roomEnemies[j] as MeleeAI).gameObject, melee as GameObject);
                                count++;
                            }
                        }

                        if (ranged != null)
                        {
                            roomEnemies = room.GetComponentsInChildren<EnemyRangedAttack>();
                            for (j = 0; j < roomEnemies.Length; j++)
                            {
                                replace((roomEnemies[j] as EnemyRangedAttack).gameObject, ranged as GameObject);
                                count++;
                            }
                        }

                        PrefabUtility.ReplacePrefab(room, rooms[i], ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
                        DestroyImmediate(room);
                    }

                    Debug.Log("Updated " + count + " enemies");
                }
                break;
            case Resource.Walls:
                RoomWall[] roomWalls;
                prefabs = Resources.LoadAll("Wall").Cast<GameObject>().ToList();
                strList = toStringList(prefabs);
                selectionIndex = EditorGUILayout.Popup("Wall:", selectionIndex, strList);

                if (GUILayout.Button("Replace Walls"))
                {
                    go = selectionIndex > 0 ? prefabs[selectionIndex - 1] : null;

                    for (i = 0; i < rooms.Count; i++)
                    {
                        room = PrefabUtility.InstantiatePrefab(rooms[i]) as GameObject;
                        roomWalls = room.GetComponentsInChildren<RoomWall>();

                        for (j = 0; j < roomWalls.Length; j++)
                        {
                            if (go == null)
                            {
                                for (l = 0; l < prefabs.Count; l++)
                                {
                                    if (roomWalls[j].referenceName == prefabs[l].GetComponent<RoomWall>().referenceName)
                                    {
                                        replace(roomWalls[j].gameObject, prefabs[l]);
                                        count++;
                                    }
                                }
                            }
                            else
                            {
                                if (roomWalls[j].referenceName == go.GetComponent<RoomWall>().referenceName)
                                {
                                    replace(roomWalls[j].gameObject, go);
                                    count++;
                                }
                            }
                        }

                        PrefabUtility.ReplacePrefab(room, rooms[i], ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
                        DestroyImmediate(room);
                    }

                    Debug.Log("Updated " + count + " walls");
                }
                break;
            case Resource.Tiles:
            default:
                RoomTile[] roomTiles;
                prefabs = Resources.LoadAll("Tile").Cast<GameObject>().ToList();
                strList = toStringList(prefabs);
                selectionIndex = EditorGUILayout.Popup("Tile:", selectionIndex, strList);

                if (GUILayout.Button("Replace Tiles"))
                {
                    go = selectionIndex > 0 ? prefabs[selectionIndex - 1] : null;

                    for (i = 0; i < rooms.Count; i++)
                    {
                        room = PrefabUtility.InstantiatePrefab(rooms[i]) as GameObject;
                        roomTiles = room.GetComponentsInChildren<RoomTile>();

                        for (j = 0; j < roomTiles.Length; j++)
                        {
                            if (go == null)
                            {
                                for (l = 0; l < prefabs.Count; l++)
                                {
                                    if (roomTiles[j].referenceName == prefabs[l].GetComponent<RoomTile>().referenceName)
                                    {
                                        replace(roomTiles[j].gameObject, prefabs[l]);
                                        count++;
                                    }
                                }
                            }
                            else
                            {
                                if (roomTiles[j].referenceName == go.GetComponent<RoomTile>().referenceName)
                                {
                                    replace(roomTiles[j].gameObject, go);
                                    count++;
                                }
                            }
                        }

                        PrefabUtility.ReplacePrefab(room, rooms[i], ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
                        DestroyImmediate(room);
                    }

                    Debug.Log("Updated " + count + " tiles");
                }
                break;
        }
    }

    private string[] toStringList(List<GameObject> prefabs)
    {
        string[] strList = new string[prefabs.Count + 1];
        strList[0] = "[All]";

        for (int i = 0; i < prefabs.Count; i++)
            strList[i + 1] = (prefabs[i] as GameObject).name;
        return strList;
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

        if (original.transform.childCount > 0 && !original.transform.GetChild(0).gameObject.activeSelf)
            obj.transform.GetChild(0).gameObject.SetActive(false);

        Undo.DestroyObjectImmediate(original);
    }
}
