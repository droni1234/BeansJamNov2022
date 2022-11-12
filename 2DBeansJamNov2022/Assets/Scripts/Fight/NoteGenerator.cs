using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        notes.OrderBy(x => x.time);
        foreach (Note note in notes)
        {
            switch (note.key)
            {
                case noteKey.left:
                    Instantiate(left, leftReference.transform.position + Vector3.up * note.time, left.transform.rotation, transform);
                    break;
                case noteKey.down:
                    Instantiate(down, downReference.transform.position + Vector3.up * note.time, down.transform.rotation, transform);
                    break;
                case noteKey.up:
                    Instantiate(up, upReference.transform.position + Vector3.up * note.time, up.transform.rotation, transform);
                    break;
                case noteKey.right:
                    Instantiate(right, rightReference.transform.position + Vector3.up * note.time, right.transform.rotation, transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

}
