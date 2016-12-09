using UnityEngine;
using System.Collections;

public class SetWispText : MonoBehaviour {

    public GameObject myImg;
    public GameObject myText;
    private bool flipped = false;
    private const float resetScale = .002f;
    private const float flipTime = 1.5f;
    private const float tarScale = .01f;
    private float fadeVal = 0.3f;
    [SerializeField]
    AnimationCurve am;



    private const string dk = "Når en fjende er dræbt vil en ånd blive\nindsamlet,som fylder et blåt ånde-meter\nop, som aktiverer en elevator", en = "When an enemy is dead a spirit is\ncollected and fills a blue spirit bar,\nwhich activates an elevator";
    private string myString;

    private float norm;
    private Vector3 faded, highlight = Vector3.one * .9f, dif, curCol;
    Renderer txtRend;
    TextMesh text;
    SpriteRenderer imgRend;

    private Vector3 targetPos, origPos, dir = new Vector3(-2, 0, 0);
    private Vector3 resScale, flipScale, scaleDif;
    private Quaternion origRot, tarRot;





    // Use this for initialization
    void Start()
    {
        faded = new Vector3(fadeVal, fadeVal, fadeVal);
        resScale = new Vector3(resetScale, resetScale, resetScale);
        flipScale = new Vector3(tarScale, tarScale, tarScale);
        scaleDif = flipScale - resScale;
        dif = highlight - faded;

        dir -= (Vector3.Scale(Camera.main.transform.up * 7, new Vector3(0, 1, 0)));
        txtRend = myText.GetComponent<MeshRenderer>();
        imgRend = myImg.GetComponent<SpriteRenderer>();
        norm = 0;
        setCol(faded);
        myString = GameManager.game.language == GameManager.Language.Danish ? dk : en;
        ResetPos();
        myText.GetComponent<TextMesh>().text = myString;
    }

    // Update is called once per frame
    void Update()
    {
        tarRot.SetLookRotation(transform.position - Camera.main.transform.position);
    }

    void ResetPos()
    {
        myText.transform.position = myImg.transform.position;
        myText.transform.localScale = Vector3.one * resetScale;
        origPos = myText.transform.position;
        origRot.SetLookRotation(myImg.transform.forward);
        myText.transform.rotation = origRot;
        targetPos = dir + origPos;
        txtRend.enabled = false;
        flipped = false;
    }

    private void setCol(Vector3 col)
    {
        imgRend.color = new Color(col.x, col.y, col.z, 1);
    }
    private void setPos(Vector3 p, Vector3 s)
    {
        myText.transform.position = p;
        myText.transform.localScale = s;
    }

    IEnumerator flipImg()
    {
        txtRend.enabled = true;
        while (norm <= 1 && flipped)
        {
            setPos(origPos + dir * am.Evaluate(norm), resScale + scaleDif * am.Evaluate(norm));
            myText.transform.rotation = Quaternion.Lerp(origRot, tarRot, am.Evaluate(norm));
            setCol(faded + dif * am.Evaluate(norm));
            norm += Time.deltaTime / flipTime;
            yield return new WaitForEndOfFrame();
        }
        if (!flipped)
            yield break;
        norm = 1;
        setPos(targetPos, flipScale);
        myText.transform.rotation = tarRot;
        setCol(highlight);
        yield break;
    }
    IEnumerator unFlipImg()
    {
        while (norm >= 0 && !flipped)
        {
            float unorm = (1f - norm);
            setPos(targetPos - dir * unorm, flipScale - scaleDif * am.Evaluate(unorm));
            myText.transform.rotation = Quaternion.Lerp(tarRot, origRot, am.Evaluate(unorm) + .3f);
            setCol(highlight - dif * am.Evaluate(unorm));
            norm -= Time.deltaTime / flipTime;
            yield return new WaitForEndOfFrame();
        }
        if (flipped)
            yield break;
        txtRend.enabled = false;
        norm = 0;
        setPos(origPos, resScale);
        myText.transform.rotation = origRot;
        setCol(faded);
        yield break;
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            flipped = true;
            StartCoroutine(flipImg());
        }
    }

    void OnTriggerExit(Collider col)
    {

        if (col.tag == "Player")
        {
            flipped = false;
            StartCoroutine(unFlipImg());
        }

    }
}
