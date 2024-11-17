using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventry : MonoBehaviour
{
    public static Inventry instance;
    InventryUI inventryUI;
    public List<Item> items = new List<Item>();
    // Start is called before the first frame update
    void Start()
    {
        inventryUI = GetComponent<InventryUI>();
        inventryUI.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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
