using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEditor;
using static noteKey;
using static UnityEngine.Color;
using EGL = UnityEditor.EditorGUILayout;
using GL = UnityEngine.GUILayout;

#if UNITY_EDITOR
[CustomEditor(typeof(BattleObject))]
public class BattleObjectEditor : Editor
{

    private string[] navOptions =
    {
        "⇤\nStart",
        "←\n1/1",
        "←\n1/2",
        "←\n1/4",
        "←\n1/8",
        "⇠\nLast",
        "⇢\nNext",
        "→\n1/8",
        "→\n1/4",
        "→\n1/2",
        "→\n1/1",
        "⇥\nEnd",
    };

    private string[] commonDenominators =
    {
        "1 \u2215 1",
        "1 \u2215 2",
        "1 \u2215 3",
        "1 \u2215 4",
        "1 \u2215 5",
        "1 \u2215 6",
        "1 \u2215 7",
        "1 \u2215 8"
    };
    
    private string[] NoteOptions = {     
        "←\nleft",
        "↓\ndown",
        "↑\nup",
        "→\nright"
    };

    private string[] lookOptions =
    {
        "Player",
        "Enemy",
        "Both"
    };

    private string[] skipOptions =
    {
        "Skip 1/8",
        "Skip 1/4",
        "Skip 1/2",
        "Skip 1/1"
    };

    private float pointer
    {
        get => ((BattleObject) target).pointer;
        set => ((BattleObject) target).pointer = value;
    }

    private CharacterType charSelection;

    private const int spriteDivider = 6;

    private bool showNoteEdit;
    private bool showFocusEdit;
    private bool showSpriteEdit;

    private float denominator
    {
        get
        {
            switch (_denominator)
            {
                default:
                case 0:
                    return 4F / 1F;
                case 1:
                    return 4F / 2F;
                case 2:
                    return 4F / 3F;
                case 3:
                    return 4F / 4F;
                case 4:
                    return 4F / 5F;
                case 5:
                    return 4F / 6F;
                case 6:
                    return 4F / 7F;
                case 7:
                    return 4F / 8F;
            }
        }
    }

    private int _denominator
    {
        get => ((BattleObject) target).denominator;
        set => ((BattleObject) target).denominator = value;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        EditorSFX.StopAllClips();
    }
    
    public override void OnInspectorGUI()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
        base.OnInspectorGUI();
        var script = (BattleObject) target;
        EGL.LabelField("", GUI.skin.horizontalSlider);

        if (GL.Button("Play Audio"))
        {
            EditorSFX.StopAllClips();
            EditorSFX.PlayClip(script.audio,  Convert.ToInt32((1F / (script.bpm / 60F)) * (pointer) * 44100F));
        }
        if (GL.Button("Stop Audio"))
        {
            EditorSFX.StopAllClips();
        }


        navigation();
        
        EGL.LabelField("", GUI.skin.horizontalSlider);
        
        showNoteEdit = EGL.Foldout(showNoteEdit, "Notes");
        if(showNoteEdit)
        {
            var noteGrid = GL.Toolbar(-1, NoteOptions, GL.Height(40));
            if(noteGrid != -1)
                script.addNote(pointer, (noteKey)noteGrid);
        }


        EGL.LabelField("", GUI.skin.horizontalSlider);
        
        showFocusEdit = EGL.Foldout(showFocusEdit, "Focus");
        if(showFocusEdit)
        {
            var lookGrid = GL.Toolbar(-1, lookOptions);
        
            if (lookGrid != -1)
                script.addFocus(pointer, (POI) lookGrid);
        }




        EGL.LabelField("", GUI.skin.horizontalSlider);
        
