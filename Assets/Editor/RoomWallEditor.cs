using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(RoomWall))]
[CanEditMultipleObjects]
public class RoomWallEditor : Editor
{

    override public void OnInspectorGUI()
    {
        RoomWall wall = target as RoomWall;

        if (AssetDatabase.Contains(wall.gameObject))
        {
            DrawDefaultInspector();
        }
        else
        {
            Object[] objects = Selection.objects;
            Object[] selection = new Object[objects.Length];
            Object[] prefabs = Resources.LoadAll("Wall");
            RoomWall prefab;

            string[] strList = new string[prefabs.Length];
            string selectionName = null;
            int selectionIndex = 0;
            int j;
            int i;

            for (i = 0; i < prefabs.Length; i++)
            {
                prefab = (prefabs[i] as GameObject).GetComponent<RoomWall>();
                strList[i] = prefab.prefabInstance;
                for (j = 0; j < objects.Length; j++)
                {
                    wall = (objects[j] as GameObject).GetComponent<RoomWall>();
                    if (wall.prefabInstance == prefab.prefabInstance)
                    {
                        if (selectionName == null)
                        {
                            selectionName = wall.prefabInstance;
                            selectionIndex = i;
                        }
                        else if (selectionName != wall.prefabInstance)
                            selectionIndex = -1;
                    }
                }
            }

            int selected = EditorGUILayout.Popup("Wall:", selectionIndex, strList);
            if (selectionIndex != selected)
            {
                GameObject newWall;
                GameObject obj;

                for (i = 0; i < objects.Length; i++)
                {
                    newWall = PrefabUtility.InstantiatePrefab(prefabs[selected] as GameObject) as GameObject;
                    obj = objects[i] as GameObject;
                    Undo.RegisterCreatedObjectUndo(newWall, "Created replacement tile");

                    newWall.transform.position = obj.transform.position;
                    newWall.transform.rotation = obj.transform.rotation;
                    newWall.transform.localScale = obj.transform.localScale;
                    newWall.transform.parent = obj.transform.parent;
                    newWall.transform.SetSiblingIndex(obj.transform.GetSiblingIndex());

                    obj.transform.parent.transform.parent.GetComponent<RoomUnit>().ReplaceWall(newWall.GetComponent<RoomWall>(), obj.GetComponent<RoomWall>().index);

                    Undo.DestroyObjectImmediate(obj);
                    selection[i] = newWall;
                }

                Selection.objects = selection;
            }

        }
    }
}
