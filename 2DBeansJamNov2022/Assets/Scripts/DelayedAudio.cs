using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAudio : MonoBehaviour
{

    public float delay;

    private float startTime;

    private AudioSource sound;
    
    // Start is called before the first frame update
    void Awake()
    {
        startTime = Time.deltaTime;
        sound = GetComponent<AudioSource>();
        StartCoroutine(delayedStart());
    }

    IEnumerator delayedStart()
    {
        yield return new WaitForSeconds(delay);
        sound.Play();
    }
}
