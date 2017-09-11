using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballSpawnerPlayer : MonoBehaviour {

    public GameObject cannonball;
    public float cannonballSpeed;
    public float cannonballLifetime;
    public float cooldown;
    public bool isRight;

    private Rigidbody _shipRb;
    private float _cooldownTimer;

    public void Awake() {
        _shipRb = GetComponentInParent<Rigidbody>();

        _cooldownTimer = 0f;
    }

    public void Update() {
        if ((Input.GetKeyDown(KeyCode.Q) && !isRight || Input.GetKeyDown(KeyCode.E) && isRight) && _cooldownTimer <= 0f) {
            Fire();
        }

        _cooldownTimer -= Time.deltaTime;
    }

    public void Fire() {
        GameObject cannonballInstance = Instantiate(cannonball);
        CannonballScript ballInfo = cannonball.GetComponent<CannonballScript>();
        ballInfo.Source = SpawnSource.EnemyHeavyWarship;
        ballInfo.Lifetime = cannonballLifetime;

        cannonballInstance.transform.position = transform.position;
        cannonballInstance.transform.rotation = transform.rotation;

        Rigidbody rigidbody = cannonballInstance.GetComponent<Rigidbody>();

        if(_shipRb != null) {
            rigidbody.velocity = _shipRb.velocity;
        }

        if (isRight) rigidbody.AddRelativeForce(new Vector3(0, 0, -cannonballSpeed));
        else rigidbody.AddRelativeForce(new Vector3(0, 0, cannonballSpeed));

        _cooldownTimer = cooldown;
    }
}
