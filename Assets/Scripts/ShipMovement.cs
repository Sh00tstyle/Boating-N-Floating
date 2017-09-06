using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public float rotationSpeed;

    private Rigidbody _rigidbody;
    private Transform _rotator;

    private Transform _compass;

    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _rotator = GetComponentInParent<Transform>();

        _compass = GetComponentInChildren<InteractionScript>().gameObject.transform;
    }

    public void FixedUpdate() {
        //Replace keys with mapped controls in unity

        if(Input.GetKey(KeyCode.W)) {
            _rigidbody.AddRelativeForce(new Vector3(0, speed, 0));
        } else if(Input.GetKey(KeyCode.S)) {
            _rigidbody.AddRelativeForce(new Vector3(0, -speed, 0));
        }

        if(Input.GetKey(KeyCode.A)) {
            _rotator.Rotate(new Vector3(rotationSpeed, 0, 0));
            _compass.Rotate(new Vector3(0, 0, -rotationSpeed));
        } else if(Input.GetKey(KeyCode.D)) {
            _rotator.Rotate(new Vector3(-rotationSpeed, 0, 0));
            _compass.Rotate(new Vector3(0, 0, rotationSpeed));
        }
    }
}
