using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShifting : MonoBehaviour {

    public Vector3 camPosLeft;
    public Vector3 camPosRight;

    public float lerpFactor;

    private Vector3 _startingCamPos;

    private float _incrementor;

    public void Awake() {
        _startingCamPos = transform.localPosition;

        _incrementor = 0.5f; //Default
    }

    public void Update() {
        if (Input.GetKey(KeyCode.E) && _incrementor < 1f) {
            _incrementor += lerpFactor;
            transform.Rotate(new Vector3(0, 55f / (0.5f / lerpFactor), 0));
            transform.localEulerAngles = new Vector3(25f, transform.localEulerAngles.y, 0f);
        } else if (Input.GetKey(KeyCode.Q) && _incrementor > 0f) {
            _incrementor -= lerpFactor;
            transform.Rotate(new Vector3(0, -55f / (0.5f / lerpFactor), 0));
            transform.localEulerAngles = new Vector3(25f, transform.localEulerAngles.y, 0f);
        }

        _incrementor = Mathf.Clamp01(_incrementor);

        transform.localPosition = Vector3.Lerp(camPosLeft, camPosRight, _incrementor);
    }

    public bool IsRight() {
        return _incrementor >= 0.5f;
    }
}
