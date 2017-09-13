using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour {

    public void OnTriggerStay(Collider other) {
        if(other.tag == Tags.CRATE && Input.GetKeyDown(KeyCode.Space)) {
            Destroy(other.gameObject);
        }
    }
}
