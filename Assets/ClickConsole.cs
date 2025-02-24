using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickConsole : MonoBehaviour
{
    void Start()
    {
        // マウスカーソルを表示
        Cursor.lockState = CursorLockMode.None; // カーソルをロックしない
        Cursor.visible = true;                 // カーソルを表示
    }

    void Update()
    {
        // カーソルが隠れないように常に設定を確認
        if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None; // カーソルをロックしない
            Cursor.visible = true;                 // カーソルを表示
        }
    }
}
