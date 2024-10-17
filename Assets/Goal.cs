using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private bool isActive = false;

    public void ActivateGoal()
    {
        isActive = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // "RPGHeroHP"という名前のGameObjectを探す
        GameObject player = GameObject.Find("RPGHeroHP");

        // プレイヤーが見つかり、そのオブジェクトがトリガーに入ったかを確認
        if (isActive && player != null && other.gameObject == player)
        {
            Debug.Log("Clear");
            UnityEditor.EditorApplication.isPlaying = false;
            // SceneManager.LoadScene("NextSceneName");  // "NextSceneName"を次のシーン名に置き換えてください
        }
    }
}
