using UnityEngine;
using System.Collections;

public class MeteorShower : MonoBehaviour {


    public float MaxRangeOfAttack = 10;
    public float MinRangeOfAttack = 1;
    public float AreaOfBoulder = 2;
    public float FallAnimationSeconds = 0.3f;
    public float FallTime = 4;
    public float SecondsOfExpanding= 1;
    public float HeightOfFall = 10;
    public GameObject Boulder, Fallarea;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ActivateAbility();
        }
	}
    void ActivateAbility()
    {
        StartCoroutine("BoulderFall");
    }
    IEnumerator BoulderFall()
    {
        Vector2 pos = Random.insideUnitCircle * (MaxRangeOfAttack - MinRangeOfAttack);
        Vector3 actual = new Vector3(pos.x+ MinRangeOfAttack, 0, pos.y+ MinRangeOfAttack);
        GameObject FallArea = Instantiate(Fallarea, actual, Quaternion.identity) as GameObject;
        float step = SecondsOfExpanding;
        while(step > 0) // increase size of fallArea to areaBoulder in SecondsOfExpanding
        {
            step -= 1 * Time.deltaTime;
            Fallarea.transform.localScale = new Vector3((step / SecondsOfExpanding) * AreaOfBoulder, .1f,(step / SecondsOfExpanding) * AreaOfBoulder);
            yield return null;
        }
        step = FallTime - SecondsOfExpanding;

        while(step>FallAnimationSeconds) // wait for boulder to spawn
        {
            step -= 1 * Time.deltaTime;
            yield return null;
        }

        GameObject boulder = Instantiate(Boulder, actual+ Vector3.up * HeightOfFall, Quaternion.identity) as GameObject;

        while(step>0) // Let the boulder fall HeightOfFall distance in FallAnimationSeconds
        {
            step -= 1 * Time.deltaTime;
            boulder.transform.position = Vector3.Lerp(actual + Vector3.up * HeightOfFall, actual, step / FallAnimationSeconds);
            yield return null;
        }
        Destroy(FallArea);
    }
}
