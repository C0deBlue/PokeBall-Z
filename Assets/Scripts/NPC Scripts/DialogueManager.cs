using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public static DialogueManager Instance { get; private set; }
    private bool dialogueActive;
    private bool dialogueJustStarted;
    private NPCBehavior talkingNPC;
    private bool busy;
    private void Awake()
    {
        // If there is already an instance that is not me, seppuku
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        dialogueActive = false;
        dialogueJustStarted = false;
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (dialogueJustStarted)
                dialogueJustStarted = false;
            else if (dialogueActive)
                DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue, NPCBehavior npc)
    {
        // Can't start another convo if busy
        if (busy)
            return;
        Debug.Log("Starting conversation with: " + dialogue.name);
        busy = true;                // Can no longer start a new convo
        // Clear previosu convo
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
        dialogueActive = true;
        dialogueJustStarted = true;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
    }

    private void EndDialogue()
    {
        Debug.Log("Conversation ended");
        dialogueActive = false;
        busy = false;
    }

}
