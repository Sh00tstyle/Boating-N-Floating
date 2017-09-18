using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateScript : MonoBehaviour {

    public float regenTimePerCrate;
    public float regenAmtPerSec;

    public CrateManagerScript crateManager;

    private PowerupActivationScript _powerupActivator;

    public void Awake() {
        _powerupActivator = GetComponentInParent<PowerupActivationScript>();
    }

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
        } else if(other.tag == Tags.POWERUP) {
            PowerupInfo info = other.gameObject.GetComponent<PowerupInfo>();
            _powerupActivator.ActivatePowerup(info);

            Destroy(other.gameObject);
        }
    }
}
