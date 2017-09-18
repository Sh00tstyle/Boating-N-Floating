using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballScript : MonoBehaviour {

    public SpawnSource Source;
    public float Lifetime;

    private float _damage;

    public GameObject WaterImpactParticles;
    public GameObject CannonballImpactParticles;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == Tags.CANNONBALL) return;

        GameObject particles = null;

        if (other.gameObject.layer == LayerMask.NameToLayer("Water")) {
            particles = Instantiate(WaterImpactParticles);
            particles.transform.position = transform.position;
            Destroy(gameObject);
        }

        if (other.gameObject.tag == Tags.PLAYER && Source != SpawnSource.Player) {
            HealthScript targetHealth = other.gameObject.GetComponent<HealthScript>();
            targetHealth.TakeDamage(10f);

            particles = Instantiate(CannonballImpactParticles);
            particles.transform.position = transform.position;
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == Tags.ENEMY && Source != SpawnSource.EnemyHeavyWarship && Source != SpawnSource.EnemyLightFrigate) {
            HealthScript targetHealth = other.gameObject.GetComponent<HealthScript>();
            targetHealth.TakeDamage(10f);

            particles = Instantiate(CannonballImpactParticles);
            particles.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    public void Update () {
        Lifetime -= Time.deltaTime;

        if (Lifetime < 0) Destroy(gameObject);
	}

    public float Damage {
        set { _damage = value; }
        get { return _damage; }
    }
}
