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

            Object[] tiles = Resources.LoadAll("Tile");
            string[] strList = new string[tiles.Length];
            string selectionName = null;
            int selectionIndex = 0;
            int j;
            int i;

            for (i = 0; i < tiles.Length; i++)
            {
                strList[i] = tiles[i].name;
                for(j = 0; j < objects.Length; j++)
                {
                    if (objects[j].name == tiles[i].name)
                    {
                        if (selectionName == null)
                        {
                            selectionName = tiles[i].name;
                            selectionIndex = i;
                        }
                        else if (selectionName != tiles[i].name)
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
                    newTile = PrefabUtility.InstantiatePrefab(tiles[selected] as GameObject) as GameObject;
                    obj = objects[i] as GameObject;
                    Undo.RegisterCreatedObjectUndo(newTile, "Created replacement tile");

                    newTile.transform.position = obj.transform.position;
                    newTile.transform.rotation = obj.transform.rotation;
                    newTile.transform.localScale = obj.transform.localScale;
                    newTile.transform.parent = obj.transform.parent;
                    newTile.transform.SetSiblingIndex(obj.transform.GetSiblingIndex());

                    Undo.DestroyObjectImmediate(obj);
                    selection[i] = newTile;
                }

                Selection.objects = selection;
            }

        }
    }
}
