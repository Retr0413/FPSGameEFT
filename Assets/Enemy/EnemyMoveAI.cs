using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveAI : MonoBehaviour
{
    // �ړI�n��ݒ肷��Ԋu
    [SerializeField] float interval;
    // ���C�̒���
    [SerializeField] float maxMoveDistance;
    [SerializeField] float animationspeed = 0.1f;
    // �o�ߎ���
    float elapsedTime;
    // ��������
    Vector3 walkDirection;
    NavMeshAgent agent;
    public float tgdistance;
    public GameObject player;
    private Animator EnemyMove;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ResetWalkParameters();
        player = GameObject.Find("PlayerPMC");
        EnemyMove = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        tgdistance = Vector3.Distance(transform.position, player.transform.position);
        if (tgdistance > 100 || tgdistance < 15)
        {
            UpdateAgentMovement();
            UpdateAnimation();
        }
        else if (tgdistance < 100 || tgdistance > 15)
        {
            agent.destination = player.transform.position;
            EnemyMove.SetBool("Walk", true);
        }
        else if (tgdistance < 50)
        {
            LookPlayer();
        }
    }

    void LookPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void ResetWalkParameters()
    {
        // �p�����[�^��������
        elapsedTime = 0f;

        // �����_���̕������쐬
        var x = (Random.value * 2f) - 1f;
        var z = (Random.value * 2f) - 1f;

        walkDirection = new Vector3(x, 0f, z).normalized;
    }


    void UpdateAgentMovement()
    {
        elapsedTime += Time.deltaTime;

        // �����Ԃ��ƂɖړI�n��ݒ肵�Ēl��������
        if (elapsedTime >= interval)
        {
            MoveTowardsTarget();
            ResetWalkParameters();
        }
    }


    void MoveTowardsTarget()
    {
        // ���C�̎n�_
        var sourcePos = transform.position;
        //sourcePos.y -= 1f;
        // ���C�̏I�_
        var targetPos = sourcePos + walkDirection * maxMoveDistance;
        // ���C�𓊂���
        var blocked = NavMesh.Raycast(sourcePos, targetPos, out NavMeshHit hitInfo, NavMesh.AllAreas);

        if (blocked)
        {
            // �q�b�g�n�_��ړI�n�ɂ���
            agent.SetDestination(hitInfo.position);
        }
        else
        {
            // �^�[�Q�b�g�ʒu��ړI�n�ɂ���B
            agent.SetDestination(targetPos);
        }
        // ���C����`��
        Debug.DrawLine(sourcePos, targetPos, blocked ? Color.red : Color.green, interval);
    }

    void UpdateAnimation()
    {
        if (agent.velocity.magnitude > animationspeed)
        {
            EnemyMove.SetBool("Walk", true);
        }
        else
        {
            EnemyMove.SetBool("Walk", false);
        }
    }
}
