using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class InventoryObjectDetails
    {
        public Image objectImage;
        public ScriptableInventoryObject objectInSlot;
        public int quantity = 0;
        public bool newItem = false;
    }

    public List<InventoryObjectDetails> activeInventory;

    [Header("Canvas Management")]
    public RectTransform inventoryPlacePrefab;
    public RectTransform horizontalLinePrefab;
    public RectTransform verticalLinePrefab;
    public CanvasGroup inventoryGroup;
    public Canvas canvas;
    public int numColumns;
    public int numRows;
    public float rectWidth;
    public float rectHeight;
    public bool generate = false;
    Vector2 sizePerSquare = new Vector2(1.0f, 1.0f);

    public void Start()
    {
        if (generate)
        {
            RectTransform rt = horizontalLinePrefab.parent as RectTransform;

            activeInventory = new List<InventoryObjectDetails>(numColumns * numRows);
            for (int row = 0; row < numRows; row++)
            {
                for (int column = 0; column < numColumns; column++)
                {
                    if (row == 0)
                    {
                        RectTransform newColumnObject = Instantiate(verticalLinePrefab, verticalLinePrefab.parent);
                        newColumnObject.anchoredPosition = new Vector2(((rectWidth - 5.0f) / numColumns) * (column + 1), newColumnObject.anchoredPosition.y);
                    }
                }

                if (row < numRows - 1)
                {
                    RectTransform newRowObject = Instantiate(horizontalLinePrefab, horizontalLinePrefab.parent);
                    newRowObject.anchoredPosition = new Vector2(newRowObject.anchoredPosition.x, -((rectHeight - 5.0f) / numRows) * (row + 1));
                }
            }
        }
    }

    public void ToggleInventory()
    {
        if (inventoryGroup.blocksRaycasts)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        inventoryGroup.alpha = 1.0f;
        inventoryGroup.blocksRaycasts = true;
    }

    public void CloseInventory()
    {
        inventoryGroup.alpha = 0.0f;
        inventoryGroup.blocksRaycasts = false;
    }

    public bool PickUpObject(ScriptableInventoryObject newObject)
    {
        int openSlot = GetFirstOpenSlot();
        if (openSlot == -1)
        {
            Debug.LogError("Failed to pick up " + newObject.name + " because no slots are open");
            return false;
        }

        Vector2 inventorySize = newObject.GetInventorySize();
        activeInventory[openSlot].objectInSlot = newObject;
        activeInventory[openSlot].objectImage.sprite = newObject.GetInventorySprite();
        activeInventory[openSlot].objectImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventorySize.x);
        activeInventory[openSlot].objectImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventorySize.y);
        return true;
    }

    public bool RemoveObject(ScriptableInventoryObject objectToRemove)
    {
        int index = FindIndexOfObject(objectToRemove); // Will return as -1 if it doesn't exist
        if (index != -1)
        {
            activeInventory[index].objectInSlot = null;
            return true;
        }
        return false;
    }

    public int GetFirstOpenSlot()
    {
        int index = FindIndexOfObject(null); // Will return as -1 if it doesn't exist
        return index;
    }

    public bool IsObjectInInventory(ScriptableInventoryObject objectToCheck)
    {
        int index = FindIndexOfObject(objectToCheck);
        return index != -1;
    }

    public int FindIndexOfObject(ScriptableInventoryObject objectToCheck)
    {
        for (int i = 0; i < activeInventory.Count; i++)
        {
            if (activeInventory[i].objectInSlot == objectToCheck)
            {
                return i;
            }
        }
        return -1;
    }
}
