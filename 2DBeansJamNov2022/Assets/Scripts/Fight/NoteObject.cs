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

        private bool hit = false;
        
        private void Update()
        {
            
            if (transform.localPosition.y < -100)
            {
                Destroy(gameObject);
                return;
            }
            
            if (!FightSystem.instance.startPlaying) return;
            
            float timing = (((FightSystem.instance.debugTime == 0 ? time + FightSystem.instance.warmupTime : time) * beat - FightSystem.instance.currentTime) *
                            FightSystem.instance.battle.speed);
            transform.position = new Vector3(transform.position.x, reference.position.y + timing, transform.position.z);
            if (Input.GetButtonDown(buttonToPress) && pressable)
            {
                hit = true;
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
            if(hit) return;
            if(!pressable) return;
            if (!other.CompareTag("Activator")) return;
            pressable = false;

            FightSystem.instance.NoteMissed();
            Destroy(gameObject, 0.5F);
        }
    }
}