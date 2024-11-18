using UnityEngine;

public class InventryUI : MonoBehaviour
{
    public Transform InventryPanel;

    Slot[] slots;
    
    void Start()
    {
        slots = InventryPanel.GetComponentsInChildren<Slot>();
        
    }

    // Update is called once per frame
  public void UpdateUI()
    {
        Debug.Log("UpdateUi");
        for(int i = 0; i < slots.Length; i++) 
        {
            if (i < Inventry.instance.items.Count) 
            {
                slots[i].AddItem(Inventry.instance.items[i]);
            
            }
            else 
            {
                slots[i].ClearItem();
            
            }
        
        }
    }
}