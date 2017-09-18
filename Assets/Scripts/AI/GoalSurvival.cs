using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSurvival : Goal {

    private CrateManagerScript CrateManager;
    private HealthScript health;

    public float RegenDuration;
    public float RegenPerSecond;

    private void Start() {
        health = GetComponent<HealthScript>();
    }

    public void UpdateState() {
        //First try to go for the nearest Crate
        Transform nearestCrate = CrateManager.GetClosestCrate(transform.position).transform;
        Vector3 deltaVec = nearestCrate.position - transform.position;

        if (deltaVec.magnitude < Behaviour.CrateDetectionRange) {
            Behaviour.SetTargetDestination(nearestCrate.position);

            if (deltaVec.magnitude < Behaviour.CratePickupRange) {
                CrateManager.RemoveCrate(nearestCrate.gameObject);
                Destroy(nearestCrate.gameObject);
                health.ApplyRegeneration(RegenDuration, RegenPerSecond);
            }
        }
        //If all the crates are out of range, try to hide in the middle of the swarm
        else {
            Behaviour.SetTargetDestination(Behaviour.Flocking.calculateLocalSwarmCenter());
        }
    }

    public void setCrateManager(CrateManagerScript cms) {
        CrateManager = cms;
    }
}
