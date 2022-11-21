using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace whip.battle
{
    public class NoteObject : MonoBehaviour
    {

        public bool pressable;

        public string buttonToPress;

        public float time;

        public float bpm;

        public float beat;

        public Transform reference;

        private void Update()
        {
            if (!FightSystem.instance.startPlaying) return;
            
            float timing = (((time + FightSystem.instance.warmupTime) * beat - FightSystem.instance.currentTime) *
                            FightSystem.instance.battle.speed);
            transform.position = new Vector3(transform.position.x, reference.position.y + timing, transform.position.z);
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
            if(!pressable) return;
            if (!other.CompareTag("Activator")) return;
            pressable = false;

            FightSystem.instance.NoteMissed();
            Destroy(gameObject, 0.5F);
        }
    }
}