using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAI : MonoBehaviour {

    private AISpawner controller;
    private Rigidbody rb;
    private Vector3 velocity;
    public float MaxSpeed;
    public float MinSpeed;
    public float MaxSteeringForce;

    public float AllignmentFactor;
    public float AllignmentRange;
    public float CohesionFactor;
    public float CohesionRange;
    public float SeperationFactor;
    public float SeperationRange;

    public int TargetFactor;

    public float AttackRange;
    public float FireCooldown;
    private float fireCooldownTimer;

    public bool debugMode = false;

    private Transform Target;
    public Transform Player;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        Target = Player;
	}
	
	// Update is called once per frame
	void Update () {
        //velocity = Vector3.zero;

        Vector3 allignment = AllignmentFactor * calculateAlignment();
        Vector3 cohesion = CohesionFactor * calculateCohesion();
        Vector3 seperation = SeperationFactor * calculateSeperation();

        velocity += allignment;
        velocity += cohesion;
        velocity += seperation;

        //velocity.Normalize();

        velocity.y = 0;
        velocity *= Time.deltaTime;

        if (velocity.magnitude > MaxSpeed) velocity = velocity.normalized * MaxSpeed;
        if (velocity.magnitude < MinSpeed) velocity = velocity.normalized * MinSpeed;

        rb.velocity = velocity * 5;
        //print(rb.velocity);
        //transform.position += velocity;

        transform.rotation = Quaternion.LookRotation(rb.velocity);

        //if (transform.position.y != 0) transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        checkAttack();
        if (fireCooldownTimer > 0) {
            fireCooldownTimer -= Time.deltaTime;
            if (fireCooldownTimer < 0) fireCooldownTimer = 0;
        }

        if(debugMode) {
            Debug.DrawRay(transform.position, velocity * 10, Color.white);
            Debug.DrawRay(transform.position, allignment * 100, Color.green);
            Debug.DrawRay(transform.position, cohesion * 100, Color.blue);
            Debug.DrawRay(transform.position, seperation * 100, Color.red);
        }

    }

    private Vector3 calculateAlignment() {
        Vector3 averageVelocity = Vector3.zero;
        int count = 0;

        foreach(GameObject agent in controller.agents) {
            Vector3 agentVel = agent.GetComponent<FlockingAI>().velocity;
            Vector3 distance = agent.transform.position - transform.position;

            if ((distance.magnitude > 0) && (distance.magnitude < AllignmentRange)) {
                averageVelocity += agentVel;
                count++;
            }
        }

        Vector3 targetDistance = Target.position - transform.position;
        averageVelocity += targetDistance.normalized * (MaxSpeed * TargetFactor);

        averageVelocity /= count + TargetFactor;

        // As long as the vector is greater than 0
        if (averageVelocity.magnitude > 0) {
            // Implement Reynolds: Steering = Desired - Velocity
            averageVelocity.Normalize();
            averageVelocity *= MaxSpeed;
            Vector3 steer = averageVelocity - velocity;
            if (steer.magnitude > MaxSteeringForce) steer = steer.normalized * MaxSteeringForce;
            return steer;
        }
        else {
            return Vector3.zero;
        }
    }

    private Vector3 calculateCohesion() {
        Vector3 averagePosition = Vector3.zero;
        int numberOfNeighbors = 0;

        Vector3 closestAgent = Vector3.zero;

        foreach (GameObject agent in controller.agents) {
            Vector3 agentPos = agent.transform.position;
            Vector3 distance = agentPos - transform.position;
            if (closestAgent == Vector3.zero) closestAgent = agentPos;

            Vector3 distanceToClosest = closestAgent - transform.position;
            if(distance.magnitude < CohesionRange && distance.magnitude > 0) {
                averagePosition += agentPos;
                numberOfNeighbors++;

            }           
            if (distance.magnitude < distanceToClosest.magnitude) closestAgent = agentPos;
        }

        if (numberOfNeighbors > 0) {
            averagePosition /= numberOfNeighbors;
            return seek(averagePosition);
        }
        else {
            return seek(closestAgent);
        }
    }

    private Vector3 calculateSeperation() {

        Vector3 steer = new Vector3(0, 0, 0);
        int count = 0;

        // For every agent check if it's too close
        foreach(GameObject agent in controller.agents) {
            Vector3 distance = agent.transform.position - transform.position;

            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((distance.magnitude > 0) && (distance.magnitude < SeperationRange)) {

                // Calculate vector pointing away from neighbor
                Vector3 diff = transform.position - agent.transform.position;
                diff.Normalize();
                diff /= distance.magnitude;// Weight by distance
                steer += diff;
                count++; 
            }
        }
        // Average -- divide by how many
        if (count > 0) {
            steer /= count;
        }

        // As long as the vector is greater than 0
        if (steer.magnitude > 0) {
            // Implement Reynolds: Steering = Desired - Velocity
            steer.Normalize();
            steer *= MaxSpeed;
            steer -= velocity;
            if (steer.magnitude > MaxSteeringForce) steer = steer.normalized * MaxSteeringForce;
        }
        return steer;
    }

    private Vector3 seek(Vector3 target) {
        Vector3 desired = target - transform.position;  // A vector pointing from the position to the target
                                                          
        desired.Normalize();
        desired *= MaxSpeed;

        // Steering = Desired minus Velocity
        Vector3 steer = desired - velocity;
        if (steer.magnitude > MaxSteeringForce) steer = steer.normalized * MaxSteeringForce;  // Limit to maximum steering force
        return steer;
    }

    public void checkAttack() {
        Vector3 targetDistance = Target.position - transform.position;
        if(targetDistance.magnitude < AttackRange && fireCooldownTimer <= 0) {
            CannonballSpawnerEnemy[] cannons = GetComponentsInChildren<CannonballSpawnerEnemy>();

            foreach (CannonballSpawnerEnemy c in cannons) {
                c.Fire(velocity);
            }
            fireCooldownTimer = FireCooldown;
        }
    }

    public Vector3 Velocity {
        get { return velocity; }
        set { velocity = value; }
    }

    public void SetController(AISpawner aic) {
        controller = aic;
    }
}
