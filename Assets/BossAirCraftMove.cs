using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAirCraftMove : MonoBehaviour
{
    public Rigidbody rb;
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

    public GameObject enemyType1;
    public GameObject enemyType2;

    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    public int totalEnemies = 10;

    public Transform parentObject;

    private bool enemiesSpawned = false; // 敵が既に生成されたかどうかを判定するフラグ
    private bool isDead = false;
    public GameObject explosionPrefab;

    void Start()
    {
        player = GameObject.Find("PlayerPMC");
        currentHP = maxHP;
        hpSlider.value = 1f;
        ResetWalkParameters();
        circlingCenter = transform.position;
        initialYPosition = transform.position.y;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
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

        if (distance > 200f)
        {
            PerformCircling();
        }
        else if (distance < 200f)
        {
            MoveRandomly();
        }

        UpdateHPBar();
    }

    private void SmoothUpdateHeight()
    {
        float currentHeight = transform.position.y;
        float newHeight = Mathf.MoveTowards(currentHeight, initialYPosition, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    private void PerformCircling()
    {
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

        if (percentage <= 0.75f)
        {
            SetWingActive(0, false);
            InstantiateExplosion(wings[0].transform.position);
        }
        if (percentage <= 0.5f)
        {
            SetWingActive(1, false);
            InstantiateExplosion(wings[1].transform.position);
        }
        if (percentage <= 0.25f)
        {
            SetWingActive(2, false);
            InstantiateExplosion(wings[2].transform.position);
        }
        if (percentage <= 0f)
        {
            SetWingActive(3, false);
            InstantiateExplosion(wings[3].transform.position);
        }

        if (currentHP <= 0)
        {
            Die();
            rb.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(250);
        }
    }

    private void InstantiateExplosion(Vector3 position)
    {
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        Destroy(explosion, 2f);
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
        if (enemiesSpawned) return; // 既に敵が生成されている場合、処理を終了

        GenerateEnemies(); // 一度だけ敵を生成
        isDead = true;
        enemiesSpawned = true; // 敵を生成したことを記録

        Destroy(this.gameObject, 2f);
    }

    void GenerateEnemies()
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            GameObject selectedEnemy = Random.Range(0, 2) == 0 ? enemyType1 : enemyType2;

            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            GameObject enemy = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);

            if (parentObject != null)
            {
                enemy.transform.SetParent(parentObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((spawnAreaMin + spawnAreaMax) / 2, spawnAreaMax - spawnAreaMin);
    }
}
