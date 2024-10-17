using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("Aim", true);
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("RunForward", true);
        }
        else
        {
            anim.SetBool("RunForward", false);
        }

        if (Input.GetKey(KeyCode.R))
        {
            anim.SetBool("Reload", true);
        }
        else
        {
            anim.SetBool("Reload", false);
        }
    }
}
