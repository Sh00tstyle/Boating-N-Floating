using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour {

    public AudioClip[] audioClips;

    private AudioSource _audioSource;

    public void Awake() {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
        _audioSource.Play();
    }
}
