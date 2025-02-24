using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerを使用するために追加
using UnityEngine.UI;

public class PlayerHPController : MonoBehaviour
{
    public Slider hpSlider;
    public Slider armorSlider;
    public Slider chargeSlider;
    public int maxHP = 100;
    public int maxArmor = 100;
    public int recoveryAmount = 5;

    private int currentHP;
    private int currentArmor;
    private float chargeAmount = 0f;
    private Animator anim;

    private GameObject healDrone; // HealDroneの参照
    private bool isHealDroneActive = false; // HealDroneが有効かどうかを判定
    private Coroutine healingCoroutine = null; // コルーチンの参照

    private bool isRetrying = false; // リトライシーン遷移中かどうかを判定

    void Start()
    {
        currentHP = maxHP;
        currentArmor = maxArmor;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        armorSlider.maxValue = maxArmor;
        armorSlider.value = currentArmor;
        chargeSlider.maxValue = 1f;
        chargeSlider.value = 0f;
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // HealDroneを動的に検索
        if (!isHealDroneActive)
        {
            healDrone = GameObject.FindWithTag("HealDrone"); // HealDroneに"HealDrone"タグを付ける
            if (healDrone != null && healDrone.activeInHierarchy)
            {
                isHealDroneActive = true;

                // HealDroneが有効になった場合、回復のコルーチンを開始
                if (healingCoroutine == null)
                {
                    healingCoroutine = StartCoroutine(HealArmorOverTime());
                }
            }
        }
        else if (healDrone != null && !healDrone.activeInHierarchy)
        {
            isHealDroneActive = false;
            healDrone = null;

            // HealDroneが無効になった場合、コルーチンを停止
            if (healingCoroutine != null)
            {
                StopCoroutine(healingCoroutine);
                healingCoroutine = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }

        // チャージ処理
        if (Input.GetKey(KeyCode.Z) && currentArmor < maxArmor)
        {
            chargeAmount += Time.deltaTime / 5f; // チャージ時間を5秒に設定
            chargeSlider.value = Mathf.Clamp01(chargeAmount);

            if (chargeSlider.value >= 1f)
            {
                currentArmor = maxArmor;
                armorSlider.value = currentArmor;
                chargeAmount = 0f;
                chargeSlider.value = 0f;
            }
        }
        else
        {
            chargeAmount = 0f;
            chargeSlider.value = 0f;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentArmor > 0)
        {
            currentArmor -= damage;
            if (currentArmor < 0)
            {
                currentArmor = 0;
            }
            armorSlider.value = currentArmor;
        }
        else
        {
            currentHP -= damage;
            if (currentHP < 0)
            {
                currentHP = 0;
                if (!isRetrying)
                {
                    StartCoroutine(HandlePlayerDeath());
                }
            }
            hpSlider.value = currentHP;
        }
    }

    private IEnumerator HandlePlayerDeath()
    {
        isRetrying = true;
        anim.SetBool("Die", true);
        Debug.Log("Player is dead! Retrying in 2 seconds...");
        yield return new WaitForSeconds(5f);

        // シーンをRetrySceneに遷移
        SceneManager.LoadScene("RetryScene");
    }

    private void RecoverArmor(int amount)
    {
        currentArmor += amount;
        if (currentArmor > maxArmor)
        {
            currentArmor = maxArmor;
        }
        armorSlider.value = currentArmor;
    }

    private IEnumerator HealArmorOverTime()
    {
        while (isHealDroneActive)
        {
            // 2秒ごとにArmorを5回復
            if (currentArmor < maxArmor)
            {
                RecoverArmor(5);
            }

            yield return new WaitForSeconds(2f); // 2秒間隔
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet") || other.CompareTag("EnemyShotgunBullet"))
        {
            TakeDamage(5);
            Debug.Log("EnemyBulletに衝突しました！5ダメージを受けました。");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Explosion"))
        {
            TakeDamage(10);
            Debug.Log("Particleに衝突しました！10ダメージを受けました。");
        }
    }
}
