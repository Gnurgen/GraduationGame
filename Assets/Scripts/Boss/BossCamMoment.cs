using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossCamMoment : MonoBehaviour {
    private BossAI boss;
    private Vector3 bossT;
    public GameObject bossTop, bossMiddle, bossButtom, bossObj;
    private Image[] playerImg;
    public GameObject playerHealthBar, bossHealtbar, bossHealtbarB, options;
    private CAMERA_MoveWithPlayer cam;
    private Quaternion headA, headB;

    private float maxAngle, t;
    private float currentTime = 0;
    void Start()
    {
        bossHealtbar.GetComponent<Image>().CrossFadeAlpha(0, 0, true);
        bossHealtbarB.GetComponent<Image>().CrossFadeAlpha(0, 0, true);
        bossObj = GameObject.Find("Boss");
        bossT = (bossTop.transform.position - (Camera.main.transform.forward * 15)) + new Vector3(0, 3, 0) ;
        boss = GameObject.Find("Boss").GetComponent<BossAI>();
        cam = Camera.main.GetComponent<CAMERA_MoveWithPlayer>();
        playerImg = playerHealthBar.GetComponentsInChildren<Image>();
    }
    void fadePlayerBars(int i, float time) {
        foreach (Image j in playerImg)
            j.CrossFadeAlpha(i, time, true);
    }
    void OnTriggerEnter(Collider col) 
    {
        if (col.tag == "Player") {
            StartCoroutine(waitForMove());
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
        fadePlayerBars(0, 0.2f);
        options.GetComponent<Image>().CrossFadeAlpha(0, 0.2f, true);
        yield return StartCoroutine(cam.lookatWhatever(GameManager.player.transform.position, bossT,2));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(turnHead(180,2, bossTop));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(turnHead(210, 1, bossButtom));
        StartCoroutine(turnHead(-180, 1, bossMiddle));
        GameManager.events.CameraShake(1.5f);
        yield return StartCoroutine(cam.cameraShake(1.5f));
        boss.Activate();
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(cam.lookatWhatever(bossT,GameManager.player.transform.position, 2));
        cam.releaseControl();
        options.GetComponent<Image>().CrossFadeAlpha(1, 1, true);
        fadePlayerBars(1, 1);
        bossHealtbar.GetComponent<Image>().CrossFadeAlpha(1, 1, true);
        bossHealtbarB.GetComponent<Image>().CrossFadeAlpha(1, 1, true);

        gameObject.SetActive(false);
    }
}
