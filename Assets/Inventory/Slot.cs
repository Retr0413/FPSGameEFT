using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image icon;
    Item item;
    public GameObject removeButton;
    
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
        removeButton.SetActive(true);
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.SetActive(false);
    }

    public void OnRemoveButton()
    {
        Inventry.instance.Remove(item);
    }

    public void UseItem()
    {
        if (item == null)
        {
            return;
        }
        item.Use();
    }
}
