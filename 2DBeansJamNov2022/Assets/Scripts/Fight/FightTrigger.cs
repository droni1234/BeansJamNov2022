using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using whip.battle.edit;

namespace whip.battle
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class FightTrigger : MonoBehaviour
    {

        public BattleObject battle;
        public string levelname;
        public DialogueTrigger dialogue;

        public void Fight()
        {
            if (dialogue)
            {
                DialogueManager2.instance.Fight(dialogue.messages, dialogue.actors, this);
            }
            else
            {
                FightTriggerMaster.instance.Fight(battle, levelname);
            }

        }
    }
}