using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.EditorCoroutines.Editor;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using static whip.battle.edit.noteKey;
using static UnityEngine.Color;
using EGL = UnityEditor.EditorGUILayout;
using GL = UnityEngine.GUILayout;
using Object = UnityEngine.Object;

#if UNITY_EDITOR

namespace whip.battle.edit
{
    [CustomEditor(typeof(BattleObject))]
    public class BattleObjectEditor : Editor
    {

        public override bool RequiresConstantRepaint() => _refreshrate;
        private bool _refreshrate = false;
        
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
            "1 \u2215 8",
            "1 \u2215 16"
        };

        private string[] NoteOptions =
        {
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

        private bool showTimelineEdit;
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
                    case 8:
                        return 4F / 16F;
                }
            }
        }

        private float beat
        {
            get => _beat;
            set => _beat = (1F / (value / 60F));
        }
        private float _beat;

        private int _denominator
        {
            get => ((BattleObject) target).denominator;
            set => ((BattleObject) target).denominator = value;
        }

        private int timelineTime = 0;
        private EditorCoroutine timer;
        private int startTimeMusic = 0;
        
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

            if (beat == 0)
            {
                beat = script.bpm;
            }

            if (EditorSFX.IsPreviewClipPlaying())
                timelineTime = (int) (((float) EditorSFX.GetClipSamplePosition() - startTimeMusic) / 44100F /
                                      (beat * denominator));
            else
                timelineTime = 0;

            if (GL.Button("⏵Play Audio"))
            {
                AssetDatabase.Refresh();
                _refreshrate = true;
                EditorSFX.StopAllClips();
                startTimeMusic = (int) ((1F / (script.bpm / 60F)) * (pointer) * 44100F);
                EditorSFX.PlayClip(script.audio, startTimeMusic);
            }
            
            if (GL.Button("Pause Here Audio"))
            {
                _refreshrate = false;
                pointer = EditorSFX.GetClipSamplePosition() / 44100F / beat;
                pointer -= (pointer % denominator);
                EditorSFX.StopAllClips();
            }

            if (GL.Button("Reset Audio"))
            {
                _refreshrate = false;
                EditorSFX.StopAllClips();
            }


            showTimelineEdit = EGL.Foldout(showTimelineEdit, "Timeline Editor");
            if (showTimelineEdit)
                navigation();

            EGL.LabelField("", GUI.skin.horizontalSlider);

            showNoteEdit = EGL.Foldout(showNoteEdit, "Notes");
            if (showNoteEdit)
            {
                var noteGrid = GL.Toolbar(-1, NoteOptions, GL.Height(40));
                if (noteGrid != -1)
                    script.addNote(pointer, (noteKey) noteGrid);
            }


            EGL.LabelField("", GUI.skin.horizontalSlider);

            showFocusEdit = EGL.Foldout(showFocusEdit, "Focus");
            if (showFocusEdit)
            {
                var lookGrid = GL.Toolbar(-1, lookOptions);

                if (lookGrid != -1)
                    script.addFocus(pointer, (POI) lookGrid);
            }




            EGL.LabelField("", GUI.skin.horizontalSlider);

            showSpriteEdit = EGL.Foldout(showSpriteEdit, "Sprites");
            if (showSpriteEdit)
            {

                charSelection = (CharacterType) EGL.EnumPopup("Select Character:", charSelection);
                float preview_Size = (Screen.width - 30) / spriteDivider;
                Sprite[] selectedSprites = charSelection switch
                {
                    CharacterType.Player => script.player.sprites,
                    CharacterType.Enemy => script.enemy.sprites,
                    _ => throw new ArgumentOutOfRangeException()
                };

                GUILayoutOption[] horizontalOptions =
                    {GL.MaxHeight(preview_Size), GL.MaxWidth(preview_Size * spriteDivider)};
                GUILayoutOption[] buttonOptions =
                    {GL.MaxWidth(preview_Size), GL.MinHeight(preview_Size), GL.MaxWidth(preview_Size)};

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
            var script = (BattleObject) target;
            EGL.LabelField("Notes:");
            GL.BeginHorizontal();
            foreach (Note note in script.notes.Where(x => x.time == pointer).ToList())
            {
                if (GL.Button(note.key.ToString()))
                {
                    script.notes.Remove(note);
                    EditorUtility.SetDirty(script);
                }
            }

            GL.EndHorizontal();


            EGL.LabelField("Sprites Player");
            GL.BeginHorizontal();
            foreach (SetSprite sprite in script.sprites.Where(x => x.time == pointer && x.type == CharacterType.Player)
                         .ToList())
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
                     from sprite in script.sprites.Where(x => x.time == pointer && x.type == CharacterType.Enemy)
                         .ToList()
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
            EGL.LabelField("Zeit im Track: " + beat * pointer + " Sekunden");
            EGL.LabelField("", GUI.skin.horizontalSlider);

            var width = GL.Width(20);

            _denominator = EGL.Popup("Denominator", _denominator, commonDenominators);

            EGL.BeginVertical();
            EGL.BeginHorizontal(width);
            EGL.LabelField("←", width);
            EGL.LabelField("↓", width);
            EGL.LabelField("↑", width);
            EGL.LabelField("→", width);
            EGL.EndHorizontal();

            var defaultColor = GUI.color;
            var defaultTextColor = GUI.contentColor;
            GUI.contentColor = white;

            int noteButtons = (int) (0.6F * Screen.height / 40);
           
            for (var i = -noteButtons - timelineTime; i <= 3 - timelineTime; i++)
            {
                EGL.BeginHorizontal(width);
                var curNoteTime = pointer - (denominator * i);
                var arrowNotes = script.notes.Where(x => x.time == curNoteTime);
                for (var j = 0; j < 4; j++)
                {
                    var curNoteKey = (noteKey) j;

                    GUI.color = j switch
                    {
                        0 when i == 0 - timelineTime => new Color(1.0F, 0.6F, 0.6F),
                        1 when i == 0 - timelineTime => new Color(0.9F, 0.6F, 1F),
                        2 when i == 0 - timelineTime => new Color(0.6F, 0.6F, 1.0F),
                        3 when i == 0 - timelineTime => new Color(0.6F, 1F, 0.6F),
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
                            script.notes.RemoveAll(x => x.time == curNoteTime && x.key == curNoteKey);
                            EditorUtility.SetDirty(script);
                        }
                    }
                    else
                    {
                        if (GL.Button("", width))
                        {
                            script.notes.Add(new Note(curNoteTime, curNoteKey));
                            EditorUtility.SetDirty(script);
                        }
                    }
                }

                GUI.color = defaultColor;
                EGL.EndHorizontal();
            }

            GUI.contentColor = defaultTextColor;
            EGL.EndVertical();


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

    }
}
#endif