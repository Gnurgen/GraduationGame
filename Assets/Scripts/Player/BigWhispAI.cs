using UnityEngine;
using System.Collections;
using Pathfinding;

public class BigWhispAI : MonoBehaviour {

    public float spawnFrequency;
    public int activatorWhisps;
    public float elevatorDistance;
    public float endScatter;
    public float scatterRate;
    public float maxScatter;
    public GameObject pathWhisp;
    public GameObject activator;

    private Vector3 startPosition;
    private Vector3 elevatorPosition;
    private Vector3 spearPosition;
    private Transform player;
    private Vector3[] path;
    private PKFxFX effectControl;
    private float scatter;
    private bool waiting;

	void Start()
    {
        player = GameManager.player.transform;
        startPosition = new Vector3(player.position.x, 1, player.position.z);
        elevatorPosition = GameObject.FindWithTag("Elevator").transform.position;
        spearPosition = GameManager.spear.transform.position;
        GetComponent<Seeker>().StartPath(transform.position, elevatorPosition, GetPath);
        waiting = true;
        effectControl = GetComponent<PKFxFX>();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        effectControl.StopEffect();
        GameManager.events.GuideWhispScatter(gameObject);
        transform.position = spearPosition;
        scatter = endScatter;
        effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
        effectControl.StartEffect();
        while (scatter < maxScatter)
        {
            transform.position = spearPosition;
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            scatter += scatterRate * Time.deltaTime;
            yield return null;
        }
        while (scatter > endScatter)
        {
            transform.position = startPosition;
            effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
            scatter -= scatterRate * Time.deltaTime;
            yield return null;
        }
        scatter = endScatter;
        GameManager.events.GuideWhispScatterStop(gameObject);
        effectControl.SetAttribute(new PKFxManager.Attribute("Scatter", scatter));
        StartCoroutine(Spawning());
        yield break;
    }

    IEnumerator Spawning()
    {
        while(waiting)
        {
            yield return null;
        }
        while(Vector3.Distance(player.position, elevatorPosition) > elevatorDistance)
        {
            GameObject wisp = Instantiate(pathWhisp) as GameObject;
            wisp.GetComponent<PathWhispAI>().Activate(path);
            yield return new WaitForSeconds(spawnFrequency);
        }
        for(int i = 0; i < activatorWhisps; i++)
        {
            Vector2 rand = Random.insideUnitCircle * 3;
            GameObject actWisp = Instantiate(activator) as GameObject;
            actWisp.GetComponent<WhispActivationAI>().Activate(new Vector3(elevatorPosition.x, 1, elevatorPosition.z), new Vector3(elevatorPosition.x + rand.x + elevatorDistance, 1, elevatorPosition.z + rand.y + elevatorDistance));
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        GameManager.events.GuideWhispScatterStop(GameObject.FindWithTag("Elevator"));
        GameManager.events.GuideWhispFollowPathStop(gameObject);
        GameManager.events.ElevatorActivated();
        yield break;
    }

    void GetPath(Path p)
    {
        path = p.vectorPath.ToArray();
        waiting = false;
    }
}
