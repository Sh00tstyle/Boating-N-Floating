using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballSpawnerPlayer : MonoBehaviour {

    public GameObject cannonball;
    public GameObject scattershot;
    public GameObject cannonFireParticle;

    public float cannonballSpeed;
    public float cannonballLifetime;
    public float damagePerCannonball;

    public int scattershotAmt;
    public float damagePerScattershot;
    public float scattershotSpeed;
    public float maxScatterSpread;
    public float minScatterSpread;
    public float rotationFactor;

    public float cooldown;
    public bool isRight; 

    public float minDelay;
    public float maxDelay;

    public AudioClip[] _cannonSounds;

    public enum Weapons {None, Cannonball, Scattershot };
    private Weapons _currentWeapon;

    private Rigidbody _shipRb;
    private AudioSource _audioSource;
    private float _cooldownTimer;
    private float _delayTimer;
    private bool _hasFired;

    private float _oldCooldown;
    private float _oldCannonballDamage;
    private float _oldClustershotDamage;

    private ParticleSystem[] _particles;

    public void Awake() {
        _shipRb = GetComponentInParent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _cooldownTimer = 0f;
        _hasFired = true;

        _particles = GetComponentsInChildren<ParticleSystem>();

        _currentWeapon = Weapons.Cannonball;
    }

    public void Update() {
        if (!_hasFired && _delayTimer <= 0f && _cooldownTimer <= 0f && _currentWeapon == Weapons.Cannonball) FireCannonball();
        else if (!_hasFired && _delayTimer <= 0f && _cooldownTimer <= 0f && _currentWeapon == Weapons.Scattershot) FireClustershot();

        _cooldownTimer -= Time.deltaTime;
        _delayTimer -= Time.deltaTime;
    }

    public void SetCannonballDelay() {
        if (_cooldownTimer > 0f) return;

        _currentWeapon = Weapons.Cannonball;
        _delayTimer = Random.Range(minDelay, maxDelay);
        _hasFired = false;
    }

    public void SetClustshotDelay() {
        if (_cooldownTimer > 0f) return;

        _currentWeapon = Weapons.Scattershot;
        _delayTimer = Random.Range(minDelay, maxDelay);
        _hasFired = false;
    }

    public void FireClustershot() {
        float rotationRangeMin = scattershotAmt / 2f - scattershotAmt;
        float rotationRangeMax = scattershotAmt / 2f;

        for (int i = 0; i < scattershotAmt; i++) {
            GameObject clustershotInstance = Instantiate(scattershot);
            CannonballScript ballInfo = cannonball.GetComponent<CannonballScript>();
            ballInfo.Source = SpawnSource.Player;
            ballInfo.Lifetime = cannonballLifetime;
            ballInfo.Damage = damagePerScattershot;

            clustershotInstance.transform.position = transform.position;
            clustershotInstance.transform.rotation = transform.rotation;

            Rigidbody rigidbody = clustershotInstance.GetComponent<Rigidbody>();

            if (_shipRb != null) {
                rigidbody.velocity = _shipRb.velocity;
            }

            float spread = Random.Range(rotationRangeMin, rotationRangeMax);

            clustershotInstance.transform.Rotate(new Vector3(0f, spread * rotationFactor, 0f));

            if (isRight) rigidbody.AddRelativeForce(new Vector3(0f, Random.Range(minScatterSpread, maxScatterSpread), -scattershotSpeed));
            else rigidbody.AddRelativeForce(new Vector3(0f, Random.Range(minScatterSpread, maxScatterSpread), scattershotSpeed));
        }

        for (int i = 0; i < _particles.Length; i++) {
            _particles[i].Play();
        }

        _audioSource.clip = _cannonSounds[Random.Range(0, _cannonSounds.Length - 1)];
        _audioSource.Play();

        _cooldownTimer = cooldown;
        _hasFired = true;
    }

    public void FireCannonball() {
        GameObject cannonballInstance = Instantiate(cannonball);
        CannonballScript ballInfo = cannonball.GetComponent<CannonballScript>();
        ballInfo.Source = SpawnSource.Player;
        ballInfo.Lifetime = cannonballLifetime;
        ballInfo.Damage = damagePerCannonball;

        cannonballInstance.transform.position = transform.position;
        cannonballInstance.transform.rotation = transform.rotation;

        Rigidbody rigidbody = cannonballInstance.GetComponent<Rigidbody>();

        if(_shipRb != null) {
            rigidbody.velocity = _shipRb.velocity;
        }

        if (isRight) rigidbody.AddRelativeForce(new Vector3(0, 0, -cannonballSpeed));
        else rigidbody.AddRelativeForce(new Vector3(0, 0, cannonballSpeed));

        for(int i = 0; i < _particles.Length; i++) {
            _particles[i].Play();
        }

        _audioSource.clip = _cannonSounds[Random.Range(0, _cannonSounds.Length - 1)];
        _audioSource.Play();

        _cooldownTimer = cooldown;
        _hasFired = true;
    }

    public void ChangeCooldown(float cooldownModifier) {
        _oldCooldown = cooldown;
        cooldown *= cooldownModifier;
    }

    public void ResetCooldown() {
        cooldown = _oldCooldown;
    }

    public void ChangeDamage(float damageModifier) {
        _oldCannonballDamage = damagePerCannonball;
        _oldClustershotDamage = damagePerScattershot;

        damagePerCannonball *= damageModifier;
        damagePerScattershot *= damageModifier;
    }

    public void ResetDamage() {
        damagePerCannonball = _oldCannonballDamage;
        damagePerScattershot = _oldClustershotDamage;
    }
}
