using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movetest : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private bool rotateClockwise = true;

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float rotationAngle = rotateClockwise ? 90f : -90f;

            transform.Rotate(0, rotationAngle, 0);

            rotateClockwise = !rotateClockwise;
        }
    }
}
