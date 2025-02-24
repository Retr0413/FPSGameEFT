using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager を使用するために追加

public class ButtonHundler : MonoBehaviour
{
    public StartMove targetObject; // StartMove スクリプトを持つオブジェクトの参照

    // ボタンが押された際に呼び出すメソッド
    public void OnButtonClicked()
    {
        Debug.Log("Button Clicked");

        // BattleScene に遷移
        SceneManager.LoadScene("BattleScene");
    }
}
