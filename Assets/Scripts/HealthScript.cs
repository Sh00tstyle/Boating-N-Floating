using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    public float Health;
    public float MaxRegenTime;

    private float _startingHealth;
    private float _remainingRegenTime;
    private float _healingAmtPerSec;

    private float _lastRegenTick;

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
        //DEBUG AND PROFILE
        /*if(gameObject.tag == Tags.PLAYER) {
            GameObject.Find("Analysis").GetComponent<Analysis>().playerinfo.DamageTaken++;
        }*/

        Health -= Damage;

        if(Health <= 0) {
            if(gameObject.tag == Tags.PLAYER) {
                transform.position = Vector3.zero;
                GainHealth(100);
            } else {
                gameObject.SetActive(false);
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

        if (_remainingRegenTime > MaxRegenTime) _remainingRegenTime = MaxRegenTime;
    }

    public float RemainingRegenTime {
        get { return _remainingRegenTime; }
    }

    public float StartingHealth {
        get { return _startingHealth; }
    }
}
