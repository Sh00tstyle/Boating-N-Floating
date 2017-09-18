using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

    public float maxSpeed;
    public float speedSteps;
    public float acceleration;
    public float rotationSpeed;

    public float minFov;
    public float maxFov;

    private float _currentSpeed;
    private float _oldMaxSpeed;

    private Rigidbody _rigidbody;
    private Transform _rotator;

    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _rotator = GetComponentInParent<Transform>();
    }

    public void FixedUpdate() {
        ProcessMovementInput();
        ProcessRotationInput();
    }

    private void ProcessMovementInput() {
        //Replace keys with mapped controls in unity

        if(Input.GetKey(KeyCode.W)) {
            _currentSpeed += speedSteps;
        } else if (Input.GetKey(KeyCode.S)) {
            _currentSpeed -= speedSteps;  
        }

        Camera.main.fieldOfView = minFov + (_currentSpeed / maxSpeed * (maxFov - minFov));

        _currentSpeed = Mathf.Clamp(_currentSpeed, 0f, maxSpeed);

        if(_rigidbody.velocity.magnitude < _currentSpeed) {
            _rigidbody.AddRelativeForce(new Vector3(acceleration, 0, 0));
        } else if(_rigidbody.velocity.magnitude > _currentSpeed && _currentSpeed != 0) {
            _rigidbody.AddRelativeForce(new Vector3(-acceleration, 0, 0));
        }
    }

    private void ProcessRotationInput() {
        if (_currentSpeed < maxSpeed / 100f * 15f) return;

        if (Input.GetKey(KeyCode.A)) {
            _rotator.Rotate(new Vector3(0, -rotationSpeed, 0));
        } else if (Input.GetKey(KeyCode.D)) {
            _rotator.Rotate(new Vector3(0, rotationSpeed, 0));
        }
    }

    public float CurrentSpeed {
        get { return _currentSpeed; }
    }

    public void ChangeMaxSpeed(float speedModifier) {
        _oldMaxSpeed = maxSpeed;

        maxSpeed *= speedModifier;
    }

    public void ResetMaxSpeed() {
        maxSpeed = _oldMaxSpeed;
    }
}
