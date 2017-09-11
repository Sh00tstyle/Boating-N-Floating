using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAllParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
            foreach (Transform child in transform)
            {
               child.gameObject.GetComponent<ParticleSystem>().Play();

            }

    }
}
