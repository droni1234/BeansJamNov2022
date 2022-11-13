using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransition : MonoBehaviour{

    private static MusicTransition instance;

    // Start is called before the first frame update
    void Awake() 
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }    
        else
        {
            
        }
    }
}

