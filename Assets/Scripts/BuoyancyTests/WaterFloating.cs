using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloating : MonoBehaviour {

    public float waterLevel, floatHeight;
    public Vector3 buoyancyCentreOffset;
    public float bounceDamp;

    private Rigidbody _rigidbody;

    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate() {
        Vector3 actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
        float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

        if (forceFactor > 0f) {
            Vector3 uplift = -Physics.gravity * (forceFactor - _rigidbody.velocity.y * bounceDamp);
            _rigidbody.AddForceAtPosition(uplift, actionPoint);
        }
    }
}