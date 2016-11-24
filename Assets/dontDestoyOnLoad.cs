using UnityEngine;
using System.Collections;

public class dontDestoyOnLoad : MonoBehaviour {
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
}
