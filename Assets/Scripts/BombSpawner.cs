using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour {

    public GameObject bomb;

    private ResourceManager _resources;

    public void Awake() {
        _resources = GetComponentInParent<ResourceManager>();
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.J) && _resources.BombAmount > 0) {
            DropBomb();
        }
    }

    public void DropBomb() {
        GameObject newBomb = Instantiate(bomb);
        newBomb.transform.position = transform.position;

        _resources.UseBomb();
    }
}
