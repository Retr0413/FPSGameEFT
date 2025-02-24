using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombMove : MonoBehaviour
{
    NavMeshAgent agent;
    public float enemydistance;

    private GameObject[] enemyMobs;
    private GameObject[] enemyBosses;
    [SerializeField] GameObject effect;

    private GameObject targetEnemy; // 追尾対象の敵
    private bool isTargetLocked = false; // 敵がロックされているか

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isTargetLocked)
        {
            // タグを使用して敵を検索
            enemyMobs = GameObject.FindGameObjectsWithTag("EnemyMob");
            enemyBosses = GameObject.FindGameObjectsWithTag("EnemyBoss");

            // 最も近い敵を取得し、ターゲットをロック
            targetEnemy = GetNearestEnemy();
            if (targetEnemy != null)
            {
                isTargetLocked = true;
                Debug.Log($"Target locked: {targetEnemy.name}");
            }
            else
            {
                Debug.Log("No enemies found!");
                return;
            }
        }

        if (targetEnemy != null)
        {
            // 敵への距離を計算し、移動先を設定
            enemydistance = Vector3.Distance(transform.position, targetEnemy.transform.position);
            agent.destination = targetEnemy.transform.position;

            // 敵に近づいたら爆発
            if (enemydistance < 1f)
            {
                Explode();
            }
        }
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

    void Explode()
    {
        // 爆発エフェクトを生成し、オブジェクトを破壊
        GameObject Bombeffect = Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(Bombeffect, 0.2f);
        Destroy(this.gameObject, 0.2f);
    }
}
