using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMove : MonoBehaviour
{
    public float speed = 10f; // ミサイルの速度
    private Transform playerTransform; // プレイヤーのTransform

    void Start()
    {
        GameObject player = GameObject.Find("PlayerPMC");
        if (player != null)
        {
            playerTransform = player.transform;

            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
