using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FightSystem : MonoBehaviour
{
    public BattleObject battle;
    
    public AudioSource music;
    private bool playedMusic = false;

    public bool startPlaying;

    public BeatScroller scroller;

    public static FightSystem instance;

    public float warmupTime = 5F;
    private float startTime = 0;

    public float currentScore = 0F;
    public float scorePerNote = 100F;
    public float malusPerNote = -150F;

    public float multiplier = 1.0F;

    public Image playerImage;
    [HideInInspector]
    public List<Sprite> playerSprite;
    public Image enemyImage;
    [HideInInspector]
    public List<Sprite> enemySprite;

    public List<LookTowards> lookTowardList;
    public List<SetSprite> setSpriteList;


    public float currentTime => Time.time - startTime;

    private void Start()
    {
        instance = this;
        playerSprite = new List<Sprite>(battle.player.sprites);
        enemySprite = new List<Sprite>(battle.enemy.sprites);
        lookTowardList = new List<LookTowards>(battle.looks);
        setSpriteList = new List<SetSprite>(battle.sprites);
        scroller.bpm = battle.bpm;
        scroller.speed = battle.speed;
        warmupTime = battle.warmup;
        music.clip = battle.audio;
        currentScore = 0;
        startTime = Time.time;
        //multiplier = 1;

        var notesCopy = battle.notes.Select(note => new Note(note)).ToList();
        notesCopy.ForEach(x => x.time = (x.time + warmupTime) * scroller.beatTempo * scroller.speed);

    FindObjectOfType<NoteGenerator>().Generate(notesCopy);

        lookTowardList.OrderBy(x => x.time);
        setSpriteList.OrderBy(x => x.time);
    }

    private void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                scroller.isRunning = true;
                startTime = Time.time;
            }
        }
        else
        {
            if (currentTime > (warmupTime * scroller.beatTempo) && !playedMusic)
            {
                music.Play();
                playedMusic = true;
            }
        }

        if (lookTowardList.Count > 0)
            CheckLookAtCommand(lookTowardList.First());

        if (setSpriteList.Count > 0)
            CheckSetSpriteCommand(setSpriteList.First());
    }

    private void CheckSetSpriteCommand(SetSprite setSprite)
    {
        if (setSprite.time < currentTime) return;
        
        switch(setSprite.type)
        {
            case CharacterType.Player:
                setPlayerSprite(setSprite.spriteIndex);
                break;
            case CharacterType.Enemy:
                setEnemySprite(setSprite.spriteIndex);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        setSpriteList.Remove(setSprite);
    }

    private void setPlayerSprite(int spriteIndex)
    {
        playerImage.sprite = playerSprite[spriteIndex];
    }
    
    private void setEnemySprite(int spriteIndex)
    {
        enemyImage.sprite = enemySprite[spriteIndex];
    }
    
    private void CheckLookAtCommand(LookTowards lookTowards)
    {
        if (lookTowards.time < currentTime) return;
        
        switch (lookTowardList.First().lookTowards)
        {
            case POI.player:
                LookAtPlayer();
                break;
            case POI.enemy:
                LookAtEnemy();
                break;
            case POI.both:
            default:
                LookAtBoth();
                break;
        }

        lookTowardList.Remove(lookTowards);
    }

    public void LookAtPlayer()
    {
        print("looking at PLayer");
    }

    public void LookAtEnemy()
    {
        print("Looking at Enemy");
    }

    public void LookAtBoth()
    {
        print("Looking at Both");
    }


    public void NoteHit()
    {
        currentScore += scorePerNote * multiplier;
        multiplier *= 1.1F;
    }

    public void NoteMissed()
    {
        multiplier = 1.0F;
        currentScore -= malusPerNote;
    }
}