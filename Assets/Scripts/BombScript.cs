using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

    public GameObject explosion;

    public int bombDamage;

    public void OnTriggerEnter(Collider other) {
        if (other.tag == Tags.GROUND || other.tag == Tags.COMPASS) return;

        //Add explosion

        if(other.gameObject.GetComponent<HealthScript>()) {
            other.gameObject.GetComponent<HealthScript>().TakeDamage(bombDamage);
        }

        Destroy(gameObject);
    }
}
