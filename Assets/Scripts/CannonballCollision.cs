using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballCollision : MonoBehaviour {

    private float _timer;

    public void Awake() {
        _timer = 0f;
    }

    public void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }

    public void Update() {
        _timer += Time.deltaTime;

        if(_timer >= 5f) {
            Destroy(gameObject);
        }
    }
}
