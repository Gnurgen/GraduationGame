using UnityEngine;
using System.Collections;

public class GuidingAI : MonoBehaviour {

    // PUBLIC FIELDS
    [SerializeField]
    private float guidingRange;
    [SerializeField]
    private float pointSkipRange;

    // PRIVATE FIELDS
    private Seeker seeker;
    private Transform player;
    private Transform spear;
    private PKFxFX effectControl;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        effectControl = GetComponent<PKFxFX>();
        player = GameManager.player.transform;
        spear = GameManager.spear.transform;
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {

        yield break;
    }

    IEnumerator Guiding()
    {
        yield break;
    }

    IEnumerator ActivatingElevator()
    {
        yield break;
    }

}
