using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPatrol : Goal {

    public float SecondsTillNextChoose;
    private float timer;

    public void UpdateState() {

        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer < 0) timer = 0;
        }
        else {
            SetRandomDestination();
        }
    }

    public void SetRandomDestination() {
        Behaviour.SetTargetDestination(new Vector3(Behaviour.Spawner.transform.position.x + Random.Range(-Behaviour.Spawner.SpawnRange, Behaviour.Spawner.SpawnRange), 0, Behaviour.Spawner.transform.position.z + Random.Range(-Behaviour.Spawner.SpawnRange, Behaviour.Spawner.SpawnRange)));
        timer = SecondsTillNextChoose;
    }
}
