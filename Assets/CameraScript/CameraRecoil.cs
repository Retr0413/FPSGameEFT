using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cameraTransform; // �J������Transform
    public float recoilAngle = 5.0f; // ���R�C���̊p�x
    public float recoilSpeed = 5.0f; // ���R�C����ɖ߂�X�s�[�h
    public float recoilAngleHorizontal = 10.0f;
    private Quaternion originalRotation;

    void Start()
    {
        // �J�����̌��̉�]��ۑ�
        originalRotation = cameraTransform.localRotation;
    }

    void Update()
    {
        // �{�^�����N���b�N���ꂽ�Ƃ�
        if (Input.GetMouseButton(0)) // "Fire1"��Unity�̓��̓}�l�[�W���[�Őݒ肳�ꂽ���˃{�^��
        {
            ApplyRecoil();
        }

        // �J�����̉�]�����̏�Ԃɖ߂�
        cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, originalRotation, Time.deltaTime * recoilSpeed);
    }

    void ApplyRecoil()
    {
        // �J�����̉�]���㏸������
        Quaternion recoilRotation = Quaternion.Euler(-recoilAngle, 0, 0);
        float horizontalRecoil = Random.Range(-recoilAngleHorizontal, recoilAngleHorizontal);
        cameraTransform.localRotation *= recoilRotation;
    }
}
