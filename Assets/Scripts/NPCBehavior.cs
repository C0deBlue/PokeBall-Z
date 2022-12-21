using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for NPC Behaviors
public enum Behavior { Stationary, LeftToRight, UpDown, Random }
/*
 * Defines the behavior of a variety of NPCs.
 * Can tweak how an NPC acts when player isn't near, when player is near, what NPC says,, if NPC has a quest to give
 * And so on
 */
public class NPCBehavior : MonoBehaviour
{
    public Behavior behavior;               // What behavior will the NPC have?
    public bool hasQuest;
    // public Quest quest



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (behavior)
        {
            case Behavior.Stationary:
              
                break;
            case Behavior.LeftToRight:
                LeftRightBehavior();
                break;
            case Behavior.UpDown:
                UpDownBehavior();
                break;
            case Behavior.Random:
                RandomBehavior();
                break;
                    
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // The player is near
        Debug.Log("COLLISION: " + collision.gameObject.tag);

        // Display what the NPC says when player is near...
    }

    private void LeftRightBehavior()
    {

    }

    private void UpDownBehavior()
    {

    }

    private void RandomBehavior()
    {

    }
}
