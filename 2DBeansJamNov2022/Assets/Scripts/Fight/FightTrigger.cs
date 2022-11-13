using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class FightTrigger : MonoBehaviour
{

    public BattleObject battle;
    public string levelname;
    
    public void Fight()
    {
        FightTriggerMaster.instance.Fight(battle, levelname);
    }
}
