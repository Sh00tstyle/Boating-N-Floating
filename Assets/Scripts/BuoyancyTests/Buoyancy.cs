using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour {

    public float UpwardForce = 12.72f; // 9.81 is the opposite of the default gravity, which is 9.81. If we want the boat not to behave like a submarine the upward force has to be higher than the gravity in order to push the boat to the surface

    private bool _inWater = false;
    private Rigidbody _rigidbody;

    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collidier) {
        _inWater = true;
        _rigidbody.drag = 5f;
    }

    private void OnCollisionEnter(Collision collision) {
        _inWater = true;
        _rigidbody.drag = 5f;
    }

    private void OnCollisionExit(Collision collision) {
        _inWater = false;
        _rigidbody.drag = 0.05f;
    }

    void OnTriggerExit(Collider collidier) {
        _inWater = false;
        _rigidbody.drag = 0.05f;
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Vector3 force = transform.up * UpwardForce;
            _rigidbody.AddRelativeForce(force, ForceMode.Acceleration);
            Debug.Log("Upward force: " + force + " @" + Time.time);
        }
    }

    void FixedUpdate() {
        if (_inWater) {
            // apply upward force
            Vector3 force = transform.up * UpwardForce;
            _rigidbody.AddRelativeForce(force, ForceMode.Acceleration);
            Debug.Log("Upward force: " + force + " @" + Time.time);
        }
    }
}