using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    public GameObject explosionEffect; 
    public float lifeTime = 10f;
    public float MissileHP = 10f;     

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
        else if (gameObject.tag == "Bullet")
        {
            MissileHP -= 1;
            if (MissileHP <= 0)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        Destroy(gameObject);
    }
}
