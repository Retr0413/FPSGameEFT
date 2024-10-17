using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform CamHolder;

    float xRotation;
    float yRotation;

    bool isMoving = false;

    void Start()
    {
        //マウスポインターを真ん中に固定し見えなくする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
           FreeLook();
        }
    }

    public void FreeLook()
    {

        //マウスの入力を取得
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        //Y軸だけ視点に上限を設ける
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //カメラとプレイヤーの向きを動かす
        CamHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
