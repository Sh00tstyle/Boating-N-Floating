using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShifting : MonoBehaviour {

    public Vector3 camPosLeft;
    public Vector3 camPosRight;

    public float lerpFactor;
    public float rotationAmount;

    private Vector3 _startingCamPos;
    private Vector3 _startingRotation;

    public enum Direction { Front, Left, Right };
    private Direction _currentDirection;
    private Direction _targetDirection;
    private Direction _previousDirection;

    private WorldHudManager _hudManager;

    private float _incrementor;

    public void Awake() {
        _startingCamPos = transform.localPosition;
        _startingRotation = transform.localEulerAngles;

        _hudManager = transform.parent.gameObject.GetComponentInChildren<WorldHudManager>(); //Bad

        _currentDirection = Direction.Front;
        _hudManager.DisableIndicator(true);
        _hudManager.DisableIndicator(false);

        _incrementor = 0f;
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            SetTargetDirection(Direction.Right);
        } else if(Input.GetKeyDown(KeyCode.Q)) {
            SetTargetDirection(Direction.Left);
        }
    }

    public void FixedUpdate() {
        HandleDirectionCamera();
    }

    private void HandleDirectionCamera() {
        switch (_targetDirection) {
            case Direction.Front:
                _incrementor -= lerpFactor;

                _incrementor = Mathf.Clamp01(_incrementor);

                if (_previousDirection == Direction.Left && _incrementor > 0f) {
                    transform.localPosition = Vector3.Lerp(_startingCamPos, camPosLeft, _incrementor);
                    transform.Rotate(new Vector3(0, rotationAmount / (1f / lerpFactor), 0));
                } else if (_previousDirection == Direction.Right && _incrementor > 0f) {
                    transform.localPosition = Vector3.Lerp(_startingCamPos, camPosRight, _incrementor);
                    transform.Rotate(new Vector3(0, -rotationAmount / (1f / lerpFactor), 0));
                }

                if (_incrementor == 0f) _currentDirection = Direction.Front;
                _hudManager.DisableIndicator(true);
                _hudManager.DisableIndicator(false);
                break;

            case Direction.Right:
                if (_currentDirection == Direction.Front) _currentDirection = Direction.Right;
                _hudManager.ActivateIndicator(true);
                _hudManager.DisableIndicator(false);

                if (_currentDirection == Direction.Left) {
                    _incrementor -= lerpFactor;
                } else {
                    _incrementor += lerpFactor;
                }

                _incrementor = Mathf.Clamp01(_incrementor);

                if (_incrementor == 0f) _currentDirection = Direction.Front;

                if (_currentDirection == Direction.Right) transform.localPosition = Vector3.Lerp(_startingCamPos, camPosRight, _incrementor);
                else if(_currentDirection == Direction.Left) transform.localPosition = Vector3.Lerp(_startingCamPos, camPosLeft, _incrementor);

                if(_incrementor < 1f) transform.Rotate(new Vector3(0, rotationAmount / (1f / lerpFactor), 0));
                break;

            case Direction.Left:
                if (_currentDirection == Direction.Front) _currentDirection = Direction.Left;
                _hudManager.ActivateIndicator(false);
                _hudManager.DisableIndicator(true);

                if (_currentDirection == Direction.Right) {
                    _incrementor -= lerpFactor;
                } else {
                    _incrementor += lerpFactor;
                }

                _incrementor = Mathf.Clamp01(_incrementor);

                if (_incrementor == 0f) _currentDirection = Direction.Front;

                if (_currentDirection == Direction.Left) transform.localPosition = Vector3.Lerp(_startingCamPos, camPosLeft, _incrementor);
                else if (_currentDirection == Direction.Right) transform.localPosition = Vector3.Lerp(_startingCamPos, camPosRight, _incrementor);

                if (_incrementor < 1f) transform.Rotate(new Vector3(0, -rotationAmount / (1f / lerpFactor), 0));
                break;

            default:
                break;
        }

        transform.localEulerAngles = new Vector3(25f, transform.localEulerAngles.y, 0f);
    }

    public void SetTargetDirection(Direction targetDirection) {
        _previousDirection = _currentDirection;

        switch(_currentDirection) {
            case Direction.Left:
                if (targetDirection == Direction.Right) _targetDirection = Direction.Front;
                break;

            case Direction.Right:
                if (targetDirection == Direction.Left) _targetDirection = Direction.Front;
                break;

            case Direction.Front:
                _targetDirection = targetDirection;
                break;

            default:
                break;
        }
    }

    public Direction CurrentDirection {
        get { return _currentDirection; }
    }
}
