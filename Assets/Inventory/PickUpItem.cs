using UnityEngine;
using UnityEngine.UI;
public class PickUpItem : MonoBehaviour
{
    //Itemデータを入れる
    public Item item;
    
    void Start()
    {
        //設定したアイコンを表示させる
        GetComponent<Image>().sprite = item.icon;
    }

//インベントリにアイテムを追加
    public void PickUp()
    {
        Inventry.instance.Add(item);
        Destroy(gameObject);
    }
}