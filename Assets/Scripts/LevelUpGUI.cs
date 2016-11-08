using UnityEngine;
using System.Collections;

public class LevelUpGUI : MonoBehaviour
{
    //TEst stuff
    public GameObject testMouse;
    private Vector3 mousePos;
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

    void Start()
    {
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

    void Update()
    {
        mousePos = testMouse.transform.position;
        if (clicked == true)
            selectedCard.transform.position = mousePos;
        if (clicked == true && released == true)
            release();
    }

    public void click()
    {
        for (int i = 0; i < 3; i++)
        {
            if (cardColliders[i].bounds.Contains(mousePos))
            {
                selectedCard = cards[i];
                selectedCardPos = selectedCard.transform.position;
                selectedCardNum = i;
                isCard = true;
                clicked = true;
            }
            if (slotColliders[i].bounds.Contains(mousePos))
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
    void release()
    {
        clicked = false;
        released = false;
        for (int i = 0; i < 3; i++) 
        {
            if (slotColliders[i].bounds.Contains(mousePos)&& isCard == true)
            {
                skillSelected(selectedCardNum ,i);
            }
            else if (slotColliders[i].bounds.Contains(mousePos) && isCard == false && i != selectedCardNum)
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
    public void testRelease() {
        released = true;
    }
}
