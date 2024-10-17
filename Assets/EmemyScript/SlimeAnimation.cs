using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlimeAnimation : MonoBehaviour
{
    private Animator anim;
    public GameObject target;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        target = GameObject.Find("RPGHeroHP");
        anim.SetBool("Walk",true);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
        Debug.Log(distance);
        if (distance < 1.7f) 
        {
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }
}
