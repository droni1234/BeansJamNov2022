using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle", menuName = "ScriptableObjects/BattleScript", order = 1)]
public class BattleObject : ScriptableObject
{
    public float bpm;
    public float speed;
    public float warmup;
    public AudioClip audio;
    public Enemy enemy;
    public Player player;

    public List<Note> notes;

    public List<LookTowards> looks;
    public List<SetSprite> sprites;
}



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
    public POI lookTowards;

    public override EventType thisEventType => EventType.LookTowards;
}

[Serializable]
public class SetSprite : Event
{
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