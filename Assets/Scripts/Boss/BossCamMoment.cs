using UnityEngine;
using System.Collections;

public class BossCamMoment : MonoBehaviour {
    private BossAI boss;
    private Vector3 bossT;
    public GameObject bossTop, bossButtom, bossObj;
    private CAMERA_MoveWithPlayer cam;
    private Quaternion headA, headB;

    private float maxAngle, t;
    private float currentTime = 0;
    void Start()
    {
        bossObj = GameObject.Find("Boss");
        bossT = GameObject.Find("Boss").transform.position+ new Vector3(0,3,0);
        boss = GameObject.Find("Boss").GetComponent<BossAI>();
        cam = Camera.main.GetComponent<CAMERA_MoveWithPlayer>();
    }
    void Update()
    {

   }
    void OnTriggerEnter(Collider col) 
    {
        if (col.tag == "Player") {
            StartCoroutine(waitForMove());
            cam.releaseControl();
        }
    }
    IEnumerator turnHead(float angle, float duration, GameObject head) {
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            t = 1 - ((duration - currentTime) / duration);
            head.transform.localRotation = Quaternion.Euler(0, angle*t,0);
            yield return null;
        }
            
        currentTime = 0;
        yield break;
    }
    IEnumerator waitForMove() {
        cam.lookAtObj(GameManager.player.transform.position, bossT,2);
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(turnHead(180,2, bossTop));
        yield return new WaitForSeconds(1);
        cam.bossShakeCamera(1);
        yield return StartCoroutine(turnHead(30, 1, bossButtom));
        boss.Activate();
        yield return new WaitForSeconds(2);
        cam.lookAtObj(bossT,GameManager.player.transform.position, 2);
    }
}
