using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleEvent : MonoBehaviour
{
    public GameObject Reticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Reticle.SetActive(true);
        }
        else
        {
            Reticle.SetActive(false);
        }
    }
}
