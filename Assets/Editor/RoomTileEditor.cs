using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(RoomTile))]
[CanEditMultipleObjects]
public class RoomTileEditor : Editor
{

    override public void OnInspectorGUI()
    {
        RoomTile tile = target as RoomTile;

        if (AssetDatabase.Contains(tile.gameObject))
        {
            DrawDefaultInspector();
        }
        else
        {
            Object[] objects = Selection.objects;
            Object[] selection = new Object[objects.Length];
            Object[] prefabs = Resources.LoadAll("Tile");
            RoomTile prefab;

            string[] strList = new string[prefabs.Length];
            string selectionName = null;
            int selectionIndex = 0;
            int j;
            int i;

            for (i = 0; i < prefabs.Length; i++)
            {
                prefab = (prefabs[i] as GameObject).GetComponent<RoomTile>();
                strList[i] = prefab.prefabInstance;
                for(j = 0; j < objects.Length; j++)
                {
                    tile = (objects[j] as GameObject).GetComponent<RoomTile>();
                    if (tile.prefabInstance == prefab.prefabInstance)
                    {
                        if (selectionName == null)
                        {
                            selectionName = tile.prefabInstance;
                            selectionIndex = i;
                        }
                        else if (selectionName != tile.prefabInstance)
                            selectionIndex = -1;
                    }
                }
            }

            int selected = EditorGUILayout.Popup("Tile:", selectionIndex, strList);
            if (selectionIndex != selected)
            {
                GameObject newTile;
                GameObject obj;

                for (i = 0; i < objects.Length; i++)
                {
                    newTile = PrefabUtility.InstantiatePrefab(prefabs[selected] as GameObject) as GameObject;
                    obj = objects[i] as GameObject;
                    Undo.RegisterCreatedObjectUndo(newTile, "Created replacement tile");

                    newTile.transform.position = obj.transform.position;
                    newTile.transform.rotation = obj.transform.rotation;
                    newTile.transform.localScale = obj.transform.localScale;
                    newTile.transform.parent = obj.transform.parent;
                    newTile.transform.SetSiblingIndex(obj.transform.GetSiblingIndex());

                    obj.transform.parent.transform.parent.GetComponent<RoomUnit>().ReplaceTile(newTile.GetComponent<RoomTile>(), obj.GetComponent<RoomTile>().index);

                    Undo.DestroyObjectImmediate(obj);
                    selection[i] = newTile;
                }

                Selection.objects = selection;
            }

        }
    }
}
