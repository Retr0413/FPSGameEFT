using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameObject target;
    public float speed;
    public float distance;
    private float Maxtime = 3.0f;
    private float counttime;

    void Start()
    {
        speed = 0.05f;
        target = GameObject.Find("RPGHero");
    }

    void Update()
    {
        // ‹——£‚ª‹ß‚­‚È‚é‚Æ’Ç‚Á‚Ä‚­‚é
        distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < 50)
        {
            transform.LookAt(target.transform);
            transform.position += transform.forward * speed;
        }
        if (distance > 50)
        {
            counttime = Time.deltaTime;
            transform.position += transform.forward * Time.deltaTime;
            if (counttime >= Maxtime)
            {
                Vector3 course = new Vector3(0, Random.Range(0, 270), 0);
                transform.localRotation = Quaternion.Euler(course);

                counttime = 0;
            }
        }
        // transform.LookAt(target.transform);
        // transform.position += transform.forward * speed;
    }
}