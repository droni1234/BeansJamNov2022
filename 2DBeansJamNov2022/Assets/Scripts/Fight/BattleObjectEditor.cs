using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattleObject))]
public class BattleObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (BattleObject)target;
 
        if(GUILayout.Button("Add to Counter", GUILayout.Height(40)))
        {
            script.addRandomNextNote();
        }
         
    }
}
