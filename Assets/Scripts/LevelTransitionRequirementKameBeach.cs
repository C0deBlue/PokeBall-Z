using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionRequirementKameBeach : LevelTransitionRequirement
{
    public ScriptableInventoryObject kameKey;

    public override bool CanUseTransition()
    {
        return InventoryManager.instance.IsObjectInInventory(kameKey);
    }
}
