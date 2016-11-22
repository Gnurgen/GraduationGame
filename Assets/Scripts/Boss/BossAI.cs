using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class BossAI : MonoBehaviour {

    // ----- Inspector -----
    // ----- Stage 1 -----
    [Header("Stage 1")]
    [SerializeField]
    private float stage1End = 0.75f;
    [SerializeField]
    private float stage1LaserDelay = 5;
    [SerializeField]
    private float stage1LaserDuration = 10;
    [SerializeField]
    private float stage1LaserRotationSpeed = 10;
    [SerializeField]
    private float stage1LaserFrequency = 5;
    [SerializeField]
    private bool[] stage1Lasers = new bool[4] {true, true, false, false};
    [SerializeField]
    private float stage1MeteorDelay = 5;
    [SerializeField]
    private int stage1MeteorCount = 0;
    [SerializeField]
    private float stage1MeteorFrequency = 5;

    // ----- Stage 2 -----
    [Header("Stage 2")]
    [SerializeField]
    private float stage2End = 0.5f;
    [SerializeField]
    private float stage2LaserDelay = 5;
    [SerializeField]
    private float stage2LaserDuration = 10;
    [SerializeField]
    private float stage2LaserRotationSpeed = 10;
    [SerializeField]
    private float stage2LaserFrequency = 5;
    [SerializeField]
    private bool[] stage2Lasers = new bool[4] { true, true, false, false };
    [SerializeField]
    private float stage2MeteorDelay = 5;
    [SerializeField]
    private int stage2MeteorCount = 0;
    [SerializeField]
    private float stage2MeteorFrequency = 5;

    // ----- Stage 3 -----
    [Header("Stage 3")]
    [SerializeField]
    private float stage3End = 0.25f;
    [SerializeField]
    private float stage3LaserDelay = 5;
    [SerializeField]
    private float stage3LaserDuration = 10;
    [SerializeField]
    private float stage3LaserRotationSpeed = 10;
    [SerializeField]
    private float stage3LaserFrequency = 5;
    [SerializeField]
    private bool[] stage3Lasers = new bool[4] { true, true, false, false };
    [SerializeField]
    private float stage3MeteorDelay = 5;
    [SerializeField]
    private int stage3MeteorCount = 0;
    [SerializeField]
    private float stage3MeteorFrequency = 5;

    // ----- Stage 4 -----
    [Header("Stage 4")]
    [SerializeField]
    private float stage4End = 0;
    [SerializeField]
    private float stage4LaserDelay = 5;
    [SerializeField]
    private float stage4LaserDuration = 10;
    [SerializeField]
    private float stage4LaserRotationSpeed = 10;
    [SerializeField]
    private float stage4LaserFrequency = 5;
    [SerializeField]
    private bool[] stage4Lasers = new bool[4] { true, true, false, false };
    [SerializeField]
    private float stage4MeteorDelay = 5;
    [SerializeField]
    private int stage4MeteorCount = 0;
    [SerializeField]
    private float stage4MeteorFrequency = 5;



    // ----- Interne -----
    private Health health;
    private float currentLaserDelay;
    private float currentLaserFrequency;
    private float currentMeteorDelay;
    private float currentMeteorFrequency;
    private BossLaser laserAbility;
    private MeteorShower meteorAbility;


    // Use this for initialization
    void Start () {
        health = GetComponent<Health>();
        currentLaserDelay = 0;
        currentLaserFrequency = 0;
        currentMeteorDelay = 0;
        currentMeteorFrequency = 0;
        laserAbility = GetComponentInChildren<BossLaser>();
        meteorAbility = GetComponentInChildren<MeteorShower>();
        Activate();
    }
	
	// Update is called once per frame
	void Update () {
        currentLaserDelay -= Time.deltaTime;
        currentLaserFrequency -= Time.deltaTime;
        currentMeteorDelay -= Time.deltaTime;
        currentMeteorFrequency -= Time.deltaTime;
    }

    public void Activate()
    {
        GameManager.events.BossActivated(gameObject);
        currentLaserDelay = stage1LaserDelay;
        currentMeteorDelay = stage1MeteorDelay;
        StartCoroutine(Stage1());
    }

    IEnumerator Stage1()
    {
        laserAbility.DeActivate();
        currentLaserDelay = stage1LaserDelay;
        currentMeteorDelay = stage1MeteorDelay;
        while (health.health / health.maxHealth > stage1End)
        {
            // Stage 1 behaviour
            if(currentLaserDelay < 0 && currentLaserFrequency < 0)
            {
                laserAbility.Activate(stage1Lasers, stage1LaserRotationSpeed, stage1LaserDuration);
                currentLaserFrequency = stage1LaserFrequency;
            }
            if(currentMeteorDelay < 0 && currentMeteorFrequency < 0)
            {
                meteorAbility.Activate(stage1MeteorCount);
                currentMeteorFrequency = stage1MeteorFrequency;
            }
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(Stage2());
        yield break;
    }

    IEnumerator Stage2()
    {
        GameManager.events.BossPhaseChange(gameObject);
        laserAbility.DeActivate();
        currentLaserDelay = stage2LaserDelay;
        currentMeteorDelay = stage2MeteorDelay;
        while (health.health / health.maxHealth > stage2End)
        {
            // Stage 2 behavior
            if (currentLaserDelay < 0 && currentLaserFrequency < 0)
            {
                laserAbility.Activate(stage2Lasers, stage2LaserRotationSpeed, stage2LaserDuration);
                currentLaserFrequency = stage2LaserFrequency;
            }
            if (currentMeteorDelay < 0 && currentMeteorFrequency < 0)
            {
                meteorAbility.Activate(stage2MeteorCount);
                currentMeteorFrequency = stage2MeteorFrequency;
            }
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(Stage3());
        yield break;
    }

    IEnumerator Stage3()
    {
        GameManager.events.BossPhaseChange(gameObject);
        laserAbility.DeActivate();
        currentLaserDelay = stage3LaserDelay;
        currentMeteorDelay = stage3MeteorDelay;
        while (health.health / health.maxHealth > stage3End)
        {
            // Stage 3 behavior
            if (currentLaserDelay < 0 && currentLaserFrequency < 0)
            {
                laserAbility.Activate(stage3Lasers, stage3LaserRotationSpeed, stage3LaserDuration);
                currentLaserFrequency = stage3LaserFrequency;
            }
            if (currentMeteorDelay < 0 && currentMeteorFrequency < 0)
            {
                meteorAbility.Activate(stage3MeteorCount);
                currentMeteorFrequency = stage3MeteorFrequency;
            }
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(Stage4());
        yield break;
    }

    IEnumerator Stage4()
    {
        GameManager.events.BossPhaseChange(gameObject);
        currentLaserDelay = stage4LaserDelay;
        currentMeteorDelay = stage4MeteorDelay;
        while (health.health / health.maxHealth > stage4End)
        {
            // Stage 4 behavior
            if (currentLaserDelay < 0 && currentLaserFrequency < 0)
            {
                laserAbility.Activate(stage4Lasers, stage4LaserRotationSpeed, stage4LaserDuration);
                currentLaserFrequency = stage4LaserFrequency;
            }
            if (currentMeteorDelay < 0 && currentMeteorFrequency < 0)
            {
                meteorAbility.Activate(stage4MeteorCount);
                currentMeteorFrequency = stage4MeteorFrequency;
            }
            yield return new WaitForFixedUpdate();
        }
        GameManager.events.BossDeath(gameObject);
        yield break;
    }

}
