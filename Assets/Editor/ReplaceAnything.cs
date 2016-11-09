using UnityEngine;
using UnityEditor;
using System.Collections;

public class ReplaceAnything : EditorWindow {

    private static Object _replacement;
    private static bool _useTranslation = true;
    private static bool _useRotation = true;
    private static bool _useScale = true;

    [MenuItem("Tools/Replace Anything",false,21)]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ReplaceAnything));
    }

    void OnGUI()
    {
        _replacement = EditorGUILayout.ObjectField("Prefab:", _replacement, typeof(GameObject), false) as GameObject;

        GUI.enabled = _replacement != null;

        GUILayout.Space(10);

        GUILayout.Label("Maintain:");
        _useTranslation = EditorGUILayout.Toggle("Translation", _useTranslation);
        _useRotation = EditorGUILayout.Toggle("Rotation", _useRotation);
        _useScale = EditorGUILayout.Toggle("Scale", _useScale);

        GUILayout.Space(10);

        if (GUILayout.Button("Replace Selection"))
        {
            Object[] objects = Selection.objects;
            Object[] selection = new Object[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                GameObject obj = PrefabUtility.InstantiatePrefab(_replacement) as GameObject;
                Undo.RegisterCreatedObjectUndo(obj, "Created replacement object");
                applyTransform(obj, (objects[i] as GameObject).transform);
                Undo.DestroyObjectImmediate(objects[i]);
                selection[i] = obj;
            }
            Selection.objects = selection;
        }
        GUI.enabled = true;
    }

    private static void applyTransform(GameObject obj, Transform transform)
    {
        if (_useTranslation)
            obj.transform.position = transform.position;
        if (_useRotation)
            obj.transform.rotation = transform.rotation;
        if (_useScale)
            obj.transform.localScale = transform.localScale;

        obj.transform.parent = transform.parent;
        obj.transform.SetSiblingIndex(transform.GetSiblingIndex());
    }
}
