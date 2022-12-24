using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCChatManager : MonoBehaviour
{
    public enum GameEvents
    {
        FoundKey = 1,
        OpenedHouse = 2,
    }

    public enum GameNPCs
    {
        KameTurtle,
    }

    public static List<GameEvents> eventsActive = new List<GameEvents>();
    public ScriptableNPCChat[] allChats;
    public Transform[] npcTransforms;

    [Header("Chat Box")]
    public CanvasGroup chatGroup;
    public GameObject boxBase;
    public TextMeshProUGUI dialogueBoxText;

    public bool dialogueOpen = false;

    public float chatStartDistance = 2.0f;
    public float chatEndDistance = 2.0f;
    public float charactersPerSecond = 30;

    int nearbyNPC;
    int currentLine = 0;
    float currentCharacter;
    ScriptableNPCChat activeChat;

    public void Update()
    {
        if (!dialogueOpen)
        {
            nearbyNPC = GetNearbyNPC();
            if (nearbyNPC != -1 && NPCHasAChat(nearbyNPC))
            {
                dialogueOpen = true;
                currentLine = 0;
                dialogueBoxText.text = activeChat.linesSpoken[currentLine];
                dialogueBoxText.maxVisibleCharacters = 0;
            }
        }
        else
        {
            if (Vector3.Distance(npcTransforms[nearbyNPC].position, PlayerMovement.playerTransform.position) > chatEndDistance)
            {
                currentCharacter = 0.0f;
                dialogueOpen = false;
            }
        }

        chatGroup.alpha = InventoryManager.instance.inventoryGroup.alpha > 0.0f ? 0.0f : 1.0f;
        boxBase.SetActive(dialogueOpen);
        if (dialogueOpen)
        {
            currentCharacter = Mathf.Clamp(currentCharacter + charactersPerSecond * Time.deltaTime, 0.0f, activeChat.linesSpoken[currentLine].Length);
            dialogueBoxText.maxVisibleCharacters = (int)currentCharacter;
        }
    }

    public static void ActivateEvent(GameEvents eventToActivate)
    {
        if (!eventsActive.Contains(eventToActivate))
        {
            eventsActive.Add(eventToActivate);
        }
    }

    public int GetNearbyNPC()
    {
        for (int i = 0; i < npcTransforms.Length; i++)
        {
            if (Vector3.Distance(PlayerMovement.playerTransform.position, npcTransforms[i].position) < chatStartDistance)
            {
                return i;
            }
        }
        return -1;
    }

    public bool NPCHasAChat(int npc)
    {
        activeChat = GetAvailableChatFor((GameNPCs)npc);
        return activeChat != null;
    }

    public ScriptableNPCChat GetAvailableChatFor(GameNPCs npc)
    {
        ScriptableNPCChat activeChat = null;
        for (int i = 0; i < allChats.Length; i++)
        {
            if (allChats[i].npcSpeaking == npc && ChatEventsComplete(allChats[i]) && (activeChat == null || activeChat.priority < allChats[i].priority))
            {
                activeChat = allChats[i];
            }
        }
        return activeChat;
    }

    public bool ChatEventsComplete(ScriptableNPCChat chat)
    {
        for (int i = 0; i < chat.eventsNeeded.Length; i++)
        {
            if (!eventsActive.Contains(chat.eventsNeeded[i]))
            {
                return false;
            }
        }
        return true;
    }
}
