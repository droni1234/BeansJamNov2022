using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
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

    private string[] lookOptions =
    {
        "Player 1/8",
        "Enemy 1/8",
        "Both 1/8",
        "Skip 1/8",
        "Player 1/4",
        "Enemy 1/4",
        "Both 1/4",
        "Skip 1/4",
        "Player 1/2",
        "Enemy 1/2",
        "Both 1/2",
        "Skip 1/2",
        "Player 1/1",
        "Enemy 1/1",
        "Both 1/1",
        "Skip 1/1"
    };

    private string[] spriteOptions =
    {
        "Skip 1/8",
        "Skip 1/4",
        "Skip 1/2",
        "Skip 1/1"
    };
    
    private float pointer = 0;

    private CharacterType charSelection;

    private const int spriteDivider = 6;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (BattleObject) target;

        if (GUILayout.Button("Add Random Note 1/4", GUILayout.Height(20)))
        {
            script.addRandomNextNote();
        }
        
        if (GUILayout.Button("Add 10 Random Notes 1/4", GUILayout.Height(20)))
        {
            script.addRandomNextNote();
            script.addRandomNextNote();
            script.addRandomNextNote();
            script.addRandomNextNote();
            script.addRandomNextNote();
            script.addRandomNextNote();
            script.addRandomNextNote();
            script.addRandomNextNote();
            script.addRandomNextNote();
            script.addRandomNextNote();
        }

        EditorGUILayout.Space(25);
        EditorGUILayout.LabelField("Add Notes");
        drawPosition();
        var noteGrid = GUILayout.SelectionGrid(-1, NoteOptions, 5, GUILayout.Height(160));

        if (noteGrid >= 0 && noteGrid < NoteOptions.Length)
        {
            switch (NoteOptions[noteGrid])
            {
                case "l 1/8":
                    script.addNote(pointer, left);
                    pointer += 0.5F;
                    break;
                case "d 1/8":
                    script.addNote(pointer, down);
                    pointer += 0.5F;
                    break;
                case "u 1/8":
                    script.addNote(pointer, up);
                    pointer += 0.5F;
                    break;
                case "r 1/8":
                    script.addNote(pointer, right);
                    pointer += 0.5F;
                    break;
                case "s 1/8":
                    pointer += 0.5F;
                    break;
                case "l 1/4":
                    script.addNote(pointer, left);
                    pointer += 1F;
                    break;
                case "d 1/4":
                    script.addNote(pointer, down);
                    pointer += 1F;
                    break;
                case "u 1/4":
                    script.addNote(pointer, up);
                    pointer += 1F;
                    break;
                case "r 1/4":
                    script.addNote(pointer, right);
                    pointer += 1F;
                    break;
                case "s 1/4":
                    pointer += 1F;
                    break;
                case "l 1/2":
                    script.addNote(pointer, left);
                    pointer += 2F;
                    break;
                case "d 1/2":
                    script.addNote(pointer, down);
                    pointer += 2F;
                    break;
                case "u 1/2":
                    script.addNote(pointer, up);
                    pointer += 2F;
                    break;
                case "r 1/2":
                    script.addNote(pointer, right);
                    pointer += 2F;
                    break;
                case "s 1/2":
                    pointer += 2F;
                    break;
                case "l 1/1":
                    script.addNote(pointer, left);
                    pointer += 4F;
                    break;
                case "d 1/1":
                    script.addNote(pointer, down);
                    pointer += 4F;
                    break;
                case "u 1/1":
                    script.addNote(pointer, up);
                    pointer += 4F;
                    break;
                case "r 1/1":
                    script.addNote(pointer, right);
                    pointer += 4F;
                    break;
                case "s 1/1":
                    pointer += 4F;
                    break;
            }
        }

        EditorGUILayout.Space(25);
        EditorGUILayout.LabelField("Add Look At");
        drawPosition();

        var lookGrid = GUILayout.SelectionGrid(-1, lookOptions, 4, GUILayout.Height(120));

        if (lookGrid >= 0 && lookGrid < NoteOptions.Length)
        {
            switch (lookOptions[lookGrid])
            {
                case "Player 1/8":
                    pointer += 0.5F;
                    break;
                case "Enemy 1/8":
                    pointer += 0.5F;
                    break;
                case "Both 1/8":
                    pointer += 0.5F;
                    break;
                case "Skip 1/8":
                    pointer += 0.5F;
                    break;
                case "Player 1/4":
                    pointer += 1F;
                    break;
                case "Enemy 1/4":
                    pointer += 1F;
                    break;
                case "Both 1/4":
                    pointer += 1F;
                    break;
                case "Skip 1/4":
                    pointer += 1F;
                    break;
                case "Player 1/2":
                    pointer += 2F;
                    break;
                case "Enemy 1/2":
                    pointer += 2F;
                    break;
                case "Both 1/2":
                    pointer += 2F;
                    break;
                case "Skip 1/2":
                    pointer += 2F;
                    break;
                case "Player 1/1":
                    pointer += 4F;
                    break;
                case "Enemy 1/1":
                    pointer += 4F;
                    break;
                case "Both 1/1":
                    pointer += 4F;
                    break;
                case "Skip 1/1":
                    pointer += 4F;
                    break;
            }
        }

        GUILayout.Space(25);
        GUILayout.Label("Sprites");
        charSelection = (CharacterType) EditorGUILayout.EnumPopup("Select Character:", charSelection);
        drawPosition();
        float preview_Size = (Screen.width - 30) / spriteDivider;
        Rect last_rect = GUILayoutUtility.GetLastRect();
        Sprite[] selectedSprites;
        if (charSelection == CharacterType.Player)
            selectedSprites = script.player.sprites;
        else
            selectedSprites = script.enemy.sprites;

        GUILayout.BeginHorizontal();
        if (selectedSprites != null)
        {
                
            for (var i = 0; i < selectedSprites.Length; i++)
            {
                var pressed = GUILayout.Button(selectedSprites[i].texture, GUILayout.MaxHeight(preview_Size),
                    GUILayout.MaxWidth(preview_Size));
                if (pressed)
                {
                    script.addSprite(pointer, charSelection, i);
                }

                if (i % spriteDivider == 3)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
            }
        }

        GUILayout.EndHorizontal();

        var spriteGrid = GUILayout.SelectionGrid(-1, spriteOptions, 4, GUILayout.Height(30));

        if (spriteGrid >= 0 && spriteGrid < NoteOptions.Length)
        {
            switch (spriteOptions[spriteGrid])
            {
                case "Skip 1/8":
                    pointer += 1F / 2F;
                    break;
                case "Skip 1/4":
                    pointer += 1;
                    break;
                case "Skip 1/2":
                    pointer += 2;
                    break;
                case "Skip 1/1":
                    pointer += 4F;
                    break;
            }
        }
    }

    void drawPosition()
    {
        pointer = float.Parse(EditorGUILayout.TextField("Position: ", pointer.ToString()));
        EditorGUILayout.LabelField("Zeit im Track: " + (1 / (((BattleObject) target).bpm / 60)) * pointer + " Sekunden");
    }
}