using UnityEngine;
using System.Collections;

public class ScreenShakeScript: MonoBehaviour {

    private float _shakeY;
    private float _shakeYSpeed;

    private float _shakeX;
    private float _shakeXSpeed;

    private void Start() {
        _shakeY = 0f;
        _shakeYSpeed = 0.7f;

        _shakeX = 0f;
        _shakeXSpeed = 0.7f;
    }

    private void Update() {
        Vector2 _newPosition = new Vector2(_shakeX, _shakeY);
        if (_shakeY < 0) {
            _shakeY *= _shakeYSpeed;
        }
        _shakeY = -_shakeY;

        if(_shakeX < 0) {
            _shakeX *= _shakeXSpeed;
        }
        _shakeX = -_shakeX;

        transform.Translate(_newPosition, Space.Self);
    }


    public void ApplyScreenShake() {
        _shakeX = 1f;
        _shakeY = 1f;

        _shakeXSpeed = 0.7f;
        _shakeYSpeed = 0.7f;
    }

    public void ApplyScreenShake(float shakeX, float shakeY, float shakeSpeedX, float shakeSpeedY) {
        _shakeX = shakeX;
        _shakeY = shakeY;

        _shakeXSpeed = shakeSpeedX;
        _shakeYSpeed = shakeSpeedY;
    }
}