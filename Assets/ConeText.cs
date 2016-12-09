using UnityEngine;
using System.Collections;

public class ConeText : MonoBehaviour {

    public GameObject myImg;
    public GameObject myText;
    private bool flipped = false;
    private const float resetScale = .002f;
    private const float flipTime = 1.5f;
    private const float tarScale = .01f;
    private float fadeVal = 0.3f;







    private float norm;
    private Vector3 faded, highlight = Vector3.one*.9f, dif, curCol;
    Renderer txtRend;
    TextMesh text;
    SpriteRenderer imgRend;

    private Vector3 targetPos, origPos, dir = new Vector3(-2, 0, 0);
    private Vector3 resScale, flipScale, scaleDif;
    private Quaternion origRot, tarRot;

	// Use this for initialization
	void Start () {
        faded = new Vector3(fadeVal, fadeVal, fadeVal);
        resScale = new Vector3(resetScale, resetScale, resetScale);
        flipScale = new Vector3(tarScale, tarScale, tarScale);
        scaleDif = flipScale - resScale;
        dif = highlight - faded;

        dir -= (Vector3.Scale(Camera.main.transform.up * 9, new Vector3(0, 1, 0)));
        txtRend = myText.GetComponent<MeshRenderer>();
        imgRend = myImg.GetComponent<SpriteRenderer>();
        norm = 0;
        setCol(faded);
        ResetPos();
    }
	
	// Update is called once per frame
	void Update () {
        tarRot.SetLookRotation(transform.position - Camera.main.transform.position);
    }

    void ResetPos()
    {
        myText.transform.position = myImg.transform.position;
        myText.transform.localScale = Vector3.one * resetScale;
        origPos = myText.transform.position;
        targetPos = dir + origPos;
        txtRend.enabled = false;
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
            setPos(origPos + dir * norm, resScale + scaleDif * norm);
            myText.transform.rotation = Quaternion.Lerp(origRot, tarRot, norm);
            setCol(faded + dif * norm);
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
            setPos(targetPos - dir * unorm, flipScale - scaleDif * unorm);
            myText.transform.rotation = Quaternion.Lerp(tarRot, origRot, unorm);
            setCol(highlight - dif * unorm);
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
