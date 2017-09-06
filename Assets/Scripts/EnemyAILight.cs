using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAILight : MonoBehaviour {

    public Transform Target;
    private BoxCollider TargetCollider;

    private NavMeshAgent agent;
    private CannonballSpawner cannons;
    private HealthScript health;

    public Collider RamCollider;

    public float Speed;
    public float Acceleration;
    public float RotationSpeed;
    [Tooltip("The Amount of seconds the ship remains idle after ramming.")]
    public float RamCooldown;

    private float RamCooldownTimer;
    private Vector3 AfterRamPosition;

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
        agent.acceleration = Acceleration;

        health = GetComponent<HealthScript>();

        TargetCollider = Target.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 distance = Target.position - transform.position;

        if (agent.isActiveAndEnabled == false && RamCooldownTimer <= 0) agent.enabled = true;
        if (agent.isActiveAndEnabled == true) agent.destination = Target.position;

        if (RamCollider.bounds.Intersects(TargetCollider.bounds) && RamCooldownTimer <= 0) {
            RamCooldownTimer = RamCooldown;
            AfterRamPosition = transform.position - (3 * distance.normalized);
            AfterRamPosition.y = transform.position.y;
            agent.enabled = false;
        } 

        if(RamCooldownTimer > 0) {
            RamCooldownTimer -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, AfterRamPosition, Time.deltaTime);
            if (RamCooldownTimer < 0) RamCooldownTimer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ram") {
            health.TakeDamage(20);
        }
        else if (collision.gameObject.tag == "Cannonball") {
            CannonballScript ball = collision.gameObject.GetComponent<CannonballScript>();

            if (ball.Source == SpawnSource.Player) {
                health.TakeDamage(20);
                Destroy(collision.gameObject);
            }
            else {
                print("Friendly fire you cunt");
            }
        }
    }
}
