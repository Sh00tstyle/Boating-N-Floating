using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour {

    private CrateManager _manager;

    public void Awake() {
        _manager = GetComponentInParent<CrateManager>();
    }

    public void OnTriggerStay(Collider other) {
        if(other.tag == Tags.CRATE && Input.GetKeyDown(KeyCode.Space)) {
            Destroy(other.gameObject);
            _manager.AddCrate();
        }
    }
}
