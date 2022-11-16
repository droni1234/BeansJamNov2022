using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightTriggerMaster : MonoBehaviour
{
    public static FightTriggerMaster instance;

    private BattleObject cache;
    private string level_cache;
    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    
    public void Fight(BattleObject battle, string level)
    {
        SceneManager.sceneLoaded += StartFight;
        SceneManager.LoadScene("Fight", LoadSceneMode.Single);
        cache = battle;
        level_cache = level;
    }

    private void StartFight(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("Fight")) return;
        var fightSystem = GameObject.FindGameObjectWithTag("Fight").GetComponent<FightSystem>();
        fightSystem.battle = cache;
        fightSystem.winLevel = level_cache;
        //fightSystem.startPlaying = true;
        SceneManager.sceneLoaded -= StartFight;
        DestroyImmediate(gameObject);
    }
    
}
