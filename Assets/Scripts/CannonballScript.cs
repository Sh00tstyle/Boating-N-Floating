using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballScript : MonoBehaviour {

    public SpawnSource Source;
    public float Lifetime;

    public GameObject WaterImpactParticles;
    public GameObject CannonballImpactParticles;

    private void OnTriggerEnter(Collider other) {
        GameObject particles = null;

        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) {
            particles = Instantiate(WaterImpactParticles);
            particles.transform.position = transform.position;
            print("Hit " + other.gameObject.tag);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == Tags.PLAYER && Source != SpawnSource.Player) {
            HealthScript targetHealth = other.gameObject.GetComponent<HealthScript>();
            targetHealth.TakeDamage(10f);

            particles = Instantiate(CannonballImpactParticles);
            particles.transform.position = transform.position;
            print("Hit " + other.gameObject.tag);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == Tags.ENEMY && Source != SpawnSource.EnemyHeavyWarship && Source != SpawnSource.EnemyLightFrigate) {
            //DEBUG AND PROFILE
            //GameObject.Find("Analysis").GetComponent<Analysis>().playerinfo.ShotsHit++;
            HealthScript targetHealth = other.gameObject.GetComponent<HealthScript>();
            targetHealth.TakeDamage(10f);

            particles = Instantiate(CannonballImpactParticles);
            particles.transform.position = transform.position;
            print("Hit " + other.gameObject.tag);
            Destroy(gameObject);
        }
    }

    public void Update () {
        Lifetime -= Time.deltaTime;

        if (Lifetime < 0) Destroy(gameObject);
	}
}
