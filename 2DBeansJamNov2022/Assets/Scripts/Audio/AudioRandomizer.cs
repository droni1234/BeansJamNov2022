using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioRandomizer : MonoBehaviour
{

    public float minDelay;
    public float maxDelay;

    [Range(0.5F,1.5F)]
    public float mindPitch = 1;
    [Range(1.0F,2.5F)]
    public float maxPitch = 1;

    public AudioClip[] audioPool;

    private float delay = 0;

    private bool playOnce = false;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        delay = Random.Range(minDelay, maxDelay);
    }

    private void Update()
    {
        if (delay >= 0)
            delay -= Time.deltaTime;
        else if (!playOnce)
            PlaySound();

    }

    private void PlaySound()
    {
        playOnce = true;

        source.pitch = Random.Range(mindPitch, maxPitch);
        source.clip = audioPool[Random.Range(0, audioPool.Length)];

        source.Play();
    }

}
