using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
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

    // ダッシュ関連の変数
    public float dashSpeed = 10f;  // ダッシュ時の移動速度
    public KeyCode dashKey = KeyCode.LeftShift;  // ダッシュに使うキー
    private bool isDashing = false;  // ダッシュ中かどうか

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        // カーソルをロックして非表示にする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        Move();
        Look();
    }

    public void Move()
    {
        // 水平方向と垂直方向の入力を取得
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // ダッシュの判定：Wキーとシフトキーが押されている、もしくはWシフトとAかD
        isDashing = Input.GetKey(KeyCode.W) && Input.GetKey(dashKey) &&
                    (horizontal != 0 || Input.GetKey(KeyCode.W));

        // ダッシュ時と通常時の移動速度を設定
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;

        // プレイヤーの移動
        float xMovement = horizontal * currentSpeed * Time.deltaTime;
        float zMovement = vertical * currentSpeed * Time.deltaTime;
        transform.Translate(xMovement, 0, zMovement);
    }

    public void Look()
    {
        // マウスの移動量を取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 上下方向のカメラ回転
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // カメラの上下回転
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // プレイヤーの左右回転
        transform.Rotate(Vector3.up * mouseX);
    }
}
