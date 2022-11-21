using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using whip.battle.edit;

public class PlayerPrefsBool : MonoBehaviour
{
    public string key;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(key))
        {
            GlobalBoolMaster.initBool(key, PlayerPrefs.GetInt(key, -1)== 1);
        }
    }
}
