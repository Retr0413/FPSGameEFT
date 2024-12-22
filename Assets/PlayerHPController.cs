using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPController : MonoBehaviour
{
    public Slider hpSlider;
    public Slider armorSlider;
    public Slider chargeSlider;
    public int maxHP = 100;
    public int maxArmor = 100;
    public int recoveryAmount = 5;
    public float chargeDuration = 5f;
    public float healRate = 10f;

    private int currentHP;
    private int currentArmor;
    private float timeSinceLastDamage = 0f;
    private bool isRecovering = false;
    private float chargeAmount = 0f;

    private GameObject healDrone;

    void Start()
    {
        healDrone = GameObject.Find("HealDrone");
        currentHP = maxHP;
        currentArmor = maxArmor;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        armorSlider.maxValue = maxArmor;
        armorSlider.value = currentArmor;
        chargeSlider.maxValue = 1f;
        chargeSlider.value = 0f;
    }

    void Update()
    {
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

        if (healDrone != null)
        {
            Debug.Log("HealDroneの状態: " + healDrone.activeInHierarchy);
        }

        if (healDrone != null && healDrone.activeInHierarchy)
        {
            RecoverArmor(Time.deltaTime * healRate);
            RecoverHP((int)(Time.deltaTime * healRate));
        }

        if (Input.GetKey(KeyCode.Z) && currentArmor < maxArmor)
        {
            chargeAmount += Time.deltaTime / chargeDuration;
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
                Debug.Log("Player is dead!");
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

    private void RecoverArmor(float amount)
    {
        currentArmor += (int)amount;
        if (currentArmor > maxArmor)
        {
            currentArmor = maxArmor;
        }
        armorSlider.value = currentArmor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
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
