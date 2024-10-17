using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPoint;
    [SerializeField] float ShootForce;
    [SerializeField] float ShootingTime;
    [SerializeField] float ReloadTime;
    [SerializeField] int MagazineSize;
    [SerializeField] int BulletPerTap;
    [SerializeField] bool ButtonHold;

    [SerializeField] LayerMask ignoreLayer;

    GameObject playerCam;

    int bulletsShot, bulletsLeft;
    bool Shooting, readyToShoot;
    public bool reloading;
    public bool allowInvoke = true;
    public Transform cameraTransform; // カメラのTransformを設定
    public float rotationSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.Find("Main Camera");
        bulletsLeft = MagazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
    }

    // ���͐���
    private void InputHandler()
    {
        // ���������ł��邩�̓��͂���������
        if (ButtonHold)
        {
            Shooting = Input.GetMouseButton(0);
        }
        else
        {
            Shooting = Input.GetMouseButtonDown(0);
        }

        // �łĂ��Ԃ̃`�F�b�N
        if (readyToShoot && Shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }

        // �����[�h
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < MagazineSize && !reloading)
        {
            Reload();
        }
    }

    // �e�𔭎�
    private void Shoot()
    {
        readyToShoot = false;

        // ��ʂ̒����ɔ�΂�
        Ray ray = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, 1000f, ~ignoreLayer))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(10);
        }

        GameObject newbullet = Instantiate(bullet, this.transform.position, Quaternion.identity); //�e�𐶐�
        Rigidbody bulletRigidbody = newbullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(this.transform.forward * ShootForce * 2); //�L�����N�^�[�������Ă�������ɒe�ɗ͂�������
        Destroy(newbullet, 10); //10�b��ɒe������

        bulletsLeft--;
        bulletsShot++;

        // �e�ƒe�ɊԊu���󂯂�
        if (allowInvoke)
        {
            Invoke("ResetShot", ShootingTime);
            allowInvoke = false;
        }
    }

    // ���Ă��Ԃɂ���
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    // �����[�h�̏���
    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadFinished), 1);
    }

    private void ReloadFinished()
    {
        bulletsLeft = MagazineSize;
        reloading = false;
    }
}
