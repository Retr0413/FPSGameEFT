using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerapos : MonoBehaviour
{
    public Transform camerapos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.position = camerapos.position;
        }
        
    }
}
