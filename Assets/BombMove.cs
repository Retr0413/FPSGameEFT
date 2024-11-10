using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BombMove : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject enemy;
    [SerializeField] GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Rougue Variant");
    }

    // Update is called once per frame
    void Update()
    {
        enemydistance = Vector3.Distance(transform.position, enemy.transform.position);
        agent.destination = enemy.transform.position;

        if (enemydistance < 0.2f)
        {
            GameObject Bombeffect = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(Bombeffect, 0.2f);
        }
        if (enemydistance < 0.1f);
        {
            Destroy(gameObject);
        }
    }
}
