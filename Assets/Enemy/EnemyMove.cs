using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    public Animator enemyMove;
    public GameObject player;
    public float distance;
    public float speed;
    [SerializeField] private RenderBuffer Enemy;
    private float Maxtime = 3.0f;
    private float counttime;

    void Start()
    {
        player = GameObject.Find("Player");
        enemyMove = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        // 距離による敵の行動パターン
        if (distance > 300)
        {
            enemyMove.SetBool("Walk", true);
            counttime += Time.deltaTime;
            transform.position += transform.forward * Time.deltaTime * speed;

            if (counttime >= Maxtime)
            {
                enemyMove.SetBool("Walk", false);
                Vector3 course = new Vector3(0, Random.Range(0, 300), 0);
                transform.localRotation = Quaternion.Euler(course);
                counttime = 0;
            }
        }

        else if (distance < 300)
        {
            enemyMove.SetBool("Walk", false);
            Vector3 direction = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
        }
    }
}
