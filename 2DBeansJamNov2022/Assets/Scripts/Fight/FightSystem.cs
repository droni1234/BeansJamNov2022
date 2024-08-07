using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;
using whip.battle.edit;
using Random = UnityEngine.Random;

namespace whip.battle
{
    public class FightSystem : MonoBehaviour
    {
        public BattleObject battle;

        public AudioSource music;
        private bool playedMusic = false;

        public bool startPlaying
        {
            set
            {
                if (!_startPlaying)
                {
                    _startPlaying = value;
                    scroller.isRunning = value;
                    startTime = Time.time;
                }
            }
            get => _startPlaying;
        }

        public bool _startPlaying;

        public BeatScroller scroller;

        public static FightSystem instance;

        public float warmupTime = 5F;
        private float startTime = 0;

        public float currentScore = 0F;
        public float scorePerNote = 100F;
        public float malusPerNote = 150F;

        public float multiplier = 1.0F;

        public float healthDifficulty = 20F;
        public int healthRestore = 2;
        public int noteHealthAmount = 10;

        private float health = 1.0F;

        public Image playerImage;
        [HideInInspector] public List<Sprite> playerSprite;
        public Image enemyImage;
        [HideInInspector] public List<Sprite> enemySprite;

        public List<LookTowards> lookTowardList;
        public List<SetSprite> setSpriteList;

        [SerializeField] private Animator cameraDeath;

        [SerializeField] private AudioSource niceSound;

        protected float niceTimer;
        private float niceCoolDown = 40F;

        [SerializeField] private TextMeshProUGUI scoreText;

        private Animator FightAnimator;

        [HideInInspector] public string winLevel;

        public float debugTime;

        public float currentTime => Time.time - startTime + debugTime;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            FightAnimator = GetComponent<Animator>();

            playerSprite = new List<Sprite>(battle.player.sprites);
            lookTowardList = new List<LookTowards>();
            setSpriteList = new List<SetSprite>();
            enemySprite = new List<Sprite>(battle.enemy.sprites);
            healthDifficulty = battle.healthLoss;
            noteHealthAmount = battle.healthAmount;
            healthRestore = battle.healthRestore;
            scroller.bpm = battle.bpm;
            scroller.speed = battle.speed;
            warmupTime = battle.warmup;
            music.clip = battle.audio;
            currentScore = 0;
            startTime = Time.time;
            multiplier = 1;

            var notesCopy = battle.notes.Select(note => new Note(note)).ToList();
            //notesCopy.ForEach(x => x.time = (x.time + warmupTime) * scroller.beatTempo * scroller.speed);

            FindObjectOfType<NoteGenerator>().Generate(notesCopy);

            lookTowardList = battle.looks.Select(look => new LookTowards(look)).ToList();
            lookTowardList.ForEach(x => x.time = (x.time + warmupTime) * scroller.beatTempo);
            lookTowardList.OrderBy(x => x.time);

            setSpriteList = battle.sprites.Select(_sprite => new SetSprite(_sprite)).ToList();
            setSpriteList.ForEach(x => x.time = (x.time + warmupTime) * scroller.beatTempo);
            setSpriteList.OrderBy(x => x.time);

            if (battle)
            {
                startPlaying = true;
            }
        }

        private void Update()
        {
            scoreText.text =
                "Score: " + Math.Round(currentScore * 100F) / 100F + "\n" +
                "Multiplier: " + Math.Round(multiplier * 10F) / 10F + "\n" +
                "Time: " + Math.Round(currentTime * 100F) / 100F;

            float beatSizer = currentTime * 100F % (scroller.beatTempo * 100F) / 100F;
            scoreText.fontSize = 39F + beatSizer * 3F;
            enemyImage.transform.localScale =
                new Vector3(1F + beatSizer * 0.05F, 1F + beatSizer * 0.05F, 1F + beatSizer * 0.05F);
            playerImage.transform.localScale =
                new Vector3(1F + beatSizer * 0.05F, 1F + beatSizer * 0.05F, 1F + beatSizer * 0.05F);
            health -= Time.deltaTime * (healthDifficulty / noteHealthAmount);

            if (startPlaying)
            {
                if (currentTime > ((debugTime != 0) ? 0 : ((warmupTime * scroller.beatTempo))) && !playedMusic)
                {
                    music.timeSamples = (int) ((debugTime * music.clip.frequency));
                    music.Play();
                    playedMusic = true;
                }
            }

            if (lookTowardList.Count > 0)
                CheckLookAtCommand(lookTowardList.First());

            if (setSpriteList.Count > 0)
                CheckSetSpriteCommand(setSpriteList.First());


            cameraDeath.PlayInFixedTime(0, 0, 1F - health);
            if (health <= 0)
            {
                Loose();
            }

            if (scroller.transform.childCount == 0)
            {
                Win();
            }
        }

        private void CheckSetSpriteCommand(SetSprite setSprite)
        {
            if (setSprite.time >= currentTime) return;

            switch (setSprite.type)
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
            if (playerSprite[spriteIndex])
            {
                playerImage.sprite = playerSprite[spriteIndex];
                playerImage.color = Color.white;
            }
            else
            {
                enemyImage.color = Color.clear;
            }
        }

        private void setEnemySprite(int spriteIndex)
        {
            if (enemySprite[spriteIndex])
            {
                enemyImage.sprite = enemySprite[spriteIndex];
                enemyImage.color = Color.white;
            }
            else
            {
                enemyImage.color = Color.clear;
            }
        }

        private void CheckLookAtCommand(LookTowards lookTowards)
        {
            if (lookTowards.time > currentTime) return;

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

        private void LookAtPlayer()
        {
            FightAnimator.SetTrigger("FocusPlayer");
        }

        private void LookAtEnemy()
        {
            FightAnimator.SetTrigger("FocusEnemy");
        }

        private void LookAtBoth()
        {
            FightAnimator.SetTrigger("FocusBoth");
        }


        public void NoteHit()
        {
            currentScore += scorePerNote * multiplier;
            multiplier += 0.1F;
            if (multiplier > 10F && niceTimer < currentTime)
            {
                GoodCombo();
            }

            health += 1F / noteHealthAmount * healthRestore;
            if (health > 1F)
            {
                health = 1F;
            }
        }

        public void NoteMissed()
        {
            multiplier = 1.0F;
            currentScore -= malusPerNote;
            niceTimer = 0;
            health -= 1F / noteHealthAmount;
        }

        public void GoodCombo()
        {
            niceTimer = currentTime + niceCoolDown;
            niceSound.pitch = Random.Range(0.9F, 1.1F);
            niceSound.Play();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Win()
        {
            if (battle.isBoolKeyPlayerPref)
            {
                PlayerPrefs.SetInt(battle.boolKey, 1);
                PlayerPrefs.Save();
            }
            GlobalBoolMaster.setBool(battle.boolKey, true);
            
            if (SceneUtility.GetBuildIndexByScenePath(winLevel) != -1) {
                SceneManager.LoadSceneAsync(winLevel, LoadSceneMode.Single);
            }
            else
            {
                Debug.Log(SceneManager.GetSceneByName(winLevel) + " is invalid");
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
            Destroy(this);
        }

        private void Loose()
        {
            SceneManager.LoadSceneAsync("Gameover", LoadSceneMode.Single);
            Destroy(this);
        }
    }
}