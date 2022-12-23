using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionRequirement : MonoBehaviour
{
    public virtual bool CanUseTransition()
    {
        return true;
    }
}
