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

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;

    public GameObject targetGameObject1;
    public GameObject targetGameObject2;
    public GameObject targetGameObject3;
    public GameObject targetGameObject4;

    private bool isSpawned1 = false;
    private bool isSpawned2 = false;
    private bool isSpawned3 = false;
    private bool isSpawned4 = false;

    private Transform playerTransform;

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

        GameObject player = GameObject.Find("PlayerPMC");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found. Make sure the Player object is correctly named.");
        }
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
            ActivateOrSpawnTargetObjects();
        }
    }

    public void ActivateOrSpawnTargetObjects()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("PlayerTransform is null. Cannot spawn prefabs.");
            return;
        }

        if (!string.IsNullOrEmpty(itemToCheck1) && prefab1 != null && ContainsItem(itemToCheck1) && !isSpawned1)
        {
            targetGameObject1 = Instantiate(prefab1, playerTransform.position + Vector3.forward * 2, Quaternion.identity);
            isSpawned1 = true;
        }
        if (!string.IsNullOrEmpty(itemToCheck2) && prefab2 != null && ContainsItem(itemToCheck2) && !isSpawned2)
        {
            targetGameObject2 = Instantiate(prefab2, playerTransform.position + Vector3.right * 2, Quaternion.identity);
            isSpawned2 = true;
        }
        if (!string.IsNullOrEmpty(itemToCheck3) && prefab3 != null && ContainsItem(itemToCheck3) && !isSpawned3)
        {
            targetGameObject3 = Instantiate(prefab3, playerTransform.position + Vector3.left * 2, Quaternion.identity);
            isSpawned3 = true;
        }
        if (!string.IsNullOrEmpty(itemToCheck4) && prefab4 != null && ContainsItem(itemToCheck4) && !isSpawned4)
        {
            targetGameObject4 = Instantiate(prefab4, playerTransform.position + Vector3.back * 2, Quaternion.identity);
            isSpawned4 = true;
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
