using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        // if (other.CompareTag("Enemy"))
        // {
        //     Enemy enemy = other.GetComponent<Enemy>();
        //     if (enemy != null)
        //     {
        //         enemy.TakeDamage(damage);
        //     }
        // }
    }
}
