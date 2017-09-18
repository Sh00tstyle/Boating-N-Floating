using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEliminateThreat : Goal {

    public float RotationSpeed;

    public GameObject cannonsLeft;
    public GameObject cannonsRight;

    private Transform distanceCheckPointRight;
    private Transform distanceCheckPointLeft;

    private float fireCooldownTimer;

    private void Start() {
        distanceCheckPointLeft = cannonsLeft.GetComponentInChildren<CannonballSpawnerEnemy>().transform;
        distanceCheckPointRight = cannonsRight.GetComponentInChildren<CannonballSpawnerEnemy>().transform;
    }

    public void UpdateState() {
        Behaviour.SetTargetDestination(Behaviour.Player.position);

        checkAttack();

        if (fireCooldownTimer > 0) {
            fireCooldownTimer -= Time.deltaTime;
            if (fireCooldownTimer < 0) fireCooldownTimer = 0;
        }
    }

    public void checkAttack() {
        Vector3 targetDistance = Behaviour.Player.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(targetDistance);
        targetRotation *= Quaternion.Euler(0, 90, 0); // this adds a 90 degrees Y rotation

        if (targetDistance.magnitude < Behaviour.AttackRange) {
            if (!Behaviour.Flocking.FireMode) {
                Behaviour.Flocking.FireMode = true;
                Behaviour.Flocking.Velocity = Vector3.zero;
            }

            Ray rayLeft = new Ray(transform.position + new Vector3(0,1,0), -transform.right);
            Ray rayRight = new Ray(transform.position + new Vector3(0, 1, 0), transform.right);

            RaycastHit hitLeft;
            RaycastHit hitRight;

            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), -transform.right * Behaviour.AttackRange, Color.cyan);
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.right * Behaviour.AttackRange, Color.cyan);


            if (Physics.Raycast(rayLeft, out hitLeft, Behaviour.AttackRange)) {
                if (fireCooldownTimer <= 0 && hitLeft.collider.tag == Tags.PLAYER) {
                    transform.rotation = Quaternion.LookRotation(targetDistance) * Quaternion.Euler(0, Random.Range(80, 100), 0);
                    CannonballSpawnerEnemy[] cannons = cannonsLeft.GetComponentsInChildren<CannonballSpawnerEnemy>();

                    foreach (CannonballSpawnerEnemy c in cannons) {
                        c.Fire(Behaviour.Flocking.Velocity);
                    }
                    fireCooldownTimer = Behaviour.FireCooldown;
                    return;              
                }
                /*else if (hitLeft.collider.tag == Tags.ENEMY) {
                    //transform.position = Vector3.Slerp(transform.position, transform.position + (transform.forward.normalized * 5), Time.deltaTime * RotationSpeed);
                }*/
            }

            if(Physics.Raycast(rayRight, out hitRight, Behaviour.AttackRange)) {
                if (fireCooldownTimer <= 0 && hitRight.collider.tag == Tags.PLAYER) {
                    transform.rotation = Quaternion.LookRotation(targetDistance) * Quaternion.Euler(0, Random.Range(-80, -100), 0);
                    CannonballSpawnerEnemy[] cannons = cannonsRight.GetComponentsInChildren<CannonballSpawnerEnemy>();

                    foreach (CannonballSpawnerEnemy c in cannons) {
                        c.Fire(Behaviour.Flocking.Velocity);
                    }
                    fireCooldownTimer = Behaviour.FireCooldown;
                    return;
                }
                /*else if(hitRight.collider.tag == Tags.ENEMY) {
                    //transform.position = Vector3.Slerp(transform.position, transform.position + (transform.forward.normalized * 5), Time.deltaTime * RotationSpeed);
                }*/
            }

            if (fireCooldownTimer <= 0) {
                Vector3 distanceLeftPlayer = Behaviour.Player.position - distanceCheckPointLeft.position;
                Vector3 distanceRightPlayer = Behaviour.Player.position - distanceCheckPointRight.position;

                //Turn left
                if (distanceLeftPlayer.magnitude < distanceRightPlayer.magnitude) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 15, 0) * transform.rotation, Time.deltaTime * RotationSpeed);
                }
                //Turn right
                else if (distanceLeftPlayer.magnitude > distanceRightPlayer.magnitude) {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -15, 0) * transform.rotation, Time.deltaTime * RotationSpeed);
                }
                //If both are the same lenght turn left
                else {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 15, 0) * transform.rotation, Time.deltaTime * RotationSpeed);
                }
                //transform.position = Vector3.Slerp(transform.position, Behaviour.Player.position - (targetDistance.normalized * (Behaviour.AttackRange * 0.85F)), Time.deltaTime * RotationSpeed);
            }
        }
        else {
            if(Behaviour.Flocking.FireMode) Behaviour.Flocking.FireMode = false;
        }
    }
}
