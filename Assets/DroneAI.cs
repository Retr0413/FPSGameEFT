using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    [SerializeField] float interval;
    [SerializeField] float maxMoveDistance;
    float elapsedTime;
    Vector3 walkDirection;
    NavMeshAgent agent;
    public float tgdistance;
    public float playerdistance;
    public GameObject player;

    private GameObject[] enemyMobs;
    private GameObject[] enemyBosses;
    private GameObject targetEnemy; // 特定のターゲット
    private bool isTargetLocked = false; // ターゲットがロックされているか

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ResetWalkParameters();
        player = GameObject.Find("PlayerPMC"); // プレイヤーを名前で検索
        if (player == null)
        {
            Debug.LogError("Player object not found with name 'PlayerPMC'. Please assign the correct name.");
        }
    }

    void Update()
    {
        if (!isTargetLocked)
        {
            // タグで敵を検索して最も近い敵をロック
            enemyMobs = GameObject.FindGameObjectsWithTag("EnemyMob");
            enemyBosses = GameObject.FindGameObjectsWithTag("EnemyBoss");

            targetEnemy = GetNearestEnemy();

            if (targetEnemy != null)
            {
                isTargetLocked = true; // ターゲットをロック
                Debug.Log($"Target locked: {targetEnemy.name}");
            }
        }

        if (isTargetLocked && targetEnemy != null)
        {
            // ターゲットへの距離を計算
            tgdistance = Vector3.Distance(transform.position, targetEnemy.transform.position);

            if (tgdistance <= 50f)
            {
                FlyTowardsTarget(targetEnemy); // ターゲットの方向に飛ぶ
            }
            else
            {
                // ターゲットに向かって地上を移動
                agent.destination = targetEnemy.transform.position;
                LookAtEnemy(targetEnemy);
            }
        }
        else if (player != null)
        {
            // ターゲットがいない場合、プレイヤーを追尾
            playerdistance = Vector3.Distance(transform.position, player.transform.position);
            agent.destination = player.transform.position;
        }
    }

    void FlyTowardsTarget(GameObject target)
    {
        // ターゲットの方向に飛ぶ
        Vector3 direction = (target.transform.position - transform.position).normalized;

        // ドローンの移動速度を設定
        float flySpeed = 10f; // 飛行速度
        transform.position += direction * flySpeed * Time.deltaTime;

        LookAtEnemy(target);
    }

    void LookAtEnemy(GameObject enemy)
    {
        // ターゲットの方向を見る
        Vector3 direction = enemy.transform.position - transform.position;
        direction.y = 0; // 水平方向に限定
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void ResetWalkParameters()
    {
        elapsedTime = 0f;

        var x = (Random.value * 2f) - 1f;
        var z = (Random.value * 2f) - 1f;

        walkDirection = new Vector3(x, 0f, z).normalized;
    }

    GameObject GetNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        // EnemyMob タグの敵を探索
        foreach (var enemy in enemyMobs)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // EnemyBoss タグの敵を探索
        foreach (var boss in enemyBosses)
        {
            float distance = Vector3.Distance(transform.position, boss.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = boss;
            }
        }

        return nearestEnemy;
    }
}
