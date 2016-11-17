using UnityEngine;
using System.Collections;

public class ConeDraw : MonoBehaviour, IAbility {

    [SerializeField]
    private float cooldown, maxConeLength, minConeLength, maxConeWidth, cancelAngle, coneAltitude, coneSpeed, damage, pushForce, stunTime;
    [Range(1, 5)]
    [SerializeField]
    private int pointsPerDegreeInFullCircle;
    private GameObject drawCone, dmgCone, drawConeObj, coneDmgObject;
    private InputManager im;
    private bool drawing = false, clockwise = true, fire = false;
    private Vector3 start, end, cur;
    private int coneResolution, ID, activeTris;
    private Mesh coneMesh;
    private float currentCooldown = 0, length;
    private const float _2pi = Mathf.PI * 2;
    private bool dirSat = false;
    private Vector3[] coneDir;

    // Use this for initialization
    void Start () {
        drawConeObj = Resources.Load<GameObject>("Prefabs/Cone/ConeMESH");
        coneDmgObject = Resources.Load<GameObject>("Prefabs/Cone/ConeAbility");
        coneResolution = pointsPerDegreeInFullCircle * 360;
        im = GameManager.input;
        ID = im.GetID();
	}
	
    void Update()
    {
        currentCooldown -= Time.deltaTime;
    }
    public float Cooldown()
    {

        return currentCooldown < 0 ? 0 : currentCooldown;
    }

    public void UseAbility()
    {
        if (currentCooldown <= 0)
        {
            StartCoroutine("Ability");
        }
    }

    IEnumerator Ability()
    {

        im.OnFirstTouchBeginSub(GetDown, ID);
        im.OnFirstTouchMoveSub(GetMove, ID);
        im.OnFirstTouchEndSub(GetEnd, ID);
        im.TakeControl(ID);
        drawing = true;
        yield return StartCoroutine("DrawCone");

        im.ReleaseControl(ID);
        im.OnFirstTouchBeginUnsub (ID);
        im.OnFirstTouchMoveUnsub(ID);
        im.OnFirstTouchEndUnsub(ID);
        // Actually use the ability with the drawn points
        GameManager.events.DrawComplete(10);
        Destroy(drawCone);
        if (dirSat)//If abality was not cancelled
        { 
            currentCooldown = cooldown;
            dmgCone = (GameObject)Instantiate(coneDmgObject, drawCone.transform.position, drawCone.transform.rotation);
            dmgCone.GetComponent<ConeAbility>().setVars(length, coneSpeed, activeTris, coneMesh, damage, pushForce, stunTime);
        }
        yield return null;
    }
    void GetDown(Vector2 p)
    {
        start = im.GetWorldPoint(p);
        drawCone = (GameObject)Instantiate(drawConeObj, transform.position, Quaternion.identity);
        Vector3 lookDir = start - drawCone.transform.position;
        drawCone.transform.LookAt(transform.position+lookDir);
        drawCone.transform.Rotate(Vector3.up * -90);
        if (drawCone.GetComponent<MeshFilter>() == null)
            drawCone.AddComponent<MeshFilter>();
        if (drawCone.GetComponent<MeshRenderer>() == null)
            drawCone.AddComponent<MeshRenderer>();
        coneMesh = new Mesh( );
        coneMesh.name = ""+drawCone.GetInstanceID();
    }

    void GetMove(Vector2 p)
    {
        cur = im.GetWorldPoint(p);
        float y =(int) Quaternion.FromToRotation((cur - transform.position), (start - transform.position)).eulerAngles.y;
        if (y > cancelAngle/2f && y<180 && !dirSat || y < 360-(cancelAngle/2f) && !dirSat) //Decide which direction to draw cone in
        {
            clockwise = y < 180;
            dirSat = true;
        }
        if (y <= cancelAngle/2f && y<180 || y >=360-(cancelAngle/2f))
        {
            coneMesh.Clear();
            dirSat = false;
            coneMesh.vertices  = new Vector3[1];
            coneMesh.triangles = new int[3];
            drawCone.GetComponent<MeshFilter>( ).mesh = coneMesh;
        }
        if (y > maxConeWidth && clockwise || y<360-maxConeWidth && !clockwise)
        {
            //Do nothing to the mesh
        }
        else if (dirSat)
        {
            coneMesh.Clear();
            activeTris =(int) (clockwise ? y * (coneResolution  / 360) : (coneResolution) - (y * (coneResolution / 360))); //Calculate amount of triangles to draw (create a cone from sphere)
            length = maxConeLength * (1 - (float)activeTris / coneResolution); //Reduce cone-length based on angle-width
            if (length < minConeLength)
                length = minConeLength;
            Vector3[] vertices = new  Vector3[coneResolution + 2];
            Vector3[] normals = new Vector3[vertices.Length];
            int[] triangles = new int[activeTris*3];
            coneDir = new Vector3[activeTris];
            vertices[0] = Vector3.up*coneAltitude; //Assign center vertix
            normals[0] = Vector3.up;
            triangles[0] = 0;
            triangles[1] = clockwise ?  1 : vertices.Length - 2;
            triangles[2] = clockwise ? 2 : vertices.Length - 1;
            for (int k = 1; k<coneResolution+1; ++k) //Generate verticies for full circle
            {
                float phi = k * _2pi/(coneResolution-1);
                vertices[k] = new Vector3(Mathf.Cos(phi) , coneAltitude / length, -Mathf.Sin(phi))*length;
                normals[k] = Vector3.up;
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
            vertices[vertices.Length - 1] = vertices[1];
            normals [vertices.Length-1] = Vector3.up;
            coneMesh.vertices = vertices;
            coneMesh.normals = normals;
            coneMesh.triangles = triangles;
            drawCone.GetComponent<MeshFilter>( ).mesh = coneMesh;
        }
    }

    void GetEnd(Vector2 p)
    {
        drawing = false;
    }

    IEnumerator DrawCone()
    {
        while (drawing )
        {
            yield return null;
        }
        yield break;
    }
}
