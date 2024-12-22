using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemHP : MonoBehaviour
{
    public int maxHP = 10; // 最大HP
    private int currentHP; // 現在のHP

    void Start()
    {
        currentHP = maxHP; // 初期HPを設定
    }

    void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject); // HPが0以下になったらオブジェクトを削除
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // タグが"Bullet"の場合
        {
            TakeDamage(1); // 1ダメージを受ける
        }
    }

    private void TakeDamage(int damage)
    {
        currentHP -= damage; // ダメージをHPから引く
        Debug.Log("Enemy HP: " + currentHP); // 現在のHPをログに表示
    }
}
