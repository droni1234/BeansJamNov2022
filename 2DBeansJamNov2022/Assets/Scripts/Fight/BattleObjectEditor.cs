using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using static noteKey;

[CustomEditor(typeof(BattleObject))]
public class BattleObjectEditor : Editor
{

    private string[] NoteOptions = {     
        "l 1/8",
        "d 1/8",
        "u 1/8",
        "r 1/8",
        "s 1/8",
        "l 1/4",
        "d 1/4",
        "u 1/4",
        "r 1/4",
        "s 1/4",
        "l 1/2",
        "d 1/2",
        "u 1/2",
        "r 1/2",
        "s 1/2",
        "l 1/1",
        "d 1/1",
        "u 1/1",
        "r 1/1",
        "s 1/1"
    };

    private float pointer = 0;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (BattleObject)target;
 
        if(GUILayout.Button("Add to Counter", GUILayout.Height(20)))
        {
            script.addRandomNextNote();
        }
        GUILayout.Space(25);
        GUILayout.Label("Add Notes");
        pointer = float.Parse(EditorGUILayout.TextField("Position: ", pointer.ToString()));
        var response = GUILayout.SelectionGrid(-1, NoteOptions, 5, GUILayout.Height(160));

        if (response > 0)
        {
            switch (NoteOptions[response])
            {
                case "l 1/8":
                    script.addNote(pointer, left);
                    pointer += 1F / 8F;
                    break;
                case "d 1/8":
                    script.addNote(pointer, down);
                    pointer += 1F / 8F;
                    break;
                case "u 1/8":
                    script.addNote(pointer, up);
                    pointer += 1F / 8F;
                    break;
                case "r 1/8":
                    script.addNote(pointer, right);
                    pointer += 1F / 8F;
                    break;
                case "s 1/8":
                    pointer += 1F / 8F;
                    break;
                case "l 1/4":
                    script.addNote(pointer, left);
                    pointer += 1F / 4F;
                    break;
                case "d 1/4":
                    script.addNote(pointer, down);
                    pointer += 1F / 4F;
                    break;
                case "u 1/4":
                    script.addNote(pointer, up);
                    pointer += 1F / 4F;
                    break;
                case "r 1/4":
                    script.addNote(pointer, right);
                    pointer += 1F / 4F;
                    break;
                case "s 1/4":
                    pointer += 1F / 4F;
                    break;
                case "l 1/2":
                    script.addNote(pointer, left);
                    pointer += 1F / 2F;
                    break;
                case "d 1/2":
                    script.addNote(pointer, down);
                    pointer += 1F / 2F;
                    break;
                case "u 1/2":
                    script.addNote(pointer, up);
                    pointer += 1F / 2F;
                    break;
                case "r 1/2":
                    script.addNote(pointer, right);
                    pointer += 1F / 2F;
                    break;
                case "s 1/2":
                    pointer += 1F / 2F;
                    break;
                case "l 1/1":
                    script.addNote(pointer, left);
                    pointer += 1F;
                    break;
                case "d 1/1":
                    script.addNote(pointer, down);
                    pointer += 1F;
                    break;
                case "u 1/1":
                    script.addNote(pointer, up);
                    pointer += 1F;
                    break;
                case "r 1/1":
                    script.addNote(pointer, right);
                    pointer += 1F;
                    break;
                case "s 1/1":
                    pointer += 1F;
                    break;
            }
        }
    }
}
