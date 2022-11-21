using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using whip.battle;

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

    private IEnumerator messagePrinter;

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
        DelayFirstMessage();
        
    }


    void DelayFirstMessage()
    {
        Message messageToDisplay = currentMessage[activeMessage];
        //messageText.text = messageToDisplay.message;
        voiceover.clip = messageToDisplay.ton;
        voiceover.PlayDelayed(0.5F);

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        StopCoroutine(messagePrinter);
        messagePrinter = fillBox(messageToDisplay.message, 0.05F);
        StartCoroutine(messagePrinter);
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        textBox.sprite = actorToDisplay.ui;
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessage[activeMessage];
        //messageText.text = messageToDisplay.message;
        voiceover.clip = messageToDisplay.ton;
        voiceover.Play();

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        StopCoroutine(messagePrinter);
        messagePrinter = fillBox(messageToDisplay.message, 0.05F);
        StartCoroutine(messagePrinter);
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        actorImage.color = actorImage.sprite ? Color.white : Color.clear;
        textBox.sprite = actorToDisplay.ui;
        
    }

    IEnumerator fillBox(string text, float delay)
    {
        messageText.text = "";
        foreach (var c in text)
        {
            messageText.text += c;
            yield return new WaitForSeconds(delay);
        } 
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
        messagePrinter = fillBox(null, 0);
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
