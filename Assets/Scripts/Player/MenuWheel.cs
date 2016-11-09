using UnityEngine;
using System.Collections;

public class MenuWheel : MonoBehaviour {
    public GameObject[] listOfButtons;
    GameObject[] listOfButtons2; 

    private float radius = 170;
    private float nullRadius = 10;
    private float screenWidth = 1000;
    private float angle, length;
    private Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
    private float centerDistance = 30.0f;

    private int nrOptions;
    public GameObject Wheel;
    private Vector2[] coordinates;
    private float[] minMax;
    private Vector2 a, b, c;
    private float cos1, cos2, sin1, sin2;

    //TEST STUFF
  //  public GameObject testObject;
    private Vector2 test;
    private bool wheelClicked = false;
    private bool mouseRelease = false;
    private int currentButton;
    private int selectedButton;

    private SpearThrow ability;
    private InputManager IM;
    private EventManager EM;

    int ID;

    void Start()
    {
        ability = FindObjectOfType<SpearThrow>();
        nrOptions = listOfButtons.Length;
        listOfButtons2 = new GameObject[nrOptions];

        coordinates = new Vector2[nrOptions+1];
        minMax = new float[nrOptions * 2];
        angle = 2 * Mathf.PI / nrOptions;
        drawWheel();
        IM = GameManager.input;
        EM = GameManager.events;
        IM.OnFirstTouchBeginSub(checkIfCenter, ID);
        IM.OnFirstTouchEndSub(OnRelease, ID);
        IM.OnFirstTouchMoveSub(updateMouse, ID);
        Wheel.SetActive(false);
    }

    void Update()    {
       
        if (wheelClicked == true)
        {           
            checkWhichState();
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
            listOfButtons2[i].transform.localScale = Vector3.one*10;
            listOfButtons2[i].transform.localRotation = Quaternion.identity;
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

    void select(Vector2 touchPos) {
        if (Vector2.Distance(center, touchPos) <= centerDistance)
            selectedButton = 10;
        else
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
                EM.WheelSelect(0);
                ability.UseAbility();
                break;
            case 1:
                Debug.Log("Button 2 selected");
                EM.WheelSelect(1);
                break;
            case 2:
                Debug.Log("Button 3 selected");
                EM.WheelSelect(2);
                break;
            case 3:
                Debug.Log("Button 4 selected");
                EM.WheelSelect(3);
                break;
            case 10:
                EM.WheelSelect(10);
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
    
    void OnClick()
    {
        Debug.Log("MENU!!");
        IM.TakeControl(ID);
        Wheel.SetActive(true);
        wheelClicked = true;
    }

    void OnRelease(Vector2 touchPos)
    {
        if (wheelClicked)
        {
            select(touchPos);
            IM.ReleaseControl(ID);
        }
    }
    void checkIfCenter(Vector2 touchPos) {
        if (Vector2.Distance(center, touchPos) <= centerDistance)
            OnClick();
    }
    void updateMouse(Vector2 touchPos) {
        test = touchPos - new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
