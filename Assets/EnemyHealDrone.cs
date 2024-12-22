using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealDrone : MonoBehaviour
{
    float elapsedTime;
    Vector3 walkDirection;
    NavMeshAgent agent;
    public float playerdistance;
    public GameObject player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Rougue Variant LMG");
    }

    void Update()
    {
        playerdistance = Vector3.Distance(transform.position, player.transform.position);

        if (playerdistance < 1000000)
        {
            agent.destination = player.transform.position;
        }
    }
}
