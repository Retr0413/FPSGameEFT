using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobMove : MonoBehaviour
{
    // 目的地を設定する間隔
    [SerializeField] float interval;
    // レイの長さ
    [SerializeField] float maxMoveDistance;
    [SerializeField] float animationspeed = 0.1f;
    // 経過時間
    float elapsedTime;
    // 歩く方向
    Vector3 walkDirection;
    NavMeshAgent agent;
    private Animator MobMovement;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ResetWalkParameters();
        MobMovement = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        UpdateAgentMovement();
        UpdateAnimation();
    }

    void ResetWalkParameters()
    {
        // パラメータを初期化
        elapsedTime = 0f;

        // ランダムの方向を作成
        var x = (Random.value * 2f) - 1f;
        var z = (Random.value * 2f) - 1f;

        walkDirection = new Vector3(x, 0f, z).normalized;
    }


    void UpdateAgentMovement()
    {
        elapsedTime += Time.deltaTime;

        // 一定期間ごとに目的地を設定して値を初期化
        if (elapsedTime >= interval)
        {
            MoveTowardsTarget();
            ResetWalkParameters();
        }
    }


    void MoveTowardsTarget()
    {
        // レイの始点
        var sourcePos = transform.position;
        //sourcePos.y -= 1f;
        // レイの終点
        var targetPos = sourcePos + walkDirection * maxMoveDistance;
        // レイを投げる
        var blocked = NavMesh.Raycast(sourcePos, targetPos, out NavMeshHit hitInfo, NavMesh.AllAreas);

        if (blocked)
        {
            // ヒット地点を目的地にする
            agent.SetDestination(hitInfo.position);
        }
        else
        {
            // ターゲット位置を目的地にする。
            agent.SetDestination(targetPos);
        }
        // ラインを描画
        Debug.DrawLine(sourcePos, targetPos, blocked ? Color.red : Color.green, interval);
    }

    void UpdateAnimation()
    {
        if (agent.velocity.magnitude > animationspeed)
        {
            MobMovement.SetBool("Walk", true);
        }
        else
        {
            MobMovement.SetBool("Walk", false);
        }
    }
}
