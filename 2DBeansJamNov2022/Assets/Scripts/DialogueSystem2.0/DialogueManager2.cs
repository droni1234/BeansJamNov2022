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

    private FightTrigger fight;

    Message[] currentMessage;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;
    public static DialogueManager2 instance;

    private CanvasGroup chatboxCanvasGroup;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {

        animator.SetBool("IsOpen", true);
        chatboxCanvasGroup.alpha = 1F;
        chatboxCanvasGroup.blocksRaycasts = true;
        currentMessage = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        fight = null;
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
        if(activeMessage < currentMessage?.Length)
        {
            DisplayMessage();
        } else
        {
            isActive = false;
            EndDialogue();
        }
    }

    private void Awake() 
    {
        DialogueManager2.instance = this;
        chatboxCanvasGroup = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && isActive == true)
        {
            NextMessage();
        }
    }

    public void Fight(Message[] messages, Actor[] actors, FightTrigger fight)
    {
        OpenDialogue(messages, actors);
        this.fight = fight;
    }
    // ReSharper disable Unity.PerformanceAnalysis
    void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
        chatboxCanvasGroup.alpha = 0F;
        chatboxCanvasGroup.blocksRaycasts = false;
        if (fight)
        {
            MusicTransition.instance.GetComponent<AudioSource>().Stop();
            FightTriggerMaster.instance.Fight(fight.battle, fight.levelname);
        }
    }

}