        showSpriteEdit = EGL.Foldout(showSpriteEdit, "Sprites");
        if(showSpriteEdit)
        {
            
            charSelection = (CharacterType) EGL.EnumPopup("Select Character:", charSelection);
            float preview_Size = (Screen.width - 30) / spriteDivider;
            Sprite[] selectedSprites = charSelection switch
            {
                CharacterType.Player => script.player.sprites,
                CharacterType.Enemy => script.enemy.sprites,
                _ => throw new ArgumentOutOfRangeException()
            };
        
            GUILayoutOption[] horizontalOptions = {GL.MaxHeight(preview_Size), GL.MaxWidth(preview_Size * spriteDivider)};
            GUILayoutOption[] buttonOptions = {GL.MaxWidth(preview_Size), GL.MinHeight(preview_Size), GL.MaxWidth(preview_Size)};
        
            GL.BeginHorizontal(horizontalOptions);
            if (selectedSprites != null)
            {
                
                for (var i = 0; i < selectedSprites.Length; i++)
                {
                    bool pressed;
                    if (!selectedSprites[i])
                        pressed = GL.Button(GUIContent.none, buttonOptions);
                    else
                        pressed = GL.Button(selectedSprites[i].texture, buttonOptions);
                    if (pressed)
                    {
                        script.addSprite(pointer, charSelection, i);
                    }

                    if (i % spriteDivider != spriteDivider - 1) continue;
                    GL.EndHorizontal();
                    GL.BeginHorizontal(horizontalOptions);
                }
            }

            GL.EndHorizontal();
        }
        
