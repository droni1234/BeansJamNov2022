using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
 
public static class EditorSFX
{
 
    public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
     
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
            null
        );
 
        method.Invoke(
            null,
            new object[] { clip, startSample, loop }
        );
    }

    public static void StopAllClips()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
 
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "StopAllPreviewClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { },
            null
        );
 
        method.Invoke(
            null,
            new object[] { }
        );
    }

    public static int GetClipSamplePosition()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
 
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "GetPreviewClipSamplePosition",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { },
            null
        );
 
        return (int)method.Invoke(
            null,
            new object[] { }
        );
    }

    public static bool IsPreviewClipPlaying()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;

        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "IsPreviewClipPlaying",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { },
            null
        );
 
        return (bool)method.Invoke(
            null,
            new object[] { }
        );
    }
}