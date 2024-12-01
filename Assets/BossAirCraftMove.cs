using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossAirCraftMove : MonoBehaviour
{
    public GameObject player; // プレイヤーオブジェクト
    public float interval = 2f; // ランダム移動の間隔
    public float maxMoveDistance = 20f; // 最大移動距離
    public float circleSpeed = 100f; // 旋回速度
    public float speed = 5f; // 移動速度
    public int maxHP = 10000; // 最大HP
    private int currentHP; // 現在のHP
    public Slider hpSlider; // HPバー
    private NavMeshAgent agent; // NavMeshAgent
    private Vector3 walkDirection; // ランダム移動の方向
    private float elapsedTime = 0f; // 経過時間
    private float angle = 0f; // 旋回角度
    public float circleRadius = 20f; // 旋回半径
    private Vector3 circlingCenter; // 旋回の中心

    void Start()
    {
        player = GameObject.Find("PlayerPMC");
        currentHP = maxHP;
        hpSlider.value = 1f; // 初期値
        agent = GetComponent<NavMeshAgent>(); // NavMeshAgentを取得
        ResetWalkParameters(); // 初期移動パラメータを設定
        circlingCenter = transform.position; // 現在位置を旋回の中心に設定
    }

    void Update()
    {
        if (currentHP <= 0)
        {
            Die();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > 1000000f)
        {
            PerformCircling(); // 旋回処理
        }
        else if (distance <= 10000f)
        {
            MoveRandomly(); // ランダム移動
        }

        UpdateHPBar(); // HPバーの更新
    }

    private void PerformCircling()
    {
        // 角度を増加
        angle += circleSpeed * Time.deltaTime; 
        float radian = angle * Mathf.Deg2Rad; // 角度をラジアンに変換

        // 旋回するための座標を計算
        float z = Mathf.Cos(radian) * circleRadius; // z座標
        float x = Mathf.Sin(radian) * circleRadius; // x座標

        // 新しい位置を設定 (z軸を中心に旋回)
        Vector3 newPosition = new Vector3(circlingCenter.x + x, transform.position.y, circlingCenter.z + z);

        // 現在位置との差分から進行方向を計算
        Vector3 forwardDirection = newPosition - transform.position;

        // -x軸を進行方向に向ける
        if (forwardDirection != Vector3.zero) // 進行方向がゼロでないことを確認
        {
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + 90f, 0); // -xを向けるために調整
        }

        // 新しい位置をオブジェクトに適用
        transform.position = newPosition;
    }

    private void MoveRandomly()
    {
        elapsedTime += Time.deltaTime;

        // ランダム移動の間隔を超えたら新しい目標地点を設定
        if (elapsedTime >= interval)
        {
            // 現在位置からランダムな方向に移動先を決定
            Vector3 sourcePos = transform.position;
            Vector3 targetPos = sourcePos + walkDirection * maxMoveDistance;

            // y座標を固定
            targetPos.y = sourcePos.y;

            // 移動先がNavMesh内かどうかを判定
            NavMeshHit hit;
            bool blocked = NavMesh.Raycast(sourcePos, targetPos, out hit, NavMesh.AllAreas);

            if (blocked)
            {
                // 移動先がブロックされている場合、移動可能な場所に設定
                agent.SetDestination(hit.position);
            }
            else
            {
                // 移動先が有効であればそのまま設定
                agent.SetDestination(targetPos);
            }

            // デバッグ用にラインを表示
            Debug.DrawLine(sourcePos, targetPos, blocked ? Color.red : Color.green, interval);

            // ランダムな方向を再生成
            ResetWalkParameters();
        }

        // プレイヤーの方向に常に向く
        FacePlayer();
    }

    private void FacePlayer()
    {
        // プレイヤーの方向を計算
        Vector3 lookDirection = player.transform.position - transform.position;
        lookDirection.y = 0; // 水平面での方向に限定

        // -x軸をプレイヤー方向に向ける
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + 90f, 0); // -xを向けるために調整
        }
    }

    private void ResetWalkParameters()
    {
        elapsedTime = 0f;

        // ランダムな方向を生成
        float x = (Random.value * 2f) - 1f;
        float z = (Random.value * 2f) - 1f;
        walkDirection = new Vector3(x, 0f, z).normalized;
    }

    private void UpdateHPBar()
    {
        hpSlider.value = (float)currentHP / maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject, 2f); // 2秒後に削除
    }
}
