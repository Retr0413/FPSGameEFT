using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMove : MonoBehaviour
{
    public Animator animator; // アニメーション用
    public float moveSpeed = 5f; // 移動速度

    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    // ボタンが押された際に呼び出すメソッド
    public void OnButtonPressed()
    {
        Debug.Log("Button Pressed");
        if (!isMoving)
        {
            isMoving = true;

            if (animator != null)
            {
                animator.SetBool("Walk", true);
            }

            StartCoroutine(ChangeSceneAfterDelay(3f));
        }
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("BattleScene");
    }
}
