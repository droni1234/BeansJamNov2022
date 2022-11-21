using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolInit : MonoBehaviour
{
    public string key;

    private void Awake()
    {
        GlobalBoolMaster.initBool(key, true);
    }
}
