using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "NPC/Create NPC Dialogue", order = 1)]
public class ScriptableNPCChat : ScriptableObject
{
    public string[] linesSpoken;
    public NPCChatManager.GameEvents[] eventsNeeded;
    public NPCChatManager.GameNPCs npcSpeaking;

    [Tooltip("A higher priority will prevent other NPC chats from displaying while this one is active")]
    public int priority = 0;

    public int GetNumLines()
    {
        return linesSpoken.Length;
    }

    public string GetLine(int index)
    {
        return linesSpoken[index];
    }
}
