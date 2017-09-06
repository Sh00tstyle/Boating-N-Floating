using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballScript : MonoBehaviour {

    public SpawnSource Source;
    public float Lifetime;   

    public void Update () {
        Lifetime -= Time.deltaTime;

        if (Lifetime < 0) Destroy(gameObject);
	}
}
