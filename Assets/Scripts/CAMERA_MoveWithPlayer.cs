using UnityEngine;
using System.Collections;

public class CAMERA_MoveWithPlayer : MonoBehaviour
{

    [SerializeField]
    private float distance;
    [SerializeField]
    private Vector3 viewingAngle;
    private Transform player;

    //Camera Shakes
    float duration = 3;
    float magnitude = 0.2f;
    bool shaking = false;
    Vector3 shakingVec;

    //Force movement for "Cutscenes"
    bool takeControl = false;
    float currentT, t;


    void Start()
    {
        GameManager.events.OnBossDeath += shakeCamera;
    }

    void Awake()
    {
        transform.rotation = Quaternion.identity;
        player = GameManager.player.transform;
        transform.Rotate(viewingAngle);
        if (player != null)
            transform.position = player.position - transform.forward * distance;
    }

    void LateUpdate()
    {
        if (player != null &&  !takeControl)
        {
            Debug.Log("Updating");
            transform.position = player.position - transform.forward * distance;
        }       
    }
    public void shakeCamera(GameObject ID)
    {
        StartCoroutine(cameraShake(3));
    }
    public void bossShakeCamera(float duration) {
        StartCoroutine(cameraShake(duration));
    }

    IEnumerator cameraShake(float duration)
    {
        Vector3 oriPos = transform.position;
        while (currentT < duration)
        {
            takeControl = true;
            currentT += Time.deltaTime;
            shakingVec = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.4f, 0.4f), 0);
            transform.position = oriPos + shakingVec;
            yield return null;
        }
        transform.position = oriPos;
        currentT = 0;
        yield break;
    }

    public void lookAtObj(Vector3 startPos, Vector3 endPos, float duration)
    {
        StartCoroutine(lookatWhatever(startPos, endPos, duration));
    }

    IEnumerator lookatWhatever(Vector3 startPos, Vector3 endPos, float duration)
    {
        while (currentT < duration)
        {
            takeControl = true;
            currentT += Time.deltaTime;
            t = 1 - ((duration - currentT) / duration);            
            transform.position = Vector3.Lerp(startPos - transform.forward * distance, endPos - transform.forward * distance, t);
            yield return null;
        }
        currentT = 0;
        yield break;
    }
    public void  releaseControl() {
        takeControl = false;
    }
}
