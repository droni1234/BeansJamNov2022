using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAudio : MonoBehaviour
{

    public float delay;

    private float startTime;

    private AudioSource sound;
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.deltaTime;
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > delay)
        {
            if(!sound.isPlaying)
                sound.Play();
        }
    }
}
