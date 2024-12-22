using System.Collections.Generic;
using UnityEngine;

public class Inventry : MonoBehaviour
{
    public static Inventry instance;
    private InventryUI inventryUI;

    public string itemToCheck1;
    public string itemToCheck2;
    public string itemToCheck3;
    public string itemToCheck4;
    public GameObject targetGameObject1;
    public GameObject targetGameObject2;
    public GameObject targetGameObject3;
    public GameObject targetGameObject4;

    private bool isActivated1 = false;
    private bool isActivated2 = false;
    private bool isActivated3 = false;
    private bool isActivated4 = false;

    private void Awake()
    {
        if (instance == null)
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

    public List<Item> GetItems()
    {
        return items;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateTargetObjects();
        }
    }

    public void ActivateTargetObjects()
    {
        if (!string.IsNullOrEmpty(itemToCheck1) && targetGameObject1 != null && ContainsItem(itemToCheck1) && !isActivated1)
        {
            targetGameObject1.SetActive(true);
            isActivated1 = true;
        }
        if (!string.IsNullOrEmpty(itemToCheck2) && targetGameObject2 != null && ContainsItem(itemToCheck2) && !isActivated2)
        {
            targetGameObject2.SetActive(true);
            isActivated2 = true;
        }
        if (!string.IsNullOrEmpty(itemToCheck3) && targetGameObject3 != null && ContainsItem(itemToCheck3) && !isActivated3)
        {
            targetGameObject3.SetActive(true);
            isActivated3 = true;
        }
        if (!string.IsNullOrEmpty(itemToCheck4) && targetGameObject4 != null && ContainsItem(itemToCheck4) && !isActivated4)
        {
            targetGameObject4.SetActive(true);
            isActivated4 = true;
        }
    }

    public bool ContainsItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.name == itemName)
            {
                return true;
            }
        }
        return false;
    }
}
