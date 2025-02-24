using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要な名前空間
using UnityEngine.UI;

public class EnemyHPController : MonoBehaviour
{
    public Slider hpSlider;
    public Slider armorSlider;
    public Slider chargeSlider;
    public int maxHP = 100;
    public int maxArmor = 100;
    public int recoveryAmount = 5;
    public float chargeDuration = 5f;

    private int currentHP;
    private int currentArmor;
    private float timeSinceLastDamage = 0f;
    private bool isRecovering = false;
    private float chargeAmount = 0f;
    private float ArmorRepear = 3;

    private GameObject healDroneEnemy; // HealDroneEnemyの参照
    private bool isHealDroneActive = false; // HealDroneEnemyが有効かどうか
    private Coroutine healCoroutine = null; // コルーチンの参照

    private Animator animator;

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

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // HealDroneEnemyを動的に検索
        if (!isHealDroneActive)
        {
            healDroneEnemy = GameObject.FindWithTag("HealDroneEnemy"); // HealDroneEnemyに"HealDroneEnemy"タグを付ける
            if (healDroneEnemy != null && healDroneEnemy.activeInHierarchy)
            {
                isHealDroneActive = true;

                // HealDroneEnemyが有効の場合、回復のコルーチンを開始
                if (healCoroutine == null)
                {
                    healCoroutine = StartCoroutine(HealArmorOverTime());
                }
            }
        }
        else if (healDroneEnemy != null && !healDroneEnemy.activeInHierarchy)
        {
            isHealDroneActive = false;
            healDroneEnemy = null;

            // HealDroneEnemyが無効になった場合、回復のコルーチンを停止
            if (healCoroutine != null)
            {
                StopCoroutine(healCoroutine);
                healCoroutine = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            timeSinceLastDamage = 0f;
        }

        timeSinceLastDamage += Time.deltaTime;

        if (armorSlider.value == 0 && timeSinceLastDamage >= 5f && currentHP < maxHP)
        {
            RecoverHP(recoveryAmount);
            isRecovering = true;
        }
        else
        {
            isRecovering = false;
        }

        if (currentArmor < 10 && ArmorRepear > 0)
        {
            chargeAmount += Time.deltaTime / chargeDuration;
            chargeSlider.value = Mathf.Clamp01(chargeAmount);

            if (chargeSlider.value >= 1f)
            {
                currentArmor = maxArmor;
                armorSlider.value = currentArmor;
                chargeAmount = 0f;
                chargeSlider.value = 0f;
                ArmorRepear -= 1;
            }
        }
        else
        {
            chargeAmount = 0f;
            chargeSlider.value = 0f;
        }
    }

    private IEnumerator HealArmorOverTime()
    {
        while (isHealDroneActive)
        {
            // 1秒おきにArmorを2回復
            if (currentArmor < maxArmor)
            {
                RecoverArmor(2);
            }
            yield return new WaitForSeconds(2f); // 1秒間隔
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
            if (currentHP <= 0)
            {
                currentHP = 0;
                Die();
            }
            hpSlider.value = currentHP;
        }
        timeSinceLastDamage = 0f;
    }

    private void RecoverHP(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        hpSlider.value = currentHP;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            TakeDamage(10);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Explosion")
        {
            TakeDamage(20);
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetBool("Die", true);
        }

        // 2秒後にシーンを移動
        StartCoroutine(LoadGameClearSceneAfterDelay(2f));
        Destroy(gameObject, 3f);
    }

    private IEnumerator LoadGameClearSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameClear");
    }
}
