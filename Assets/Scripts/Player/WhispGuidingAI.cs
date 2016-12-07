using UnityEngine;
using System.Collections;
using Pathfinding;

public class WhispGuidingAI : MonoBehaviour {

    // PUBLIC FIELDS
    [SerializeField]
    private float guidingRange;
    [SerializeField]
    private float pointSkipRange;
    [SerializeField]
    private GameObject activatorWhisp;
    [SerializeField]
    private int numberOfActivators;
    [SerializeField]
    private float activationDistance;
    [SerializeField]
    private float maxScatter;
    [SerializeField]
    private float scatterRate;
    [SerializeField]
    private float endScatter;


    // PRIVATE FIELDS
    private Seeker seeker;
    private Transform player;
    private Transform spear;
    private Transform elevator;
    private PKFxFX effectControl;
    private float scatter;
    private Vector3 guidingDir;
    private Vector3 dir;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        elevator = GameObject.FindWithTag("Elevator").transform;
        effectControl = GetComponent<PKFxFX>();
        player = GameManager.player.transform;
        spear = GameManager.spear.transform;
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        effectControl.StopEffect();
        GameManager.events.GuideWhispScatter(gameObject);
        guidingDir = elevator.position - player.position;
        seeker.StartPath(player.position, elevator.position, ReceivePath);
        transform.position = spear.position;
        scatter = endScatter;
        effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
        effectControl.StartEffect();
        while (scatter < maxScatter)
        {
            transform.position = spear.position;
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            scatter += scatterRate * Time.deltaTime;
            yield return null;
        }
        while (scatter > endScatter)
        {
            transform.position = new Vector3(player.position.x + guidingDir.x, 1, player.position.z + guidingDir.z);
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            scatter -= scatterRate * Time.deltaTime;
            yield return null;
        }
        scatter = endScatter;
        GameManager.events.GuideWhispScatterStop(gameObject);
        effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
        StartCoroutine(Guiding());
        yield break;
    }

    IEnumerator Guiding()
    {
        GameManager.events.GuideWhispFollowPath(gameObject);
        while(Vector3.Distance(transform.position, elevator.position) > activationDistance)
        {
            transform.position = new Vector3(player.position.x + guidingDir.x, 1, player.position.z + guidingDir.z);
            yield return null;
        }
        dir = (elevator.position - player.position).normalized;
        transform.position = elevator.position + new Vector3(dir.x * activationDistance, 1, dir.z * activationDistance);
        StartCoroutine(ActivatingElevator());
        yield break;
    }

    IEnumerator ActivatingElevator()
    {
      
        GameManager.events.GuideWhispScatter(elevator.gameObject);
        while (numberOfActivators > 0)
        {
            GameObject a = Instantiate(activatorWhisp) as GameObject;
            a.GetComponent<WhispActivationAI>().Activate(new Vector3(elevator.position.x, 0, elevator.position.z), transform.position);
            numberOfActivators -= 1;
            yield return new WaitForSeconds(0.05f);
        }
       
        effectControl.StopEffect();
        yield return new WaitForSeconds(1f);
        GameManager.events.GuideWhispScatterStop(elevator.gameObject);
        GameManager.events.GuideWhispFollowPathStop(gameObject);
        GameManager.events.ElevatorActivated();
        Destroy(gameObject);
        yield break;
    }

    void ReceivePath(Path path)
    {
        if (path.vectorPath.Count > 2)
        {
            dir = (path.vectorPath[2] - player.position).normalized;
        }
        else
        {
            dir = (path.vectorPath[1] - player.position).normalized;
        }
        guidingDir = new Vector3(dir.x * guidingRange, 1, dir.z * guidingRange);
        if(Vector3.Distance(transform.position, elevator.position) > activationDistance)
            seeker.StartPath(player.position, elevator.position, ReceivePath);
    }
}
