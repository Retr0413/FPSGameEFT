using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionbullet : MonoBehaviour
{
    public GameObject explosionEffect; 
    public float lifeTime = 10f;     

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
