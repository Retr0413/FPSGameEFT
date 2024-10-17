using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float groundDrag;

    public float playerHeight;
    public LayerMask Ground;
    bool grounded;

    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    bool readyToJump;

    public Transform orientation;

    float horizontal;
    float vertical;

    Vector3 moveDirection;

    Rigidbody rb;

    public float mouseSensitivity = 100f;
    float xRotation = 0f;

    public Transform gun; // �e��Transform���Q�Ƃ��邽�߂̕ϐ�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        // �J�[�\�������b�N���ĉ�ʂ̒����ɌŒ肷��
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // �n�ʂƐڂ��Ă��邩�𔻒f
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        // �ڂ��Ă���ꍇ�́A�ݒ肵�������l�������v���C���[������ɂ�������
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        Move();
        Look();
    }

    public void Move()
    {
        float xMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float zMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(xMovement, 0, zMovement);
    }

    public void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // �c�����̉�]
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // �e�̏c�����̉�]���X�V
        if (gun != null)
        {
            gun.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // �������̉�]
        transform.Rotate(Vector3.up * mouseX);
    }
}
