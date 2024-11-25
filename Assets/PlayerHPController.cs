using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPController : MonoBehaviour
{
    public Slider hpSlider;           // HPゲージ用のスライダー
    public Slider armorSlider;        // アーマーゲージ用のスライダー
    public Slider chargeSlider;       // アーマー充電用のスライダー
    public int maxHP = 100;           // 最大HP
    public int maxArmor = 100;        // 最大アーマー
    public int recoveryAmount = 5;    // 回復量
    public float chargeDuration = 5f; // 充電が100%になるまでの時間
    public float healRate = 10f;      // HealDroneによる回復速度（秒あたり）

    private int currentHP;            // 現在のHP
    private int currentArmor;         // 現在のアーマー
    private float timeSinceLastDamage = 0f; // 最後のダメージからの経過時間
    private bool isRecovering = false;      // 回復状態のチェック
    private float chargeAmount = 0f;        // 充電の進行度

    private GameObject healDrone;     // HealDroneゲームオブジェクト

    void Start()
    {
        // HealDroneをシーンから取得
        healDrone = GameObject.Find("HealDrone");

        // 初期値の設定
        currentHP = maxHP;
        currentArmor = maxArmor;

        // スライダーの初期値を設定
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;

        armorSlider.maxValue = maxArmor;
        armorSlider.value = currentArmor;

        // 充電スライダーの初期値を設定
        chargeSlider.maxValue = 1f;  // 0から1の範囲で管理
        chargeSlider.value = 0f;
    }

    void Update()
    {
        // デバッグ用：スペースキーでダメージを受ける
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10); // 10ダメージ
            timeSinceLastDamage = 0f; // ダメージを受けたのでタイマーをリセット
        }

        // ダメージを受けていない時間を計測
        timeSinceLastDamage += Time.deltaTime;

        // 条件：アーマーがゼロかつ、5秒以上ダメージを受けていない場合にHPを回復
        if (armorSlider.value == 0 && timeSinceLastDamage >= 5f && currentHP < maxHP)
        {
            RecoverHP(recoveryAmount);
            isRecovering = true; // 回復中とする
        }
        else
        {
            isRecovering = false; // 回復中でない
        }

        // HealDroneがアクティブな場合、アーマーとHPを徐々に回復
        if (healDrone != null)
        {
            Debug.Log("HealDroneの状態: " + healDrone.activeInHierarchy);
        }

        if (healDrone != null && healDrone.activeInHierarchy)
        {
            RecoverArmor(Time.deltaTime * healRate); // アーマーの回復
            RecoverHP((int)(Time.deltaTime * healRate)); // HPの回復
        }

        // Zキーが押され、アーマーが100%未満の時に充電を増加
        if (Input.GetKey(KeyCode.Z) && currentArmor < maxArmor)
        {
            chargeAmount += Time.deltaTime / chargeDuration; // 充電の進行度を設定
            chargeSlider.value = Mathf.Clamp01(chargeAmount); // 0から1までの範囲で制限

            // 充電スライダーが100%に達した場合、アーマーを100%に
            if (chargeSlider.value >= 1f)
            {
                currentArmor = maxArmor;
                armorSlider.value = currentArmor;
                chargeAmount = 0f; // 充電をリセット
                chargeSlider.value = 0f;
            }
        }
        else
        {
            // Zキーが離されたら充電をリセット
            chargeAmount = 0f;
            chargeSlider.value = 0f;
        }
    }

    // ダメージを受ける関数
    public void TakeDamage(int damage)
    {
        if (currentArmor > 0)
        {
            // アーマーがある場合はアーマーを減らす
            currentArmor -= damage;
            if (currentArmor < 0)
            {
                currentArmor = 0; // アーマーは0以下にならない
            }
            armorSlider.value = currentArmor;
        }
        else
        {
            // アーマーがない場合はHPを減らす
            currentHP -= damage;
            if (currentHP < 0)
            {
                currentHP = 0; // HPも0以下にならない
                // HPが0になった際の処理（ゲームオーバーなど）
                Debug.Log("Player is dead!");
            }
            hpSlider.value = currentHP;
        }
        timeSinceLastDamage = 0f; // ダメージを受けたのでタイマーをリセット
    }

    // HPを回復する関数
    private void RecoverHP(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP; // HPは最大値を超えない
        }
        hpSlider.value = currentHP;
    }

    // アーマーを回復する関数
    private void RecoverArmor(float amount)
    {
        currentArmor += (int)amount;
        if (currentArmor > maxArmor)
        {
            currentArmor = maxArmor; // アーマーは最大値を超えない
        }
        armorSlider.value = currentArmor;
    }
}
