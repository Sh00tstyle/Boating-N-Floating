using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlockingAI), typeof(GoalSurvival), typeof(GoalEliminateThreat))]
[RequireComponent(typeof(GoalPatrol))]
public class BehaviourAI : MonoBehaviour {

    private enum Goals {
        Survival,
        EliminateThreat,
        Patrol,
    }

    private Goals currentGoal;

    private FlockingAI flocking;
    private AISpawner spawner;
    private HealthScript health;

    private GoalSurvival survival;
    private GoalEliminateThreat eliminate;
    private GoalPatrol patrol;

    public Transform Player;
    public CrateManagerScript CrateManager;

    public float HealthDivider;

    public float PlayerDetectionRange;
    public float CrateDetectionRange;
    public float CratePickupRange;

    public float AttackRange;
    public float FireCooldown;

    // Use this for initialization
    void Start () {
        flocking = GetComponent<FlockingAI>();
        health = GetComponent<HealthScript>();
        SetTargetDestination(Player.position);


        survival = GetComponent<GoalSurvival>();
        eliminate = GetComponent<GoalEliminateThreat>();
        patrol = GetComponent<GoalPatrol>();
        
        survival.setParent(this);
        survival.setCrateManager(CrateManager);
        eliminate.setParent(this);
        patrol.setParent(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.T)) health.TakeDamage(26);

        determineState();
        updateCurrentState();
    }

    private void determineState() {
        Vector3 distance = Player.transform.position - transform.position;

        if (health.Health < (health.StartingHealth / HealthDivider)) {
            if(currentGoal != Goals.Survival) {
                currentGoal = Goals.Survival;
                print("Survival Goal activated");
            }
            return;
        }

        if (distance.magnitude < PlayerDetectionRange) {
            if(currentGoal != Goals.EliminateThreat) {
                currentGoal = Goals.EliminateThreat;
                print("ElimnateThreat Goal activated");
            }
            return;    
        }

        if (currentGoal != Goals.Patrol) {
            currentGoal = Goals.Patrol;
            print("Patrol Goal activated");
            return;
        }

        //print("No state switch occured");       
    }

    private void updateCurrentState() {
        switch  (currentGoal) {
            case Goals.Survival:
                survival.UpdateState();
                break;
            case Goals.EliminateThreat:
                eliminate.UpdateState();
                break;
            case Goals.Patrol:
                patrol.UpdateState();
                break;
            default:
                break;
        }
    }

    public void SetTargetDestination(Vector3 destination) {
        flocking.SetTarget(destination);
    }

    public void SetSpawner(AISpawner sp) {
        spawner = sp;
    }

    public AISpawner Spawner {
        get { return spawner; }
    }

    public FlockingAI Flocking {
        get { return flocking; }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            //health.TakeDamage(20);
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