        EGL.LabelField("", GUI.skin.horizontalSlider);
        currentTimeInfo();

    }

    void currentTimeInfo()
    {
        var script = (BattleObject)target;
        EGL.LabelField("Notes:");
        GL.BeginHorizontal();
        foreach (Note note in script.notes.Where(x=> x.time == pointer).ToList())
        {
            if (GL.Button(note.key.ToString()))
            {
                script.notes.Remove(note);
            }
        }
        GL.EndHorizontal();
        
        
        EGL.LabelField("Sprites Player");
        GL.BeginHorizontal();
        foreach (SetSprite sprite in script.sprites.Where(x=> x.time == pointer && x.type == CharacterType.Player).ToList())
        {
            var frame = script.player.sprites[sprite.spriteIndex];
            if (GL.Button(frame ? frame.texture : Texture2D.blackTexture, GL.Height(100),
                    GL.Width(100)))
            {
                script.sprites.Remove(sprite);
            }
        }
        GL.EndHorizontal();
        
        EGL.LabelField("Sprites Enemy");
        GL.BeginHorizontal();
        //Filters for pressed buttons
        foreach (SetSprite sprite in 
                 from sprite in script.sprites.Where(x=> x.time == pointer && x.type == CharacterType.Enemy).ToList() 
                 let frame = script.enemy.sprites[sprite.spriteIndex] 
                 where GL.Button(frame ? frame.texture : Texture2D.blackTexture, GL.Height(100), GL.Width(100)) 
                 select sprite)
        {
            script.sprites.Remove(sprite);
        }
        GL.EndHorizontal();
    }

    void navigation()
    {
        var script = (BattleObject) target;
        var pointerInput = EGL.TextField("Position: ", pointer.ToString());
        pointer = float.Parse(pointerInput == "" ? "0" : pointerInput);
        EGL.LabelField("Zeit im Track: " + (1F / (((BattleObject) target).bpm / 60)) * pointer + " Sekunden");
        EGL.LabelField("", GUI.skin.horizontalSlider);

        var width = GL.Width(20);

        _denominator = EGL.Popup("Denominator", _denominator, commonDenominators);

        EGL.BeginHorizontal();
        EGL.BeginVertical(width);
        EGL.LabelField("←", width);
        EGL.LabelField("↓", width);
        EGL.LabelField("↑", width);
        EGL.LabelField("→", width);
        EGL.EndVertical();

        var defaultColor = GUI.color;
        var defaultTextColor = GUI.contentColor;
        GUI.contentColor = white;

        int noteButtons = (int) (0.9F * Screen.width / 40);
        noteButtons /= 2;
        noteButtons *= 2;

        for (var i = noteButtons; i >= - noteButtons; i--)
        {
            EGL.BeginVertical(width);
            var curNoteTime = pointer - (denominator * i);
            var arrowNotes = script.notes.Where(x => x.time == curNoteTime);
            for (var j = 0; j < 4; j++)
            {
                var curNoteKey = (noteKey) j;

                GUI.color = j switch
                {
                    0 when i == 0 => new Color(1.0F, 0.6F, 0.6F),
                    1 when i == 0 => new Color(0.9F, 0.6F, 1F),
                    2 when i == 0 => new Color(0.6F, 0.6F, 1.0F),
                    3 when i == 0 => new Color(0.6F, 1F, 0.6F),
                    0 => red,
                    1 => new Color(0.5F, 0.1F, 0.7F),
                    2 => new Color(0.3F, 0.3F, 1.0F),
                    3 => green,
                    _ => GUI.color
                };

                if (arrowNotes.Any(x => x.key == curNoteKey))
                {
                    if (GL.Button("X", width))
                    {
                        script.notes.RemoveAll (x => x.time == curNoteTime && x.key == curNoteKey);
                    }
                }
                else
                {
                    if(GL.Button("", width))
                    {
                        script.notes.Add(new Note(curNoteTime,curNoteKey));
                    }
                }
            }

            GUI.color = defaultColor;
            EGL.EndVertical();
        }
        GUI.contentColor = defaultTextColor;
        EGL.EndHorizontal();
        
        
        EGL.LabelField("", GUI.skin.horizontalSlider);
        var navToolbar = GL.Toolbar(-1, navOptions);

        switch (navToolbar)
        {
            case 0:
                script.notes = script.notes.OrderBy(x => x.time).ToList();
                pointer = script.notes.Count > 0 ? script.notes.First().time : 0;
                break;
            case 1:
                pointer -= 4.0F;
                break;
            case 2:
                pointer -= 2.0F;
                break;
            case 3:
                pointer -= 1.0F;
                break;
            case 4:
                pointer -= 0.5F;
                break;
            case 5:
                //Gets smaller number, if there is none get self or bigger, if there is none zero
                var lesser = script.notes.Where(x => x.time < pointer);
                if (!lesser.Any())
                {
                    lesser = script.notes.Where(x => x.time >= pointer);
                    if (!lesser.Any())
                    {
                        pointer = 0;
                        break;
                    }
                    pointer = lesser.OrderBy(x => x.time).First().time;
                    break;
                }
                pointer = lesser.OrderBy(x => x.time).Last().time;
                break;
            case 6:
                //Gets smaller number, if there is none get self or bigger, if there is none zero
                var greater = script.notes.Where(x => x.time > pointer);
                if (!greater.Any())
                {
                    greater = script.notes.Where(x => x.time <= pointer);
                    if (!greater.Any())
                    {
                        pointer = 0;
                        break;
                    }
                    pointer = greater.OrderBy(x => x.time).Last().time;
                    break;
                }
                pointer = greater.OrderBy(x => x.time).First().time;
                break;
            case 7:
                pointer += 0.5F;
                break;
            case 8:
                pointer += 1.0F;
                break;
            case 9:
                pointer += 2.0F;
                break;
            case 10:
                pointer += 4.0F;
                break;
            case 11:
                script.notes = script.notes.OrderBy(x => x.time).ToList();
                pointer = script.notes.Count > 0 ? script.notes.Last().time : 0;
                break;
        }
        
    }
    private Texture2D MakeBackgroundTexture(int width, int height, Color textureColor, RectOffset border, Color bordercolor)
    {
        int widthInner = width;
        width += border.left;
        width += border.right;
 
        Color[] pix = new Color[ width * (height + border.top + border.bottom)];
 
 
       
        for(int i = 0; i < pix.Length; i++) {
            if(i < (border.bottom * width) )
                pix[i] = bordercolor;
            else if(i >= ( (border.bottom * width) + (height * width)) )  //Border Top
                pix[i] = bordercolor;
            else { //Center of Texture
 
                if( (i%width) < border.left) // Border left
                    pix[i] = bordercolor;
                else if ( (i%width) >= (border.left+widthInner) ) //Border right
                    pix[i] = bordercolor;
                else
                    pix[i] = textureColor;    //Color texture
            }
        }  
 
        Texture2D result = new Texture2D(width, height + border.top + border.bottom);
        result.SetPixels(pix);     
        result.Apply();
               
       
        return result;
       
    }

}

#endif