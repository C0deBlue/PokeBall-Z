using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public float borderThickness = 5.0f;
    public Vector2 borderSize = new Vector2(10.0f, 10.0f);
    public bool generate = false;

    public Color normalColor = Color.white;
    public Color selectColor = Color.yellow;

    Vector2 sizePerSquare = new Vector2(1.0f, 1.0f);
    Vector2 sizePerSquareNoBorder = new Vector2(1.0f, 1.0f);
    Vector2 upperLeft = new Vector2(1.0f, 1.0f);

    [Header("Selection")]
    public GameObject selectionTextBox;
    public TextMeshProUGUI selectionDescription;

    int lastClickedID = -1;

    public void Start()
    {
        if (generate)
        {
            RectTransform rt = horizontalLinePrefab.parent as RectTransform;
            sizePerSquare.x = rectWidth / numColumns;
            sizePerSquare.y = rectHeight / numRows;

            sizePerSquareNoBorder.x = (rectWidth - 5.0f * (numColumns + 1)) / numColumns;
            sizePerSquareNoBorder.y = (rectHeight - 5.0f * (numRows + 1)) / numRows;

            upperLeft.x = -rectWidth * 0.5f;
            upperLeft.y = rectHeight * 0.5f;

            activeInventory = new List<InventoryObjectDetails>();
            for (int row = 0; row < numRows; row++)
            {
                for (int column = 0; column < numColumns; column++)
                {
                    if (row == 0)
                    {
                        RectTransform newColumnObject = Instantiate(verticalLinePrefab, verticalLinePrefab.parent);
                        newColumnObject.anchoredPosition = new Vector2(((rectWidth - 5.0f) / numColumns) * (column + 1), newColumnObject.anchoredPosition.y);
                    }

                    activeInventory.Add(new InventoryObjectDetails());

                    RectTransform newInventorySpot = Instantiate(inventoryPlacePrefab, inventoryPlacePrefab.parent);
                    newInventorySpot.anchoredPosition = new Vector2(upperLeft.x + sizePerSquareNoBorder.x * (column + 0.5f) + borderThickness * (column + 1), 
                                                                    upperLeft.y - sizePerSquareNoBorder.y * (row + 0.5f) - borderThickness * (row + 1));
                    newInventorySpot.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizePerSquareNoBorder.x - borderSize.x);
                    newInventorySpot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizePerSquareNoBorder.y - borderSize.y);
                    activeInventory[activeInventory.Count - 1].objectImage = newInventorySpot.GetComponent<Image>();
                }

                if (row < numRows - 1)
                {
                    RectTransform newRowObject = Instantiate(horizontalLinePrefab, horizontalLinePrefab.parent);
                    newRowObject.anchoredPosition = new Vector2(newRowObject.anchoredPosition.x, -((rectHeight - 5.0f) / numRows) * (row + 1));
                }
            }
        }
    }

    public void SquareClicked(int id)
    {
        if (activeInventory[id].objectInSlot != null)
        {
            activeInventory[id].objectImage.color = selectColor;
            if (lastClickedID != -1)
            {
                activeInventory[lastClickedID].objectImage.color = normalColor;
            }
            lastClickedID = id;

            selectionTextBox.SetActive(true);
            selectionDescription.text = activeInventory[id].objectInSlot.GetDescription();
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
        selectionTextBox.SetActive(false);

        if (lastClickedID != -1)
        {
            activeInventory[lastClickedID].objectImage.color = normalColor;
        }
        lastClickedID = -1;
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
