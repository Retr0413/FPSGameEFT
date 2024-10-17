using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMove : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public string targetObjectName = "SightLook"; // ターゲットオブジェクトの名前
    public string reverseLookObjectName = "ReverseLook"; // 追従するオブジェクトの名前

    private GameObject targetObject;
    private GameObject reverseLookObject;
    private bool isAiming = false; // エイム中かどうかのフラグ
    private Vector3 targetPosition;
    private Vector3 reverseLookPosition;

    void Start()
    {
        targetObject = GameObject.Find(targetObjectName);
        reverseLookObject = GameObject.Find(reverseLookObjectName);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 右クリックが押されたとき
        {
            isAiming = true;
        }

        if (Input.GetMouseButtonUp(1)) // 右クリックが離されたとき
        {
            isAiming = false;
        }

        if (isAiming)
        {
            AimAndMove();
        }
        else
        {
            FollowReverseLook();
        }
    }

    void AimAndMove()
    {
        if (targetObject == null) return;

        targetPosition = targetObject.transform.position;
        MoveToTarget(targetPosition);
        Debug.Log("Aiming and moving towards target: " + targetPosition);
    }

    void MoveToTarget(Vector3 position)
    {
        Vector3 direction = position - transform.position;

        if (direction.magnitude > 0.1f)
        {
            Vector3 move = direction.normalized * moveSpeed * Time.deltaTime;
            transform.position += move;
            Debug.Log("Moving towards: " + position + " | Current position: " + transform.position);
        }
        else
        {
            transform.position = position;
            Debug.Log("Reached position: " + position);
        }
    }

    void FollowReverseLook()
    {
        if (reverseLookObject == null) return;

        reverseLookPosition = reverseLookObject.transform.position;
        MoveToTarget(reverseLookPosition);
        Debug.Log("Following reverse look object: " + reverseLookPosition);
    }
}
