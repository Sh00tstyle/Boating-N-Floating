using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassPositioning : MonoBehaviour {

    public Transform _playerTransform;

    public void Update() {
        transform.position = _playerTransform.position;
    }
}
