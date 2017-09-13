﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePickupScript : MonoBehaviour {

    public float regenTimePerCrate;
    public float regenAmtPerSec;

    public CrateManagerScript crateManager;

    public void OnTriggerStay(Collider other) {
        if(other.tag == Tags.CRATE) {
            crateManager.RemoveCrate(other.gameObject);
            Destroy(other.gameObject);

            HealthScript health = null;

            if(gameObject.GetComponent<HealthScript>()) {
                health = gameObject.GetComponent<HealthScript>();
            } else if(gameObject.GetComponentInParent<HealthScript>()) {
                health = gameObject.GetComponentInParent<HealthScript>();
            }

            if (health != null) {
                health.ApplyRegeneration(regenTimePerCrate, regenAmtPerSec);
            }
        }
    }
}
