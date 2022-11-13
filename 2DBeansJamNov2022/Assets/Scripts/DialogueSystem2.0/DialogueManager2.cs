using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager2 : MonoBehaviour
{

    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public Image textBox;

    public Animator animator;

    public AudioSource voiceover;

    Message[] currentMessage;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;
    public static DialogueManager2 instance;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {

        animator.SetBool("IsOpen", true);
        currentMessage = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        Debug.Log("Started conversation! Loaded messages: " + messages.Length);
        DisplayMessage();
        
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessage[activeMessage];
        messageText.text = messageToDisplay.message;
        voiceover.clip = messageToDisplay.ton;
        voiceover.Play();

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        textBox.sprite = actorToDisplay.ui;
        
    }

    public void NextMessage()
    {
        activeMessage++;
        if(activeMessage < currentMessage.Length)
        {
            DisplayMessage();
        } else
        {
            Debug.Log("Conversation ended!");
            isActive = false;
            EndDialogue();
        }
    }

    private void Awake() 
    {
        DialogueManager2.instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isActive == true)
        {
            NextMessage();
        }
    }

    void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
	}

}
