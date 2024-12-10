using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAirCraftMove : MonoBehaviour
{
    public GameObject player;
    public float interval = 2f;
    public float maxMoveDistance = 20f;
    public float circleSpeed = 100f;
    public float speed = 5f;
    public int maxHP = 10000;
    private int currentHP;
    public Slider hpSlider;
    private Vector3 walkDirection;
    private float elapsedTime = 0f;
    private float angle = 0f;
    public float circleRadius = 20f;
    private Vector3 circlingCenter;
    public GameObject[] wings;
    private float initialYPosition;

    public GameObject enemyPrefab;
    public GameObject enemyPrefab1;

    private bool enemiesSpawned = false;

    public float heightChangeSpeed = 2f;
    private float targetHeight;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.Find("PlayerPMC");
        currentHP = maxHP;
        hpSlider.value = 1f;
        ResetWalkParameters();
        circlingCenter = transform.position;
        initialYPosition = transform.position.y;
        targetHeight = initialYPosition;
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(250);
        }

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        SmoothUpdateHeight();

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < 1f)
        {
            PerformCircling();
        }
        else if (distance > 1f)
        {
            MoveRandomly();
        }

        UpdateHPBar();
    }

    private void SmoothUpdateHeight()
    {
        float currentHeight = transform.position.y;
        float newHeight = Mathf.MoveTowards(currentHeight, targetHeight, heightChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    private void PerformCircling()
    {
        float hoverHeight = Mathf.Sin(Time.time * 0.5f) * 2f;
        angle += circleSpeed * Time.deltaTime;
        float radian = angle * Mathf.Deg2Rad;

        float z = Mathf.Cos(radian) * circleRadius;
        float x = Mathf.Sin(radian) * circleRadius;

        Vector3 newPosition = new Vector3(circlingCenter.x + x, transform.position.y, circlingCenter.z + z);
        Vector3 forwardDirection = newPosition - transform.position;

        transform.position = newPosition;

        if (forwardDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + 90f, 0);
        }
    }

    private void MoveRandomly()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= interval)
        {
            ResetWalkParameters();
            elapsedTime = 0f;
        }

        Vector3 movement = walkDirection * speed * Time.deltaTime;
        transform.position += movement;

        float currentHeight = transform.position.y;
        float newHeight = Mathf.MoveTowards(currentHeight, targetHeight, heightChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);

        FacePlayer();
    }

    private void FacePlayer()
    {
        Vector3 lookDirection = player.transform.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + 90f, 0);
        }
    }

    private void ResetWalkParameters()
    {
        elapsedTime = 0f;

        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        walkDirection = new Vector3(x, 0f, z).normalized;
    }

    private void UpdateHPBar()
    {
        hpSlider.value = (float)currentHP / maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        float percentage = Mathf.Clamp01((float)currentHP / maxHP);
        targetHeight = Mathf.Lerp(0, initialYPosition, percentage);

        if (percentage <= 0.75f)
        {
            SetWingActive(0, false);
        }
        if (percentage <= 0.5f)
        {
            SetWingActive(1, false);
        }
        if (percentage <= 0.25f)
        {
            SetWingActive(2, false);
        }
        if (percentage <= 0f)
        {
            SetWingActive(3, false);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void SetWingActive(int index, bool isActive)
    {
        if (index >= 0 && index < wings.Length)
        {
            wings[index].SetActive(isActive);
        }
    }

    private void Die()
    {
        isDead = true;
        targetHeight = 0;

        SmoothUpdateHeight();

        if (!enemiesSpawned)
        {
            SpawnEnemies();
            enemiesSpawned = true;
        }
    }

    private void SpawnEnemies()
    {
        Vector3 spawnPosition1 = transform.position + new Vector3(3f, 0f, 0f);
        Vector3 spawnPosition2 = transform.position + new Vector3(-3f, 0f, 0f);

        Instantiate(enemyPrefab, spawnPosition1, Quaternion.identity);
        Instantiate(enemyPrefab1, spawnPosition2, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(250);
        }
    }
}
