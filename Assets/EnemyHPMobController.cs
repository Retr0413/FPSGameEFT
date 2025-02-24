using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPMobController : MonoBehaviour
{
    public Slider hpSlider;
    public int maxHP = 100;
    public int recoveryAmount = 5;

    private int currentHP;
    private float timeSinceLastDamage = 0f;
    private bool isRecovering = false;

    private Animator animator;

    void Start()
    {
        currentHP = maxHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            timeSinceLastDamage = 0f;
        }

        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage >= 5f && currentHP < maxHP)
        {
            RecoverHP(recoveryAmount);
            isRecovering = true;
        }
        else
        {
            isRecovering = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
        hpSlider.value = currentHP;
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
        Destroy(gameObject, 3f);
    }
}
