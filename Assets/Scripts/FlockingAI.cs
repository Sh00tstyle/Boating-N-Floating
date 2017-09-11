﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAI : MonoBehaviour {

    private AiController controller;
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

    public float AttackRange;
    public float FireCooldown;
    private float fireCooldownTimer;

    public bool debugMode = false;

    public Transform Target;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 allignment = AllignmentFactor * calculateAlignment();
        Vector3 cohesion = CohesionFactor * calculateCohesion();
        Vector3 seperation = SeperationFactor * calculateSeperation();

        velocity += allignment;
        velocity += cohesion;
        velocity += seperation;

        velocity.Normalize();

        transform.rotation = Quaternion.LookRotation(velocity);

        if (velocity.magnitude > MaxSpeed) velocity = velocity.normalized * MaxSpeed;
        if (velocity.magnitude < MinSpeed) velocity = velocity.normalized * MinSpeed;

        transform.position += velocity;

        checkAttack();
        if (fireCooldownTimer > 0) {
            fireCooldownTimer -= Time.deltaTime;
            if (fireCooldownTimer < 0) fireCooldownTimer = 0;
        }

        if(debugMode) {
            Debug.DrawRay(transform.position, velocity * 1000, Color.white);
            Debug.DrawRay(transform.position, allignment * 10000, Color.green);
            Debug.DrawRay(transform.position, cohesion * 10000, Color.blue);
            Debug.DrawRay(transform.position, seperation * 10000, Color.red);
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
        averageVelocity += targetDistance.normalized * MaxSpeed;

        averageVelocity /= count + 1;

        Vector3 alignment = averageVelocity - velocity;

        return alignment;
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

    public void SetController(AiController aic) {
        controller = aic;
    }
}