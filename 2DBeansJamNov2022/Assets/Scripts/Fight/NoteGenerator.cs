using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using whip.battle.edit;

namespace whip.battle
{
    public class NoteGenerator : MonoBehaviour
    {

        public BattleObject battle;

        public GameObject left;
        public GameObject right;
        public GameObject down;
        public GameObject up;

        public GameObject leftReference;
        public GameObject rightReference;
        public GameObject downReference;
        public GameObject upReference;

        public void Generate(List<Note> notes)
        {
            battle = FightSystem.instance.battle;
            notes.OrderBy(x => x.time);
            foreach (Note note in notes)
            {
                NoteObject noteObject;
                switch (note.key)
                {
                    case noteKey.left:
                        noteObject = Instantiate(left, leftReference.transform.position + Vector3.up * note.time,
                            left.transform.rotation, transform).GetComponent<NoteObject>();
                        noteObject.reference = leftReference.transform;
                        break;
                    case noteKey.down:
                        noteObject = Instantiate(down, downReference.transform.position + Vector3.up * note.time,
                            down.transform.rotation, transform).GetComponent<NoteObject>();
                        noteObject.reference = downReference.transform;
                        break;
                    case noteKey.up:
                        noteObject = Instantiate(up, upReference.transform.position + Vector3.up * note.time,
                            up.transform.rotation, transform).GetComponent<NoteObject>();
                        noteObject.reference = upReference.transform;
                        break;
                    case noteKey.right:
                        noteObject = Instantiate(right, rightReference.transform.position + Vector3.up * note.time,
                            right.transform.rotation, transform).GetComponent<NoteObject>();
                        noteObject.reference = rightReference.transform;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                noteObject.time = note.time;
                noteObject.bpm = battle.bpm;
                noteObject.beat = 1F / (noteObject.bpm / 60);
            }
        }

    }
}