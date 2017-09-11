using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour {

    public GameObject[] agents;
    public int FlockSize;
    public GameObject AgentPrefab;
    public Transform Target;

	// Use this for initialization
	void Start () {
        agents = new GameObject[FlockSize];
        for (int i = 0; i < FlockSize; i++) {
            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-5f, 5f), 0, transform.position.z + Random.Range(-5f, 5f));
            Vector3 spawnVelocity = new Vector3(Random.Range(-6f, 6f), 0, Random.Range(-6f, 6f));

            GameObject agent = Instantiate(AgentPrefab, spawnPosition, transform.rotation);
            FlockingAI f = agent.GetComponent<FlockingAI>();
            f.SetController(this);
            f.Velocity = spawnVelocity;
            f.Target = Target;
            agents[i] = agent;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.DrawRay(Vector3.zero, new Vector3(6, 0, 6));
	}
}
