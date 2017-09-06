﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

    public float maxSpeed;
    public float speedSteps;
    public float acceleration;
    public float rotationSpeed;

    private float _currentSpeed;

    private Rigidbody _rigidbody;
    private Transform _rotator;

    private Transform _compass;

    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _rotator = GetComponentInParent<Transform>();

        _compass = GetComponentInChildren<InteractionScript>().gameObject.transform;
    }

    public void FixedUpdate() {
        ProcessMovementInput();
        ProcessRotationInput();

        Debug.Log("Current speed level " + _currentSpeed);
        Debug.Log("Velocity " + _rigidbody.velocity.magnitude);
    }

    private void ProcessMovementInput() {
        //Replace keys with mapped controls in unity

        if(Input.GetKey(KeyCode.W)) {
            _currentSpeed += speedSteps;
        } else if (Input.GetKey(KeyCode.S)) {
            _currentSpeed -= speedSteps;
        }
        
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, maxSpeed);

        if(_rigidbody.velocity.magnitude < _currentSpeed) {
            _rigidbody.AddRelativeForce(new Vector3(0, acceleration, 0));
            Debug.Log("Accelerating");
        } else if(_rigidbody.velocity.magnitude > _currentSpeed && _currentSpeed != 0) {
            _rigidbody.AddRelativeForce(new Vector3(0, -acceleration, 0));
            Debug.Log("Deccelerating");
        }
    }

    private void ProcessRotationInput() {
        if (Input.GetKey(KeyCode.A)) {
            _rotator.Rotate(new Vector3(rotationSpeed, 0, 0));
            _compass.Rotate(new Vector3(0, 0, -rotationSpeed));
        } else if (Input.GetKey(KeyCode.D)) {
            _rotator.Rotate(new Vector3(-rotationSpeed, 0, 0));
            _compass.Rotate(new Vector3(0, 0, rotationSpeed));
        }
    }
}
