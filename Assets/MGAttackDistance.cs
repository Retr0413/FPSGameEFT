using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGAttackDistance : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("PlayerPMC");
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 myPosition = transform.position;

            float distance = Vector3.Distance(myPosition, playerPosition);

            if (distance <= 1000f)
            {
                Vector3 directionToPlayer = (playerPosition - myPosition).normalized;

                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f); // 回転速度を調整可能
            }
        }
    }
}
