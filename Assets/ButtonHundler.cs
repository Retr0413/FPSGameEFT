using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHundler : MonoBehaviour
{
    public StartMove targetObject; // StartMove スクリプトを持つオブジェクトの参照

    // ボタンが押された際に呼び出すメソッド
    public void OnButtonClicked()
    {
        Debug.Log("Button Clicked");
        if (targetObject != null)
        {
            targetObject.OnButtonPressed(); // StartMove の関数を呼び出し
        }
        else
        {
            Debug.LogWarning("Target object is not set.");
        }
    }
}
