using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public Transform gunTransform; // �e��Transform
    public float recoilAngleVertical = 30.0f; // �c�����̃��R�C���p�x
    public float recoilAngleHorizontal = 10.0f; // �������̃��R�C���p�x
    public float recoilSpeed = 10.0f; // ���R�C����ɖ߂�X�s�[�h
    private Quaternion originalRotation;
    private Quaternion currentRecoil;

    void Start()
    {
        // �e�̌��̉�]��ۑ�
        originalRotation = gunTransform.localRotation;
        currentRecoil = originalRotation;
    }

    void Update()
    {
        // �{�^�����N���b�N����Ă����
        if (Input.GetMouseButton(0)) // "Fire1"��Unity�̓��̓}�l�[�W���[�Őݒ肳�ꂽ���˃{�^��
        {
            ApplyRecoil();
        }

        // �e�̉�]�����̏�Ԃɖ߂�
        currentRecoil = Quaternion.Lerp(currentRecoil, originalRotation, Time.deltaTime * recoilSpeed);
        gunTransform.localRotation = currentRecoil;
    }

    void ApplyRecoil()
    {
        // �c�����̃��R�C����K�p
        float verticalRecoil = -recoilAngleVertical * Time.deltaTime;

        // �������̃��R�C���������_���ɓK�p
        float horizontalRecoil = Random.Range(-recoilAngleHorizontal, recoilAngleHorizontal) * Time.deltaTime;

        // �e�̉�]���X�V
        Quaternion recoilRotation = Quaternion.Euler(verticalRecoil, horizontalRecoil, 0);
        currentRecoil *= recoilRotation;
    }
}
