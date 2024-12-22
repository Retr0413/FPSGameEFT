using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBombMove : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject enemy;
    public float enemydistance;
    [SerializeField] GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GameObject.Find("PlayerPMC");
    }

    // Update is called once per frame
    void Update()
    {
        enemydistance = Vector3.Distance(transform.position, enemy.transform.position);
        agent.destination = enemy.transform.position;

        if (enemydistance < 1f)
        {
            GameObject Bombeffect = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(Bombeffect, 0.2f);
            Destroy(this.gameObject, 0.2f);
        }
    }
}
