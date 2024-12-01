using System.Collections.Generic;
using UnityEngine;

public class Inventry : MonoBehaviour
{
    public static Inventry instance;
    private InventryUI inventryUI;

    // 特定のアイテムを指定するための変数
    public string itemToCheck1;
    public string itemToCheck2;
    public string itemToCheck3;
    public string itemToCheck4;
    public GameObject targetGameObject1;
    public GameObject targetGameObject2;
    public GameObject targetGameObject3;
    public GameObject targetGameObject4;
    

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

        // 初期状態で特定のアイテムをチェックしてオブジェクトを有効化
        CheckAndActivateItem();
    }

    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        items.Add(item);
        inventryUI.UpdateUI();

        // アイテムを追加した際にチェック
        CheckAndActivateItem();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        inventryUI.UpdateUI();

        // アイテムを削除した際に再チェック
        CheckAndActivateItem();
    }

    public List<Item> GetItems()
    {
        return items;
    }

    // 特定のアイテムが存在するか確認し、対象オブジェクトを有効化する
    public void CheckAndActivateItem()
    {
        if (!string.IsNullOrEmpty(itemToCheck1) && targetGameObject1 != null)
        {
            bool itemExists = ContainsItem(itemToCheck1);
            targetGameObject1.SetActive(itemExists);
        }
        if (!string.IsNullOrEmpty(itemToCheck2) && targetGameObject2 != null)
        {
            bool itemExists = ContainsItem(itemToCheck2);
            targetGameObject2.SetActive(itemExists);
        }
        if (!string.IsNullOrEmpty(itemToCheck3) && targetGameObject3 != null)
        {
            bool itemExists = ContainsItem(itemToCheck3);
            targetGameObject3.SetActive(itemExists);
        }
        if (!string.IsNullOrEmpty(itemToCheck4) && targetGameObject4 != null)
        {
            bool itemExists = ContainsItem(itemToCheck4);
            targetGameObject4.SetActive(itemExists);
        }
    }

    // 指定されたアイテム名が存在するか確認
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
