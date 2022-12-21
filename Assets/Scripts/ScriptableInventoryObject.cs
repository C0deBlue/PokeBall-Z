using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObject", menuName = "Inventory/Create Inventory Object", order = 1)]
public class ScriptableInventoryObject : ScriptableObject
{
    [Header("Base Info")]
    [SerializeField] Sprite inventorySprite;
    [SerializeField] Vector2 inventorySize;
    [SerializeField] string inventoryDescription;
    [SerializeField] string shortName;

    [Header("Stacks")]
    [SerializeField] bool stackable = false;
    [SerializeField] int maxStacks = 999;

    public virtual Sprite GetInventorySprite()
    {
        return inventorySprite;
    }

    public virtual Vector2 GetInventorySize()
    {
        return inventorySize;
    }

    public virtual string GetDescription()
    {
        return inventoryDescription;
    }

    public virtual string GetShortName()
    {
        return shortName;
    }
}
