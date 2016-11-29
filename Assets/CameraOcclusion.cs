using UnityEngine;
using System.Collections;

public class CameraOcclusion : MonoBehaviour
{
    private Vector3[] tarPoint = new Vector3[4];
    [SerializeField]
    [Range(1, 3)]
    private short scanAreas;

    private short curRay;
    Ray ray;
    RaycastHit[] hit;
    void Start()
    {
        StartCoroutine(WaitForAnim(3.0f));
    }

    void Update()
    {
        ray = new Ray(transform.position, tarPoint[curRay]);
        hit = Physics.RaycastAll(ray, tarPoint[curRay].magnitude);
        for(int k = 0; k<hit.Length; ++k)
        {
            if (hit[k].transform.gameObject.tag == "Occluder")
                hit[k].transform.GetComponent<Occluder>().Stop();
        }
        ++curRay;
        if (curRay >= scanAreas + 1)
            curRay = 0;
    }

    IEnumerator WaitForAnim(float dur)
    {
        yield return new WaitForSeconds(dur);
        tarPoint[0] = GameManager.player.transform.position - transform.position;
        if (scanAreas == 2)
            tarPoint[1] = tarPoint[0] + Vector3.up * GameManager.player.GetComponent<CapsuleCollider>().height;
        if (scanAreas == 3)
        {
            tarPoint[2] = tarPoint[0] + Vector3.right * GameManager.player.GetComponent<CapsuleCollider>().radius + Vector3.up * GameManager.player.GetComponent<CapsuleCollider>().height / 2f;
            tarPoint[3] = tarPoint[2] - Vector3.right * GameManager.player.GetComponent<CapsuleCollider>().radius * 2;
        }
    }
}