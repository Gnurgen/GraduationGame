using UnityEngine;
using System.Collections;

public class testLevelUp : MonoBehaviour {

    public void levelUp() {
        GameManager.events.LevelUp(1);
    }
}
