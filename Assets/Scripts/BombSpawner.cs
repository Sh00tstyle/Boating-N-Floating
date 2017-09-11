using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour {

    public GameObject bomb;

    public void Update() {
        if(Input.GetKeyDown(KeyCode.J)) {
            DropBomb();
        }
    }

    public void DropBomb() {
        GameObject newBomb = Instantiate(bomb);
        newBomb.transform.position = transform.position;
    }
}
