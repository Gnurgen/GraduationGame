using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UpdateRefs : EditorWindow{

    private static Resource resource = Resource.None;

    private enum Resource{
        None,
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
        /*
        int i;
        int j;
        int l;
        int count = 0;
        RoomWall[] roomWalls;
        GameObject room;
        List<GameObject> rooms = Resources.LoadAll("Room").Cast<GameObject>().ToList();
        List<GameObject> walls = Resources.LoadAll("Wall").Cast<GameObject>().ToList();

        for (i = 0; i < rooms.Count; i++)
        {
            room = PrefabUtility.InstantiatePrefab(rooms[i]) as GameObject;
            roomWalls = room.GetComponentsInChildren<RoomWall>();

            for (j = 0; j < roomWalls.Length; j++)
            {
                for (l = 0; l < walls.Count; l++)
                {
                    if (roomWalls[j].referenceName == walls[l].GetComponent<RoomWall>().referenceName)
                    {
                        replace(roomWalls[j].gameObject, walls[l]);
                        count++;
                    }
                }
            }
            PrefabUtility.ReplacePrefab(room, rooms[i], ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
            DestroyImmediate(room);
        }

        Debug.Log("Updated " + count + " objects");
        */
    }

    public static void UpdateTileReferences()
    {
        int i;
        int j;
        int l;
        int count = 0;
        RoomTile[] roomTiles;
        GameObject room;
        List<GameObject> rooms = Resources.LoadAll("Room").Cast<GameObject>().ToList();
        List<GameObject> tiles = Resources.LoadAll("Tile").Cast<GameObject>().ToList();

        for (i = 0; i < rooms.Count; i++)
        {
            room = PrefabUtility.InstantiatePrefab(rooms[i]) as GameObject;
            roomTiles = room.GetComponentsInChildren<RoomTile>();

            for (j = 0; j < roomTiles.Length; j++)
            {
                for (l = 0; l < tiles.Count; l++)
                {
                    if (roomTiles[j].referenceName == tiles[l].GetComponent<RoomTile>().referenceName)
                    {
                        replace(roomTiles[j].gameObject, tiles[l]);
                        count++;
                    }
                }
            }
            PrefabUtility.ReplacePrefab(room, rooms[i], ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
            DestroyImmediate(room);
        }

        Debug.Log("Updated " + count + " objects");
    }

    public static void UpdateEnemyReferences()
    {
        int i;
        int j;
        int meleeCount = 0;
        int rangedCount = 0;
        int shieldedCount = 0;
        Object[] roomEnemies;
        GameObject room;
        List<GameObject> rooms = Resources.LoadAll("Room").Cast<GameObject>().ToList();
        List<GameObject> enemies = Resources.LoadAll("Enemy").Cast<GameObject>().ToList();
        GameObject meleeEnemy = null;
        GameObject rangedEnemy = null;

        for (i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetComponent<MeleeAI>() != null)
                meleeEnemy = enemies[i].gameObject;
            else if (enemies[i].GetComponent<EnemyRangedAttack>() != null)
                rangedEnemy = enemies[i].gameObject;
        }

        for (i = 0; i < rooms.Count; i++)
        {
            room = PrefabUtility.InstantiatePrefab(rooms[i]) as GameObject;

            if (meleeEnemy != null)
            {
                roomEnemies = room.GetComponentsInChildren<MeleeAI>();
                for (j = 0; j < roomEnemies.Length; j++)
                {
                    replace((roomEnemies[j] as MeleeAI).gameObject, meleeEnemy);
                    meleeCount++;
                }
            }

            if (rangedEnemy != null)
            {
                roomEnemies = room.GetComponentsInChildren<EnemyRangedAttack>();
                for (j = 0; j < roomEnemies.Length; j++)
                {
                    replace((roomEnemies[j] as EnemyRangedAttack).gameObject, rangedEnemy);
                    rangedCount++;
                }
            }

            PrefabUtility.ReplacePrefab(room, rooms[i], ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
            DestroyImmediate(room);
        }

        Debug.Log("Updated " + (meleeCount + rangedCount + shieldedCount) + " objects [Melee: " + meleeCount + ", Ranged: " + rangedCount + ", Shielded: " + shieldedCount + "]");
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
}
