using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenuOnInput : MonoBehaviour
{

    public float delay = 5F;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }
    
    private void Update()
    {
        if (!(Time.time - startTime > delay)) return;
        if (!Input.anyKeyDown) return;
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);

    }
}
