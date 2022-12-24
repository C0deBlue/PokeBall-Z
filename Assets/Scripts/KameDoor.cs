using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameDoor : MonoBehaviour
{
    public ScriptableInventoryObject kameKey;
    public Transform doorTransform;

    public float openDistance = 3.0f;
    public bool open = false;

    public void Update()
    {
        if (!open && 
            Vector3.Distance(doorTransform.position, PlayerMovement.instance.transform.position) < openDistance &&
            InventoryManager.instance.IsObjectInInventory(kameKey))
        {
            NPCChatManager.ActivateEvent(NPCChatManager.GameEvents.OpenedHouse);
            open = true;
            doorTransform.gameObject.SetActive(false);
        }
    }
}
