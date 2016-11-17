using UnityEngine;
using UnityEditor;
using System.Collections;

public class SnapToSurface : Editor{

    [MenuItem("Tools/Snap to Surface %w", false,22)]
    public static void Snap()
    {
        Object[] objects = Selection.objects;
        GameObject obj;
        Collider collider;
        Ray ray;
        RaycastHit hit;
        int[] layers = new int[objects.Length];
        int i;

        for (i = 0; i < objects.Length; i++)
        {
            obj = objects[i] as GameObject;
            layers[i] = obj.layer;
            obj.layer = 2;
        }

        for (i = 0; i < objects.Length; i++)
        {
            obj = objects[i] as GameObject;
            collider = obj.GetComponentInChildren<Collider>();

            if (collider)
            {
                ray = new Ray(obj.transform.position + Vector3.up * obj.transform.localScale.y, Vector3.down);

                if (Physics.Raycast(ray, out hit))
                {
                    ray = new Ray(hit.point + Vector3.down * obj.transform.localScale.y, Vector3.up);
                    obj.layer = 0;

                    if (collider.Raycast(ray, out hit, hit.distance + obj.transform.localScale.y))
                    {
                        Undo.RecordObject(obj.transform,"Snap to surface");
                        obj.transform.position += Vector3.down * (hit.distance - obj.transform.localScale.y);
                    }

                    obj.layer = 2;
                }
            }
        }

        for (i = 0; i < objects.Length; i++)
            (objects[i] as GameObject).layer = layers[i];
    }
}
