using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Skip2Boss : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.touchCount > 8 || Input.GetKeyDown(KeyCode.F10))
        {
            SceneManager.LoadScene("BossLevel");
        }
	}
}
