using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{

    public bool isRunning;


    public float bpm
    {
        set {
            _bpm = value;
            beatTempo = 1 / (_bpm / 60);
        }
        get => _bpm;
    }

    private float _bpm;
    public float speed = 10;
    [HideInInspector]
    public float beatTempo;

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }

        transform.position += Vector3.down * (beatTempo * Time.deltaTime * 4 * speed);
    }
}
