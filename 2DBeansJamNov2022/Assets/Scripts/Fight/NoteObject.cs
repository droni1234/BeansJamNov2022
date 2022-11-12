using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{

    public bool pressable;

    public string buttonToPress;

    private void Update()
    {
        if (Input.GetButtonDown(buttonToPress) && pressable)
        {
            FightSystem.instance.NoteHit();
            DestroyImmediate(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Activator")) return;
        pressable = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Activator")) return;
        pressable = false;

        FightSystem.instance.NoteMissed();
    }
}
