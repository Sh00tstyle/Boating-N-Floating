using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballSpawnerEnemy : MonoBehaviour {


    public GameObject cannonball;
    public float cannonballSpeed;
    public float cannonballLifetime;
    public bool isRight;

    public void Update() {
        /*if (Input.GetKeyDown(KeyCode.Q) && !isRight) {
            Debug.Log("Shoot left!");
            Fire();
        } else if (Input.GetKeyDown(KeyCode.E) && isRight) {
            Debug.Log("Shoot right!");
            Fire();
        }*/
    }

    public void Fire(Vector3 shipVelocity) {
        GameObject cannonballInstance = Instantiate(cannonball);
        CannonballScript ballInfo = cannonball.GetComponent<CannonballScript>();
        ballInfo.Source = SpawnSource.EnemyHeavyWarship;
        ballInfo.Lifetime = cannonballLifetime;

        cannonballInstance.transform.position = transform.position;
        cannonballInstance.transform.rotation = transform.rotation;

        Rigidbody rigidbody = cannonballInstance.GetComponent<Rigidbody>();

        if (isRight) rigidbody.AddRelativeForce(new Vector3(0, 0, -cannonballSpeed) + shipVelocity);
        else rigidbody.AddRelativeForce(new Vector3(0, 0, cannonballSpeed) + shipVelocity);
    }
}
