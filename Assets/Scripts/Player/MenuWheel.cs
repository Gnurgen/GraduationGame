using UnityEngine;
using System.Collections;

public class MenuWheel : MonoBehaviour {
    public GameObject[] listOfButtons;
    GameObject[] listOfButtons2; 

    private float radius = 70;
    private float nullRadius = 10;
    private float screenWidth = 1000;
    private float angle, length;
    
    private int nrOptions;
    public GameObject Wheel;
    private Vector2[] coordinates;
    private float[] minMax;
    private Vector2 a, b, c;
    private float cos1, cos2, sin1, sin2;

    //TEST STUFF
    public GameObject testObject;
    private Vector2 test;
    public bool wheelClicked = true;
    public bool mouseRelease = false;
    private int currentButton;
    private int selectedButton;

    void Start()
    {
        nrOptions = listOfButtons.Length;
        listOfButtons2 = new GameObject[nrOptions];

        coordinates = new Vector2[nrOptions+1];
        minMax = new float[nrOptions * 2];
        angle = 2 * Mathf.PI / nrOptions;
        drawWheel();
    }

    void Update()    {
        test = testObject.transform.position;
        if (wheelClicked == true)
        {
            Wheel.SetActive(true);
            checkWhichState();
            if (mouseRelease == true)
            {
                select();
            }
        }
    }

    void drawWheel()
    {
        for (int i = 0; i < nrOptions; i++)
        {
            float currentAngle = (angle * i) -(Mathf.PI/2.0f);
            sin1 = currentAngle;
            sin2 = currentAngle - (angle / 4.0f) ;

            if (currentAngle == 0)
                cos1 = 0;
            else
                cos1 = currentAngle;
            
            if (currentAngle - (angle / 2.0f) == 0)
                cos2 = 0;
            else
                cos2 = currentAngle - (angle / 4.0f);
  
            listOfButtons2[i] = Instantiate(listOfButtons[i]) as GameObject;
            listOfButtons2[i].transform.parent = GameObject.Find("Wheel").transform;
            listOfButtons2[i].transform.localPosition = new Vector3(Mathf.Cos(cos1+Mathf.PI) * radius, Mathf.Sin(sin1 + Mathf.PI) * radius, 0);
            coordinates[i + 1] = new Vector2(Mathf.Cos(cos2) * screenWidth, Mathf.Sin(sin2) * screenWidth);
        }

    }
    void checkWhichState() {       
        switch (checkWhichField(test))
        {
            case 0:
                hover(0);               
                break;
            case 1:
                hover(1);
                break;
            case 2:
                hover(2);
                break;
            case 3:
                hover(3);
                break;
            default:
                Debug.Log("Button Out of Bound");
                break;
        }
    }

    private int checkWhichField(Vector2 mousePos) {
        currentButton = 0;

        bool[] myList = new bool[nrOptions];
        for (int i = 0; i < nrOptions; i++)
        {
            myList[i] = rightOfLine(mousePos, coordinates[i + 1]);
        }

        for (int i = 0; i < nrOptions; i++)
            if (myList[i] == true && myList[(i + 1)%nrOptions] == false)
                currentButton = i;
            
                   
        return currentButton;

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

    void select() {
        selectedButton = currentButton;
        wheelClicked = false;
        Wheel.SetActive(false);
        mouseRelease = false;

        switch (selectedButton) {
            default:
                Debug.Log("Button not found");
                break;
            case 0:
                Debug.Log("Button 1 selected");
                break;
            case 1:
                Debug.Log("Button 2 selected");
                break;
            case 2:
                Debug.Log("Button 3 selected");
                break;
            case 3:
                Debug.Log("Button 4 selected");
                break;

        }
    }
    void hover(int exception) {
        for (int i = 0; i < nrOptions; i++) {
            if(i != exception)
                listOfButtons2[i].GetComponent<MenuButtonHover>().hoverImageOff();
            else
                listOfButtons2[i].GetComponent<MenuButtonHover>().hoverImageOn();
        }

    }
  
}
