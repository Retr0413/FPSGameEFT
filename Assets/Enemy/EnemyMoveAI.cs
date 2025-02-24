using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveAI : MonoBehaviour
{
    [SerializeField] float interval;
    [SerializeField] float maxMoveDistance;
    [SerializeField] float animationspeed = 0.1f;
    float elapsedTime;
    Vector3 walkDirection;
    NavMeshAgent agent;
    public float tgdistance;
    public GameObject player;
    private Animator EnemyMove;
    public static EnemyMoveAI instance;
    public float speed = 1f;
    public float heightChangeSpeed = 2f;
    private float targetHeight;

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

        if (tgdistance > 100)
        {
            UpdateAgentMovement();
            LookPlayer();
            UpdateAnimation();
        }
        else if (tgdistance < 100 && tgdistance > 30)
        {
            agent.destination = player.transform.position;
            EnemyMove.SetBool("Walk", true);
        }
        else if (tgdistance < 30)
        {
            MoveRandomly();
            LookPlayer();
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

    // void LookPlayerZAxis()
    // {
    //     Vector3 direction = player.transform.position - transform.position;
    //     direction.y = 0; // y軸の回転を無視
    //     direction.x = 0; // x軸の回転を無視してz軸方向に向ける
    //     if (direction.magnitude > 0.1f) // プレイヤーとの位置差がある場合のみ回転
    //     {
    //         Quaternion lookRotation = Quaternion.LookRotation(direction);
    //         transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    //     }
    // }

    private void MoveRandomly()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= interval)
        {
            ResetWalk();
            elapsedTime = 0f;
        }

        Vector3 movement = walkDirection * speed * Time.deltaTime;
        transform.position += movement;

        float currentHeight = transform.position.y;
        float newHeight = Mathf.MoveTowards(currentHeight, targetHeight, heightChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    private void ResetWalk()
    {
        elapsedTime = 5f;

        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        walkDirection = new Vector3(x, 0f, z).normalized;
    }

    void ResetWalkParameters()
    {
        elapsedTime = 0f;
        var x = (Random.value * 2f) - 1f;
        var z = (Random.value * 2f) - 1f;
        walkDirection = new Vector3(x, 0f, z).normalized;
    }

    void UpdateAgentMovement()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= interval)
        {
            MoveTowardsTarget();
            ResetWalkParameters();
        }
    }

    void MoveTowardsTarget()
    {
        var sourcePos = transform.position;
        var targetPos = sourcePos + walkDirection * maxMoveDistance;
        var blocked = NavMesh.Raycast(sourcePos, targetPos, out NavMeshHit hitInfo, NavMesh.AllAreas);

        if (blocked)
        {
            agent.SetDestination(hitInfo.position);
        }
        else
        {
            agent.SetDestination(targetPos);
        }

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

    public void TriggerReloadAnimation()
    {
        if (EnemyMove != null)
        {
            EnemyMove.SetBool("Reload", true);
        }
        else
        {
            EnemyMove.SetBool("Reload", false);
        }
    }
}
