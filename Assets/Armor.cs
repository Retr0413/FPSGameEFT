using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    public float ArmorHP;
    public float NowArmorHP;

    // Start is called before the first frame update
    void Start()
    {
        NowArmorHP = ArmorHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Bullet")
        {
            NowArmorHP--;
        }
    }
}
