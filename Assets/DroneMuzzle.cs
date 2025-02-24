using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMuzzle : MonoBehaviour
{
    public GameObject bulletPrefab;   // 弾丸のPrefab
    public Transform gunPoint;       // 弾を発射する場所
    public float fireRate = 10f;    // 射撃間隔
    public float bulletSpeed = 200f; // 弾速
    public float battleRange = 50f;  // 射程
    [SerializeField] GameObject effect; // 発射エフェクト

    private GameObject[] enemies; // 検索した敵を格納
    private Transform targetEnemy; // 現在のターゲット
    private float nextFireTime = 0f; // 次の発射可能時間

    void Update()
    {
        // タグで敵を検索
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            targetEnemy = nearestEnemy.transform;

            // 敵を向く
            LookAtEnemy(targetEnemy);

            // 射程内なら弾を発射
            float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy.position);
            if (distanceToEnemy <= battleRange && Time.time >= nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void LookAtEnemy(Transform enemy)
    {
        Vector3 direction = enemy.position - transform.position;
        direction.y = 0; // 水平方向に限定
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    void FireBullet()
    {
        // 弾丸を生成
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);

        // 弾丸に速度を付与
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = gunPoint.forward * bulletSpeed;
        }

        // 発射エフェクトを生成
        if (effect != null)
        {
            GameObject muzzleEffect = Instantiate(effect, gunPoint.position, gunPoint.rotation);
            Destroy(muzzleEffect, 0.5f);
        }

        // 弾丸の寿命を設定
        Destroy(bullet, 5f);
    }

    GameObject FindNearestEnemy()
    {
        // タグで敵を検索
        GameObject[] enemyMobs = GameObject.FindGameObjectsWithTag("EnemyMob");
        GameObject[] enemyBosses = GameObject.FindGameObjectsWithTag("EnemyBoss");

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
