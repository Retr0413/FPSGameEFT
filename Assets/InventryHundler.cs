using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    private Canvas canvasComponent; // Canvasコンポーネントを参照
    private bool system = false;   // Canvasの有効/無効状態を管理

    // Startは最初のフレームで呼び出される
    void Start()
    {
        // Canvasコンポーネントを取得
        canvasComponent = GetComponent<Canvas>();

        if (canvasComponent != null)
        {
            // 初期状態でCanvasを無効に設定
            canvasComponent.enabled = false;
        }

        // 初期状態でマウスカーソルを非表示にしてロック
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Updateは毎フレーム呼び出される
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // Iキーを押すと切り替え
        {
            if (canvasComponent != null)
            {
                // Canvasコンポーネントの有効/無効を切り替え
                system = !system;
                canvasComponent.enabled = system;

                if (system)
                {
                    // Canvasが有効になった場合
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    // Canvasが無効になった場合
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
}
