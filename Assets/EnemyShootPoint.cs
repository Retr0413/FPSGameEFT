using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootPoint : MonoBehaviour
{
    public GameObject MortarBullet; // 弾丸のPrefab
    public Transform shootpoint;    // 発射位置
    public Transform target;        // ターゲットの位置

    public float shootForce = 10f; // デフォルトの射出力
    public float launchSpeed = 20f; // 発射速度
    public float shootingDistance = 300f; // 発射開始距離
    public float shootingInterval = 10f;  // 発射間隔

    private float shootTimer = 0f;  // 発射タイマー

    void Start()
    {
        // ターゲットオブジェクトをシーンから探す（オブジェクト名に注意）
        GameObject targetObject = GameObject.Find("PlayerPMC");
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            Debug.LogError("Target object 'PlayerPMC' not found in the scene.");
        }
    }

    void Update()
    {
        if (target != null)
        {
            // ターゲットとの距離を計算
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // 距離が100を切った場合
            if (distanceToTarget <= shootingDistance)
            {
                shootTimer += Time.deltaTime;

                // 一定時間ごとに発射
                if (shootTimer >= shootingInterval)
                {
                    Shoot();
                    shootTimer = 0f;
                }
            }
        }
    }

    void Shoot()
    {
        if (target == null)
        {
            Debug.LogError("Target is not set.");
            return;
        }

        // 弾丸を生成
        GameObject MortarFire = Instantiate(MortarBullet, shootpoint.position, Quaternion.identity);

        // 弾丸の方向を計算
        Vector3 MortarDirection = target.position - MortarFire.transform.position;

        // 水平距離と高さを取得
        float horizontalDistance = new Vector3(MortarDirection.x, 0, MortarDirection.z).magnitude;
        float verticalDistance = MortarDirection.y;

        // 重力の絶対値を取得
        float gravity = Mathf.Abs(Physics.gravity.y);

        // 判別式を計算
        float discriminant = launchSpeed * launchSpeed * launchSpeed * launchSpeed - gravity * (gravity * horizontalDistance * horizontalDistance + 2 * verticalDistance * launchSpeed * launchSpeed);

        if (discriminant < 0)
        {
            Debug.LogError("Cannot compute angle: discriminant is negative. Target might be out of range.");
            return;
        }

        // 発射角度を計算
        float angle = Mathf.Atan((launchSpeed * launchSpeed - Mathf.Sqrt(discriminant)) / (gravity * horizontalDistance));

        // 初速度を計算
        Vector3 velocity = new Vector3(MortarDirection.x, 0, MortarDirection.z).normalized * Mathf.Cos(angle) * launchSpeed;
        velocity.y = Mathf.Sin(angle) * launchSpeed;

        // Rigidbodyを取得して速度を設定
        Rigidbody rb = MortarFire.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = velocity;
        }
        else
        {
            Debug.LogError("No Rigidbody found on the MortarBullet prefab.");
        }
    }
}
