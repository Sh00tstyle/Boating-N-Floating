using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourAI : MonoBehaviour {

    private HealthScript health;

	// Use this for initialization
	void Start () {
        health = GetComponent<HealthScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.T)) health.TakeDamage(25);
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            health.TakeDamage(20);
        }
        else if (collision.gameObject.tag == "Cannonball") {
            CannonballScript ball = collision.gameObject.GetComponent<CannonballScript>();

            if (ball.Source == SpawnSource.Player) {
                health.TakeDamage(20);
                Destroy(collision.gameObject);
            }
            else {
                //print("Friendly fire you cunt");
            }
        }
    }
}
