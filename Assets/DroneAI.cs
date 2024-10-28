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
    public GameObject enemy;
    public GameObject player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // ResetWalkParameters();
        enemy = GameObject.Find("Rougue Variant");
        player = GameObject.Find("PlayerPMC");
    }

    void Update()
    {
        tgdistance = Vector3.Distance(transform.position, enemy.transform.position);
        playerdistance = Vector3.Distance(transform.position, player.transform.position);
        // if (tgdistance > 100 || tgdistance < 15)
        // {
        //     UpdateAgentMovement();
        // }
        if (tgdistance < 50 && tgdistance > 15)
        {
            agent.destination = enemy.transform.position;
        }
        else if (tgdistance < 50)
        {
            Lookenemy();
        }
        else if (playerdistance < 30 && playerdistance > 5 && tgdistance > 50)
        {
            agent.destination = player.transform.position;
        }
    }

    void Lookenemy()
    {
        Vector3 direction = enemy.transform.position - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // void ResetWalkParameters()
    // {
    //     elapsedTime = 0f;

    //     var x = (Random.value * 2f) - 1f;
    //     var z = (Random.value * 2f) - 1f;

    //     walkDirection = new Vector3(x, 0f, z).normalized;
    // }


    // void UpdateAgentMovement()
    // {
    //     elapsedTime += Time.deltaTime;

    //     if (elapsedTime >= interval)
    //     {
    //         MoveTowardsTarget();
    //         ResetWalkParameters();
    //     }
    // }


    // void MoveTowardsTarget()
    // {
    //     var sourcePos = transform.position;
    //     //sourcePos.y -= 1f;
    //     var targetPos = sourcePos + walkDirection * maxMoveDistance;
    //     var blocked = NavMesh.Raycast(sourcePos, targetPos, out NavMeshHit hitInfo, NavMesh.AllAreas);

    //     if (blocked)
    //     {
    //         agent.SetDestination(hitInfo.position);
    //     }
    //     else
    //     {
    //         agent.SetDestination(targetPos);
    //     }
    //     Debug.DrawLine(sourcePos, targetPos, blocked ? Color.red : Color.green, interval);
    // }
}
