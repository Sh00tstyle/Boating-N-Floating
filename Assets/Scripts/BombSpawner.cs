using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour {

    public GameObject bomb;
    public float cooldown;
    public float damagePerBomb;

    private float _cooldownTimer;

    private float _oldCooldown;
    private float _oldBombDamage;

    public void Update() {
        _cooldownTimer -= Time.deltaTime;
    }

    public void DropBomb() {
        if (_cooldownTimer > 0f) return;

        GameObject newBomb = Instantiate(bomb);
        newBomb.transform.position = transform.position;

        BombScript bombScript = newBomb.GetComponent<BombScript>();
        bombScript.bombDamage = damagePerBomb;

        _cooldownTimer = cooldown;
    }

    public void ChangeCooldown(float cooldownModifier) {
        _oldCooldown = cooldown;
        cooldown *= cooldownModifier;
    }

    public void ResetCooldown() {
        cooldown = _oldCooldown;
    }

    public void ChangeDamage(float damageModifier) {
        _oldBombDamage = damagePerBomb;

        damagePerBomb *= damageModifier;
    }

    public void ResetDamage() {
        damagePerBomb = _oldBombDamage;
    }
}
