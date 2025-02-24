using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuzzleAssult : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPoint;
    [SerializeField] float ShootForce;
    [SerializeField] float ShootingTime;
    [SerializeField] float ReloadTime;
    [SerializeField] int MagazineSize;
    [SerializeField] int BulletPerTap;
    [SerializeField] bool ButtonHold;

    [SerializeField] GameObject effect;  // エフェクト用のGameObject
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] Text ammoText; // 残弾数と所持弾数を表示するテキスト

    GameObject playerCam;

    int bulletsShot, bulletsLeft;
    int totalAmmo = 1000; // 所持弾数
    bool Shooting, readyToShoot;
    public bool reloading;
    public bool allowInvoke = true;
    public Transform cameraTransform; // カメラのTransformを設定
    public float rotationSpeed = 5f;

    void Start()
    {
        playerCam = GameObject.FindWithTag("MainCamera");
        bulletsLeft = MagazineSize;
        readyToShoot = true;
        UpdateAmmoText();
    }

    void Update()
    {
        ImputHandler();
    }

    private void ImputHandler()
    {
        if (ButtonHold)
        {
            Shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            Shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && Shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < MagazineSize && !reloading && totalAmmo > 0)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

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

        // 弾丸を生成して発射 (銃口のRotationを適用)
        GameObject newbullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        Rigidbody bulletRigidbody = newbullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(this.cameraTransform.forward * ShootForce * 2);
        Destroy(newbullet, 20f);

        // エフェクトをMuzzleの位置に生成して削除
        GameObject newEffect = Instantiate(effect, shootPoint.position, shootPoint.rotation, shootPoint); // 銃口を親に設定
        Destroy(newEffect, 0.5f);

        bulletsLeft--;
        bulletsShot++;
        UpdateAmmoText();

        if (allowInvoke)
        {
            Invoke("ResetShot", ShootingTime);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        int bulletsToReload = MagazineSize - bulletsLeft;
        int bulletsToDeduct = Mathf.Min(bulletsToReload, totalAmmo);
        totalAmmo -= bulletsToDeduct;
        bulletsLeft += bulletsToDeduct;
        UpdateAmmoText();
        Invoke(nameof(ReloadFinished), ReloadTime);
    }

    private void ReloadFinished()
    {
        reloading = false;
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = $"残弾数: {bulletsLeft}/{MagazineSize} | 所持弾数: {totalAmmo}";
        }
    }
}
