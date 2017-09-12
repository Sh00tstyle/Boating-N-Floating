using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballScript : MonoBehaviour {

    public SpawnSource Source;
    public float Lifetime;

    public GameObject WaterImpactParticles;
    public GameObject CannonballImpactParticles;

    private void OnTriggerEnter(Collider other) {
        GameObject particles;

        if(other.gameObject.layer == LayerMask.NameToLayer("Water")) {
            particles = Instantiate(WaterImpactParticles);
            particles.transform.position = transform.position;
        } else {
            particles = Instantiate(CannonballImpactParticles);
            particles.transform.position = transform.position;
        }

        Destroy(gameObject);
    }

    public void Update () {
        Lifetime -= Time.deltaTime;

        if (Lifetime < 0) Destroy(gameObject);
	}
}
