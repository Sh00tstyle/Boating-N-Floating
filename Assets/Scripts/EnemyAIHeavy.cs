using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIHeavy : MonoBehaviour {

    public Transform Target;

    private NavMeshAgent agent;
    private CannonballSpawner cannons;
    private HealthScript health;

    public float Speed;
    public float Acceleration;
    public float RotationSpeed;
    public float FireCooldown;
    public float FireRange;

    private float fireCooldownTimer;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
        agent.acceleration = Acceleration;

        health = GetComponent<HealthScript>();
	}
	
	// Update is called once per frame
	void Update () {
        //Calculate distance between this enemy and the player
        Vector3 distance = Target.position - transform.position;

        //If he is further away then your Firerange move towards him
        if (distance.magnitude > FireRange) {

            if(!agent.isActiveAndEnabled) agent.enabled = true;
            agent.destination = Target.position;

            //While constantly rotating to face him
            Quaternion targetRotation = Quaternion.LookRotation(distance);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
        }
        //Else get into firing position and then fire
        else {
            if (agent.isActiveAndEnabled) agent.enabled = false;

            distance.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(distance);
            targetRotation *= Quaternion.Euler(0, 90, 0); // this adds a 90 degrees Y rotation

            //If your in firing position and you're not realoding atm -> FIRE ALL THE CANNONS
            if (transform.rotation == targetRotation && fireCooldownTimer <= 0) {
                CannonballSpawner[] cannons = GetComponentsInChildren<CannonballSpawner>();

                foreach(CannonballSpawner c in cannons) {
                    c.Fire();
                    print("pew");
                }
                fireCooldownTimer = FireCooldown;
            }
            //Else rotate some moar
            else {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
            }
        }

        //Firing cooldown timer
        if(fireCooldownTimer > 0) {
            fireCooldownTimer -= Time.deltaTime;
            if (fireCooldownTimer < 0) fireCooldownTimer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player") {
            health.TakeDamage(20);
        }
        else if(collision.gameObject.tag == "Cannonball") {
            CannonballScript ball = collision.gameObject.GetComponent<CannonballScript>();

            if(ball.Source == SpawnSource.Player) {
                health.TakeDamage(20);
                Destroy(collision.gameObject);
            }
            else {
                print("Friendly fire you cunt");
            }
        }
    }
}
