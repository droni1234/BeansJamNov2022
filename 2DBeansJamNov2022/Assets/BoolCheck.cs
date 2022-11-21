using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoolCheck : MonoBehaviour
{

    public GlobalBoolMaster.BoolKey[] pairs;

    private void Start()
    {
        gameObject.SetActive(pairs.ToList().All(CheckPair));
    }

    private static bool CheckPair(GlobalBoolMaster.BoolKey pair)
    {
        GlobalBoolMaster.getBool(pair.key, out var value);
        return value == pair.value;
    }
}
