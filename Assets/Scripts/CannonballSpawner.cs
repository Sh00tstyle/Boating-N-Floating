using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballSpawner : MonoBehaviour {

    public GameObject cannonball;
    public float cannonballSpeed;
    public bool isRight;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Q) && !isRight || Input.GetKeyDown(KeyCode.E) && isRight) {
            CreateBall();
        }
    }

    public void CreateBall() {
        GameObject cannonballInstance = Instantiate(cannonball);
        cannonballInstance.transform.position = transform.position;
        cannonballInstance.transform.rotation = GetComponentInParent<ShipMovement>().gameObject.transform.rotation;

        Rigidbody rigidbody = cannonballInstance.GetComponent<Rigidbody>();
        Rigidbody body = GetComponentInParent<Rigidbody>();
        rigidbody.velocity = body.velocity;

        if (isRight) rigidbody.AddRelativeForce(new Vector3(0, 0, -cannonballSpeed * Random.Range(0.7f, 1f)));
        else rigidbody.AddRelativeForce(new Vector3(0, 0, cannonballSpeed * Random.Range(0.7f, 1f)));
    }
}
