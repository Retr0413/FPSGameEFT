using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PichUpItem : MonoBehaviour
{
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = item.icon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }
}
