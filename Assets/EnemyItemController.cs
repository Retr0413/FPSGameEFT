using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemController : MonoBehaviour
{
    public GameObject player; 
    public float detectRange = 10f; 
    public GameObject itemPrefab; 
    public GameObject itemPrefab1;
    public float minWaitTime = 2f; 
    public float maxWaitTime = 5f; 
    private bool isEngagedInCombat = false; 

    void Start()
    {
        player = GameObject.Find("PlayerPMC"); 
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectRange && !isEngagedInCombat)
        {
            StartCoroutine(EnterCombatAndDropItem());
        }
    }

    IEnumerator EnterCombatAndDropItem()
    {
        isEngagedInCombat = true; 
        Debug.Log("Entering combat state...");

        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        Debug.Log($"Waiting for {waitTime} seconds...");
        yield return new WaitForSeconds(waitTime);

        SpawnItem();
        isEngagedInCombat = false; 
    }

    void SpawnItem()
    {
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);
            Debug.Log("Item spawned!");

            Instantiate(itemPrefab1, transform.position + Vector3.up, Quaternion.identity);
            Debug.Log("Item spawned!");
        }
        else
        {
            Debug.LogError("Item prefab is not assigned!");
        }
    }
}
