using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventryUI : MonoBehaviour
{
    public Transform InventryPanel;
    Slot[] slots;
    // Start is called before the first frame update
    void Start()
    {
        slots = InventryPanel.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventry.instance.items.Count)
            {
                slots[i].AddItem(Inventry.instance.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
