using UnityEngine;
using System.Collections;

public class ConeDraw : MonoBehaviour {

    [SerializeField]
    private float maxConeLength, minConeLength, maxConeWidth, cancelAngle, coneAltitude, coneSpeed, damage, pushForce, stunTime;
    [Range(1, 10)]
    [SerializeField]
    private int pointResolution = 1;
    private GameObject drawCone, dmgCone, drawConeObj, coneDmgObject;
    private InputManager im;
    private bool drawing = false, clockwise = true;
    private Vector3 start, end, cur, lookDir;
    private int coneResolution, ID, activeTris;
    private float length;
    private const float _2pi = Mathf.PI * 2;
    private bool dirSat = false;
    private bool doDraw = false;
    private int onlyHitLayermask;
    private float _currentCooldown;
    public float currentCooldown
    {
        get
        {
            return _currentCooldown;
        }
        set
        {
            _currentCooldown = value;
        }
    }
    //Collision checking
    Ray ray;
    RaycastHit hit;

    // Use this for initialization
    void Start () {
        drawConeObj = Resources.Load<GameObject>("Prefabs/Cone/ConeMESH");
        coneDmgObject = Resources.Load<GameObject>("Prefabs/Cone/ConeAbility");
        coneResolution = pointResolution * 90;
        im = GameManager.input;
        ID = im.GetID();
        onlyHitLayermask = 1 << LayerMask.NameToLayer("ConeBlocker");
    }
	
    void Update()
    {
        _currentCooldown -= Time.deltaTime;
    }

    public void UseAbility(Vector3 p)
    {
        clockwise = true;
        dirSat = false;
        start = p;
        doDraw = false;
        drawCone = (GameObject)Instantiate(drawConeObj, transform.position, Quaternion.identity);
        lookDir = start - drawCone.transform.position;
        drawCone.transform.LookAt(drawCone.transform.position + lookDir);
        drawCone.transform.Rotate(Vector3.up * -90);
        if (drawCone.GetComponent<MeshFilter>() == null)
            drawCone.AddComponent<MeshFilter>();
        if (drawCone.GetComponent<MeshRenderer>() == null)
            drawCone.AddComponent<MeshRenderer>();
        drawCone.GetComponent<MeshFilter>().mesh = new Mesh();
        drawCone.GetComponent<MeshFilter>().mesh.name = ""+drawCone.GetInstanceID();
        StartCoroutine(Ability());

    }

    IEnumerator Ability()
    {
        im.OnFirstTouchMoveSub(GetMove, ID);
        im.OnFirstTouchEndSub(GetEnd, ID);
        im.TakeControl(ID);
        drawing = true;
        yield return StartCoroutine(DrawCone());
        im.ReleaseControl(ID);
        im.OnFirstTouchMoveUnsub(ID);
        im.OnFirstTouchEndUnsub(ID);
        // Actually use the ability with the drawn points
        Destroy(drawCone);
        if (doDraw)//If abality was not cancelled
        {
            GetComponent<PlayerControls>().EndAbility(true);
            GameManager.events.ConeAbilityUsed(GameManager.player);
            dmgCone = (GameObject)Instantiate(coneDmgObject, drawCone.transform.position, drawCone.transform.rotation);
            dmgCone.GetComponent<ConeAbility>().setVars(coneSpeed, activeTris, drawCone.GetComponent<MeshFilter>().mesh, damage, pushForce, stunTime);
        }
        else
        {
            GetComponent<PlayerControls>().EndAbility(false);
            GameManager.events.ConeAbilityCancel(gameObject);
        }
        yield break;
    }

    private bool coneDrawAnalysis(float y)
    {
        if (clockwise && y > 270)
            dirSat = false;
        else if (!clockwise && y < 90)
            dirSat = false;

        if (y < 270 && !dirSat)
        {
            clockwise = true;
            dirSat = true;
        }
        else if(y > 90 && !dirSat)
        {
            clockwise = false;
            dirSat = true;
        }
        return y > cancelAngle / 2f && clockwise|| y < 360f - cancelAngle / 2f && !clockwise;
    }

