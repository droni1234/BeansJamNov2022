using System;
using UnityEngine;

namespace whip.battle.edit
{
    [Serializable]
    public class Enemy : Character
    {
    
    }
    [Serializable]
    public class Player : Character
    {
        
    }
    [Serializable]
    public class Character
    {
        public Sprite[] sprites;
    }
    
    
    
    [Serializable]
    public class LookTowards : Event
    {
        public LookTowards(float time, POI lookTowards)
        {
            this.time = time;
            this.lookTowards = lookTowards;
        }
    
        public LookTowards(LookTowards _looktowards)
        {
            time = _looktowards.time;
            lookTowards = _looktowards.lookTowards;
        }
        
        public POI lookTowards;
    
        public override EventType thisEventType => EventType.LookTowards;
    }
    
    [Serializable]
    public class SetSprite : Event
    {
    
        public SetSprite(float time, CharacterType type, int spriteIndex)
        {
            this.time = time;
            this.type = type;
            this.spriteIndex = spriteIndex;
        }
        
        public SetSprite(SetSprite setSprite)
        {
            this.time = setSprite.time;
            this.type = setSprite.type;
            this.spriteIndex = setSprite.spriteIndex;
        }
        
        public CharacterType type;
        public int spriteIndex;
        public override EventType thisEventType => EventType.SetSprite;
    }
    
    public enum CharacterType
    {
        Player,
        Enemy
    }
    
    [Serializable]
    public enum POI
    {
        player,
        enemy,
        both
    }
    
    [Serializable]
    public class Note : Event
    { 
        public noteKey key;
    
        public Note(float time, noteKey direction)
        {
            this.time = time;
            this.key = direction;
        }
        
        public Note(Note note)
        {
            this.key = note.key;
            this.time = note.time;
        }
    
        public override EventType thisEventType => EventType.Note;
    }
    [Serializable]
    public enum noteKey
    {
        left,
        down,
        up,
        right
    }
    [Serializable]
    public abstract class Event
    {
        public float time;
    
        public abstract EventType thisEventType { get; }
    }
    
    public enum EventType
    {
        Note,
        LookTowards,
        SetSprite,
        Undefined
    }
}