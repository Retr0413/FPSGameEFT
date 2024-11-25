using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public GameObject inventory;
    public bool system = false;

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態でマウスカーソルを非表示にする
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // GetKeyからGetKeyDownに変更
        {
            if (system == false)
            {
                inventory.SetActive(true);
                system = true;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                inventory.SetActive(false);
                system = false;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