    void GetMove(Vector2 p)
    {
        cur = im.GetWorldPoint(p);
        float y =(int) Quaternion.FromToRotation((cur - drawCone.transform.position), (start - drawCone.transform.position)).eulerAngles.y;
        doDraw = coneDrawAnalysis(y);

        if (!doDraw)
        {
            drawCone.GetComponent<MeshFilter>().mesh.Clear();
            drawCone.GetComponent<MeshFilter>().mesh.vertices = new Vector3[1];
            drawCone.GetComponent<MeshFilter>().mesh.triangles = new int[3];
        }
        else if (y > maxConeWidth && clockwise || y<360-maxConeWidth && !clockwise)
        {
            y = 180;
        }
        if(doDraw)
        {
            drawCone.GetComponent<MeshFilter>().mesh.Clear();
            activeTris =(int) (clockwise ? y * (coneResolution  / 360f) : (coneResolution) - (y * (coneResolution / 360f))); //Calculate amount of triangles to draw (create a cone from sphere)
            length = maxConeLength * (1 - (float)activeTris / coneResolution); //Reduce cone-length based on angle-width
            if (length < minConeLength)
                length = minConeLength;
            Vector3[] vertices = new  Vector3[coneResolution + 2];
            Vector3[] normals = new Vector3[vertices.Length];
            Vector2[] uvs = new Vector2[vertices.Length];
            int[] triangles = new int[activeTris*3];
            vertices[0] = Vector3.up*coneAltitude; //Assign center vertix
            normals[0] = Vector3.up;
            triangles[0] = 0;
            triangles[1] = clockwise ?  1 : vertices.Length - 2;
            triangles[2] = clockwise ? 2 : vertices.Length - 1;
            for (int k = 1; k<coneResolution+1; ++k) //Generate verticies for full circle
            {
                float phi = k * _2pi/(coneResolution-1);
                Vector3 curVert = new Vector3(Mathf.Cos(phi), coneAltitude / length, -Mathf.Sin(phi));
                normals[k] = Vector3.up;
                ray = new Ray(drawCone.transform.position, Quaternion.Euler(drawCone.transform.rotation.eulerAngles)*curVert);
                if(Physics.Raycast(ray, out hit, length, onlyHitLayermask))
                    vertices[k] = curVert * hit.distance;
                else
                    vertices[k] = curVert*length;
                
                uvs[k] = new Vector2(.5f+(vertices[k].x)/(2*length), .5f+(vertices[k].z)/(2*length));
                if (k < activeTris) //Draw triangles only in fields encapsuled by the drawn angle
                {
                    if (!clockwise)
                    {
                        triangles[k * 3] = k ;
                        triangles[k * 3 + 1] = k + 1;
                        triangles[k * 3 + 2] = 0;
                    }
                    else
                    {
                        triangles[k * 3]  = vertices.Length - (k + 1);
                        triangles[k * 3 + 1] = vertices.Length - k;
                        triangles[k * 3 + 2] = 0;
                    }
                }
            }
            Vector3 coneLook = vertices[triangles[(triangles.Length / 3 / 2 - 1) * 3]];
            coneLook.y = 0;
            transform.LookAt(transform.position  + Quaternion.Euler(drawCone.transform.rotation.eulerAngles) * coneLook);
            vertices[vertices.Length - 1] = vertices[1];
            uvs[0] = Vector2.one*.5f;
            uvs[vertices.Length - 1] = uvs[0];
            normals [vertices.Length-1] = Vector3.up;
            drawCone.GetComponent<MeshFilter>().mesh.vertices = vertices;
            drawCone.GetComponent<MeshFilter>().mesh.triangles = triangles;
            drawCone.GetComponent<MeshFilter>().mesh.uv = uvs;
            drawCone.GetComponent<MeshFilter>().mesh.normals = normals;
        }
    }

    void GetEnd(Vector2 p)
    {
        drawing = false;

    }

    IEnumerator DrawCone()
    {
        while (drawing)
        {
            yield return null;
        }
        yield break;
    }
}