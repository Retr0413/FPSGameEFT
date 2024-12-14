using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMuzzle : MonoBehaviour
{
     public GameObject missilePrefab;
    public float interval = 10f;
    public float battleRange = 50f;
    private GameObject player;
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerPMC");
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= interval)
        {
            FireMissile();
            elapsedTime = 0f;
        }

        CheckPlayerInRangeAndFace();
    }

    private void FireMissile()
    {
        GameObject missile = Instantiate(missilePrefab, transform.position, transform.rotation);
        Rigidbody rb = missile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * 500f, ForceMode.Impulse);
        }
    }
    

    private void CheckPlayerInRangeAndFace()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= battleRange)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0; 

            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            }
        }
    }
}
