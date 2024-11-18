using System.Collections.Generic;
using UnityEngine;

public class Inventry : MonoBehaviour
{
    public static Inventry instance;
    InventryUI inventryUI;
    private void Awake()
    {
        if(instance== null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        inventryUI = GetComponent<InventryUI>();
        inventryUI.UpdateUI();
    }

    public List<Item> items = new List<Item>();

    public void Add(Item item) 
    {
        items.Add(item);
        inventryUI.UpdateUI();
    }

    public void Remove(Item item) 
    {
        items.Remove(item);
        inventryUI.UpdateUI();
    }
}