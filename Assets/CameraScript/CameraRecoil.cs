using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cameraTransform; // カメラのTransform
    public float recoilAngle = 5.0f; // リコイルの角度
    public float recoilSpeed = 5.0f; // リコイル後に戻るスピード
    public float recoilAngleHorizontal = 10.0f;
    private Quaternion originalRotation;

    void Start()
    {
        // カメラの元の回転を保存
        originalRotation = cameraTransform.localRotation;
    }

    void Update()
    {
        // ボタンがクリックされたとき
        if (Input.GetMouseButton(0)) // "Fire1"はUnityの入力マネージャーで設定された発射ボタン
        {
            ApplyRecoil();
        }

        // カメラの回転を元の状態に戻す
        cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, originalRotation, Time.deltaTime * recoilSpeed);
    }

    void ApplyRecoil()
    {
        // カメラの回転を上昇させる
        Quaternion recoilRotation = Quaternion.Euler(-recoilAngle, 0, 0);
        float horizontalRecoil = Random.Range(-recoilAngleHorizontal, recoilAngleHorizontal);
        cameraTransform.localRotation *= recoilRotation;
    }
}
