using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeScript : MonoBehaviour {

    public float Lifetime;

    public void Update() {
        if(Lifetime <= 0f) {
            Destroy(gameObject);
        }

        Lifetime -= Time.deltaTime;
    }
}
