using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;

    public void StartDialogue()
    {
        print("test");
        DialogueManager2.instance.OpenDialogue(messages, actors);
    }
}
    [System.Serializable]
    public class Message 
    {
        public int actorID;
        public string message;

        public AudioClip ton;
    }

    [System.Serializable]
    public class Actor 
    {
        public string name;
        public Sprite sprite;
        public Sprite ui;
    }





