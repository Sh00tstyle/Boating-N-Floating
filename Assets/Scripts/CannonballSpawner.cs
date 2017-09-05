using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballSpawner : MonoBehaviour {

    public GameObject cannonball;
    public float cannonballSpeed;
    public bool isRight;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Q) && !isRight) {
            Debug.Log("Shoot left!");
            CreateBall();
        } else if (Input.GetKeyDown(KeyCode.E) && isRight) {
            Debug.Log("Shoot right!");
            CreateBall();
        }
    }

    public void CreateBall() {
        GameObject cannonballInstance = Instantiate(cannonball);
        cannonballInstance.transform.position = transform.position;

        cannonballInstance.transform.rotation = GetComponentInParent<ShipMovement>().gameObject.transform.rotation;

        Rigidbody rigidbody = cannonballInstance.GetComponent<Rigidbody>();

        if (isRight) rigidbody.AddRelativeForce(new Vector3(0, 0, -cannonballSpeed));
        else rigidbody.AddRelativeForce(new Vector3(0, 0, cannonballSpeed));
    }
}
