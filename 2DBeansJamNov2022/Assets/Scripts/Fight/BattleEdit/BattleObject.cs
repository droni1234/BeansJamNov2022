using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace whip.battle.edit
{
    [CreateAssetMenu(fileName = "Battle", menuName = "ScriptableObjects/BattleScript", order = 1)]
    public class BattleObject : ScriptableObject
    {
        public float bpm;
        public float warmup;
        
        public Enemy enemy;
        public Player player;
        public AudioClip audio;
        [Header("Difficulty")]
        public float speed;
        public int healthAmount = 10;
        public int healthRestore = 2;
        public float healthLoss = 20;

        [Space]
        
        public List<Note> notes = new();

        public List<LookTowards> looks;
        public List<SetSprite> sprites;

        [HideInInspector]
        public float pointer = 0;
        [HideInInspector]
        public int denominator = 1;
        
        public string boolKey = "undefined";
        public bool isBoolKeyPlayerPref = false;
        
        public void addRandomNextNote()
        {
            var time = Mathf.Round(notes.Last().time) + 1;
            notes.Add(new Note(time, (noteKey)Random.Range(0, 4)));
        }

        public void addNote(float time, noteKey direction)
        {
            notes.Add(new Note(time, direction));
        }

        public void addSprite(float time, CharacterType type, int index)
        {
            sprites.Add(new SetSprite(time, type, index));
        }

        public void addFocus(float time, POI poi)
        {
            looks.Add(new LookTowards(time, poi));
        }
    }
}
