using UnityEngine;
using System.Collections;

public class MenuWheel : MonoBehaviour {
    private int nrOptions = 4;
    private float radius = 70;
    private float nullRadius = 10;
    private float screenWidth = 100;
    private float angle, length;
    public GameObject image;
    private GameObject Wheel;
    private Vector2[] coordinates;
    private float[] minMax;
    public GameObject testObject;
    private Vector2 test;
    private Vector2 a, b, c;
    private bool wheelIsUp = true;


    void Start()
    {

        coordinates = new Vector2[nrOptions+1];
        minMax = new float[nrOptions * 2];
        angle = 2 * Mathf.PI / nrOptions;
        drawWheel();
    }

    void Update()    {
        test = testObject.transform.position;
        if (wheelIsUp == true)
            checkChichState();
    }

    void drawWheel()
    {
        for (int i = 0; i < nrOptions; i++)
        {
            GameObject go = Instantiate(image) as GameObject;
            go.transform.parent = GameObject.Find("Wheel").transform;
            go.transform.localPosition = new Vector3(Mathf.Cos(angle * i) * radius, Mathf.Sin(angle * i) * radius, 0);
            coordinates[i + 1] = new Vector2(Mathf.Cos(angle * i) * screenWidth, Mathf.Sin(angle * i) * screenWidth);
        }

    }
    void checkChichState() {       
        switch (checkWhichField(test))
        {
            case 0:
                Debug.Log("Button 1 ");
                break;
            case 1:
                Debug.Log("Button 2");
                break;
            case 2:
                Debug.Log("Button 3");
                break;
            case 3:
                Debug.Log("Button 4");
                break;
            default:
                Debug.Log("Button Out of Bound");
                break;
        }
    }

    private int checkWhichField(Vector2 mousePos) {
        int currentTri = 0;

        bool[] myList = new bool[nrOptions];
        for (int i = 0; i < nrOptions; i++)
        {
            myList[i] = rightOfLine(mousePos, coordinates[i + 1]);
            Debug.Log(myList[i]);
        }

        for (int i = 0; i < nrOptions; i++)
            if (myList[i] == true && myList[(i + 1)%nrOptions] == false)
                currentTri = i;
            
                   
        return currentTri;

    }

    private bool rightOfLine(Vector2 mousePos, Vector2 numCoordinate){
        a = coordinates[0];
        b = numCoordinate;
        c = mousePos;
        
        length = (((b.y - a.y) * c.x) - ((b.x - a.x) * c.y) + ((b.x * a.y) - (b.y * a.y))) / (Mathf.Sqrt(Mathf.Pow(b.y - a.y, 2) + Mathf.Pow(b.x - a.x, 2)));

        if (length <= 0)
            return false;
        else
            return true;
    }

    void select(Vector2 releasePos) {
        
    }

  
}
