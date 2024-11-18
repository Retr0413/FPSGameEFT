using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 

public class Slot : MonoBehaviour
{
    public Image icon;
    public GameObject removeButton;
    public List<GameObject> targetObjects; 
    Item item;

    // アイテムを追加する
    public void AddItem(Item newItem) 
    {
        item = newItem;
        icon.sprite = newItem.icon;
        removeButton.SetActive(true);
    }

    // アイテムを取り除く
    public void ClearItem()
    {
        item = null;
        icon.sprite = null;
        removeButton.SetActive(false);
    }

    // アイテムの消去ボタン
    public void OnRemoveButton()
    {
        Inventry.instance.Remove(item);

        foreach (GameObject obj in targetObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    // アイテムの使用ボタン
    public void UseItem() 
    {
        if (item == null) 
        {
            return;
        }
        item.Use();
    }
}
