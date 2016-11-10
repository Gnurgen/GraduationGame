using UnityEngine;
using System.Collections;

public class LevelUpGUI : MonoBehaviour
{
    //TEst stuff
   // public GameObject testMouse;
   // private Vector3 mousePos;
    public bool clicked = false;
    public bool released = false;
    private bool isCard = false;

    //Real stuff
    public GameObject levelUpGUI;
    public GameObject[] cards;
    public GameObject[] slots;
    private Collider[] cardColliders;
    private Collider[] slotColliders;
    private GameObject selectedCard;
    private int selectedCardNum;
    private Vector3 selectedCardPos;
    private Vector3 tempPos;

    //Manager
    private InputManager IM;
    int ID;

    void Start()
    {
        IM = FindObjectOfType<InputManager>();
        IM.OnFirstTouchBeginSub(click, ID);
        IM.OnFirstTouchEndSub(release, ID);
        IM.OnFirstTouchMoveSub(followTouch, ID);

        cardColliders = new Collider[3];
        slotColliders = new Collider[3];

        for (int i = 0; i < 3; i++)
        {
            cardColliders[i] = cards[i].GetComponent<Collider>();
        }
        for (int i = 0; i < 3; i++)
        {
            slotColliders[i] = slots[i].GetComponent<Collider>();
        }
    }

    void click(Vector2 touchPos)
    {
        Vector3 temp = new Vector3(touchPos.x - Screen.width / 2, touchPos.y - Screen.height / 2 , 0);
        Debug.Log(touchPos + " and " + cardColliders[1].bounds);
        for (int i = 0; i < 3; i++)
        {
            if (cardColliders[i].bounds.Contains(temp))
            {
                Debug.Log("Click Card");
                selectedCard = cards[i];
                selectedCardPos = selectedCard.transform.position;
                selectedCardNum = i;
                isCard = true;
                clicked = true;
            }
            if (slotColliders[i].bounds.Contains(temp))
            {
               // Debug.Log("Card " + i + " selected");
                selectedCard = slots[i];
                selectedCardPos = selectedCard.transform.position;
                selectedCardNum = i;
                isCard = false;
                clicked = true;
            }
        }
    }
    void release(Vector2 touchPos)
    {
        clicked = false;
        released = false;
        for (int i = 0; i < 3; i++) 
        {
            if (slotColliders[i].bounds.Contains(touchPos) && isCard == true)
            {
                skillSelected(selectedCardNum ,i);
            }
            else if (slotColliders[i].bounds.Contains(touchPos) && isCard == false && i != selectedCardNum)
            {
                tempPos = slots[i].transform.position;
                //Debug.Log("Card " + i + " pending switch");
                slots[i].transform.position = selectedCardPos;
                selectedCard.transform.position = tempPos;
                selectedCardPos = tempPos;
            }
            else
            {
               
                selectedCard.transform.position = selectedCardPos;
            }

        }
    }
    void skillSelected(int cardNum, int slotNum)
    {
       // Debug.Log("Card #" + cardNum + " Has been moved to slot #" + slotNum);
        levelUpGUI.SetActive(false);
    }
    void followTouch(Vector2 touchPos) {        
        selectedCard.transform.position = touchPos;
        Debug.Log("Draggin");
    }


}
