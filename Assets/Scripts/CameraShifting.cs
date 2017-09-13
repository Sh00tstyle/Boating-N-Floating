using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShifting : MonoBehaviour {

    public Vector3 camPosLeft;
    public Vector3 camPosRight;

    public float lerpFactor;

    private Vector3 _startingCamPos;

    private enum Direction { None, Left, Right };
    private Direction _currentDirection;

    private float _incrementor;

    public void Awake() {
        _startingCamPos = transform.localPosition;

        _currentDirection = Direction.None;

        _incrementor = 0f; //was 0.5f
    }

    public void FixedUpdate() {
        /*if (Input.GetKey(KeyCode.E) && _incrementor < 1f) {
            _incrementor += lerpFactor;
            transform.Rotate(new Vector3(0, 55f / (0.5f / lerpFactor), 0));
            transform.localEulerAngles = new Vector3(25f, transform.localEulerAngles.y, 0f);
        } else if (Input.GetKey(KeyCode.Q) && _incrementor > 0f) {
            _incrementor -= lerpFactor;
            transform.Rotate(new Vector3(0, -55f / (0.5f / lerpFactor), 0));
            transform.localEulerAngles = new Vector3(25f, transform.localEulerAngles.y, 0f);
        }

        _incrementor = Mathf.Clamp01(_incrementor);

        transform.localPosition = Vector3.Lerp(camPosLeft, camPosRight, _incrementor);*/

        if(Input.GetKey(KeyCode.E)) {
            if(IsRight()) {
                _incrementor += lerpFactor;
            } else {
                _incrementor -= lerpFactor;

                if (_incrementor <= 0f) {
                    _currentDirection = Direction.Right;
                }
            }

            if (_incrementor <= 1f) transform.Rotate(new Vector3(0, 55f / (1f / lerpFactor), 0));
        } else if(Input.GetKey(KeyCode.Q)) {
            if (IsRight()) {
                _incrementor -= lerpFactor;

                if(_incrementor <= 0f) {
                    _currentDirection = Direction.Left;
                }
            } else {
                _incrementor += lerpFactor;
            }

            if(_incrementor <= 1f) transform.Rotate(new Vector3(0, -55f / (1f / lerpFactor), 0));
        }

        _incrementor = Mathf.Clamp01(_incrementor);
        transform.localEulerAngles = new Vector3(25f, transform.localEulerAngles.y, 0f);

        if (IsRight()) transform.localPosition = Vector3.Lerp(_startingCamPos, camPosRight, _incrementor);
        else transform.localPosition = Vector3.Lerp(_startingCamPos, camPosLeft, _incrementor);
    }

    public bool IsRight() {
        //return _incrementor >= 0.5f;

        return _currentDirection != Direction.Left;
    }
}
