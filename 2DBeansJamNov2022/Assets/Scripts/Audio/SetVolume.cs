using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{

    [SerializeField]
    private AudioMixer mainMixer;

    public void SetVol_SFX(float volume)
    {
        if (volume <= -20)
            mainMixer.SetFloat("sfx_vol", -80);
        else
            mainMixer.SetFloat("sfx_vol", volume);

    }


    public void SetVol_Music(float volume)
    {
        if (volume <= -20)
            mainMixer.SetFloat("music_vol", -80);
        else
            mainMixer.SetFloat("music_vol", volume);
    }

}
