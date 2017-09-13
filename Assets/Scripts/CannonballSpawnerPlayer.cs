using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballSpawnerPlayer : MonoBehaviour {

    public GameObject cannonball;
    public GameObject cannonFireParticle;

    public float cannonballSpeed;
    public float cannonballLifetime;
    public float cooldown;
    public bool isRight;

    public float minDelay;
    public float maxDelay;

    public AudioClip[] _cannonSounds;

    private Rigidbody _shipRb;
    private AudioSource _audioSource;
    private float _cooldownTimer;
    private float _delayTimer;
    private bool _hasFired;

    private ParticleSystem[] _particles;

    public void Awake() {
        _shipRb = GetComponentInParent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _cooldownTimer = 0f;
        _hasFired = true;

        _particles = GetComponentsInChildren<ParticleSystem>(); 
    }

    public void Update() {
        if (!_hasFired && _delayTimer <= 0f && _cooldownTimer <= 0f) Fire();

        _cooldownTimer -= Time.deltaTime;
        _delayTimer -= Time.deltaTime;
    }

    public void SetDelayTimer() {
        if (_cooldownTimer > 0f) return;

        _delayTimer = Random.Range(minDelay, maxDelay);
        _hasFired = false;
    }

    public void Fire() {
        GameObject cannonballInstance = Instantiate(cannonball);
        CannonballScript ballInfo = cannonball.GetComponent<CannonballScript>();
        ballInfo.Source = SpawnSource.Player;
        ballInfo.Lifetime = cannonballLifetime;

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
}
