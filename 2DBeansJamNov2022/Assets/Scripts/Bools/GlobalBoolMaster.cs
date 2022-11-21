using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using This = Unity.VisualScripting.This;

public class GlobalBoolMaster : MonoBehaviour
{
    
    private static GlobalBoolMaster instance
    {
        get
        {
            if (!_instance)
            {
                var go = new GameObject("_GlobalBoolMaster");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<GlobalBoolMaster>();
            }

            return _instance;
        }
    }

    private static GlobalBoolMaster _instance;


    [SerializeField]
    private List<BoolKey> keys = new();

    public static bool getBool(string key, out bool value)
    {
        if (instance.keys.Any(x => x.key == key))
        {
            value = instance.keys.Where(x => x.key == key).ToList().First().value;
            return true;
        }
        else
        {
            value = false;
            return false;
        }
            
    }

    public static void setBool(string key, bool value)
    {
        instance.keys.Add(BoolKey.consist(key, value));
    }

    public static bool initBool(string key, bool value)
    {
        if (instance.keys.Contains(BoolKey.consist(key, value)))
            return false;
        instance.keys.Add(BoolKey.consist(key, value));
        return true;
    }
    
    [Serializable]
    public struct BoolKey
    {
        public string key;
        public bool value;
        
        public override bool Equals(object obj)
        {
            return (obj.GetType() == typeof(BoolKey)) && ((BoolKey)obj).key == this.key && ((BoolKey)obj).value == this.value;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static BoolKey consist(string key, bool value)
        {
            return new BoolKey
            {
                key = key,
                value = value
            };
        }
    }
}
