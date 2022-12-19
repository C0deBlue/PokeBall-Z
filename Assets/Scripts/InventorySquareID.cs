using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySquareID : MonoBehaviour
{
    public InventoryManager manager;
    public int id = 0;

    public void SquareClicked()
    {
        manager.SquareClicked(id);
    }
}
