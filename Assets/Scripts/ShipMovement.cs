using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

    [SerializeField]
    private float _speed;

    private Rigidbody _rigidbody;
    private Transform _rotator;

    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _rotator = GetComponentInParent<Transform>();
    }

    public void Update() {
        //Replace keys with mapped controls in unity

        if(Input.GetKey(KeyCode.W)) {
            _rigidbody.AddRelativeForce(new Vector3(0, _speed, 0));
        } else if(Input.GetKey(KeyCode.S)) {
            _rigidbody.AddRelativeForce(new Vector3(0, -_speed, 0));
        }

        Debug.Log(_rigidbody.velocity.magnitude);

        if(Input.GetKey(KeyCode.A)) {
            _rotator.Rotate(new Vector3(1, 0, 0));
        } else if(Input.GetKey(KeyCode.D)) {
            _rotator.Rotate(new Vector3(-1, 0, 0));
        }
    }
}
