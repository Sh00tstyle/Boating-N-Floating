using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    public float Health;

    private float _startingHealth;
    private float _remainingRegenTime;
    private float _healingAmtPerSec;

    private float _lastRegenTick;

    private float _additionalHP;

    public void Awake() {
        _startingHealth = Health;
        _lastRegenTick = Time.time;
    }

    public void Update() {
        if(_remainingRegenTime > 0f && Time.time - _lastRegenTick >= 1f) { //ensuring you will only get one heal per sec
            GainHealth(_healingAmtPerSec);
            _lastRegenTick = Time.time;
        }

        _remainingRegenTime -= Time.deltaTime;

        //DEBUG

        if(Input.GetKeyDown(KeyCode.N)) {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float Damage) {
        Health -= Damage;

        if(Health <= 0) {
            if(gameObject.tag == Tags.PLAYER) {
                transform.position = Vector3.zero;
                GainHealth(100);

                GetComponent<ReferenceHolder>().Menus.ActivateMenu(MenuManager.Menu.Death);
            } else if(gameObject.tag == Tags.ENEMY) {
                gameObject.SetActive(false);

                //TODO Spawn crate
            }
        }
    }

    public void GainHealth(float Heal) {
        Health += Heal;

        if(Health > _startingHealth) {
            Health = _startingHealth;
        }
    }

    public void ApplyRegeneration(float duration, float amountPerSec) {
        if (_remainingRegenTime <= 0f) _remainingRegenTime = 0f;

        _remainingRegenTime += duration;
        _healingAmtPerSec = amountPerSec;
    }

    public void AddTempHP(float additionalHP) {
        _startingHealth += additionalHP;
        _additionalHP = additionalHP;
        GainHealth(additionalHP);
    }

    public void RemoveTempHP() {
        _startingHealth -= _additionalHP;

        if (Health > _startingHealth) Health = _startingHealth;
    }

    public float RemainingRegenTime {
        get { return _remainingRegenTime; }
    }

    public float StartingHealth {
        get { return _startingHealth; }
    }
}
