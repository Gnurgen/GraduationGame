using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectCurveDraw : MonoBehaviour {

    [SerializeField]
    private GameObject lineEffect;

    private List<Vector3> line;
    private List<GameObject> effects;
    private int effectIndex;

    void Start()
    {
        line = new List<Vector3>();
        effects = new List<GameObject>();
    }
	
    public void AddPoint(Vector3 point)
    {
        line.Add(point);
        if(line.Count > 1)
        {
            DrawLine(line[line.Count - 2], line[line.Count - 1]);
        }
    }

    public List<Vector3> GetPoints()
    {
        return line;
    }

    public List<GameObject> GetEffects()
    {
        return effects;
    }

    public void CleanUp()
    {
        line.Clear();
        effectIndex = 0;
    }

    void DrawLine(Vector3 from, Vector3 to)
    {
        Vector3 dir = to - from;
        Vector3 pos = (dir / 2) + from;
        if (effectIndex < effects.Count)
        {
            effects[effectIndex].transform.position = pos;
            effects[effectIndex].GetComponent<PKFxFX>().StartEffect();
            effects[effectIndex].GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Area", new Vector3(0, 0, dir.magnitude * 0.5f)));
            effects[effectIndex].GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("FireColor", new Vector3(0.5f, 0.5f, 1f)));
            effects[effectIndex].GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Fire", dir.magnitude));
            effects[effectIndex].transform.LookAt(to);
        }
        else
        {
            GameObject newLine = Instantiate(lineEffect, pos, Quaternion.identity) as GameObject;
            newLine.GetComponent<PKFxFX>().StartEffect();
            newLine.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Area", new Vector3(0, 0, (to - from).magnitude * 0.5f)));
            newLine.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("FireColor", new Vector3(0.5f, 0.5f, 1f)));
            newLine.GetComponent<PKFxFX>().SetAttribute(new PKFxManager.Attribute("Fire", dir.magnitude));
            newLine.transform.LookAt(to);
            effects.Add(newLine);

        }
        effectIndex += 1;
    }
}
