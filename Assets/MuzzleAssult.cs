using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        playerCam = GameObject.FindWithTag("MainCamera");
        bulletsLeft = MagazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
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
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < MagazineSize && !reloading)
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

        GameObject newbullet = Instantiate(bullet, this.transform.position, Quaternion.identity);
        Rigidbody bulletRigidbody = newbullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(this.cameraTransform.forward * ShootForce * 2);
        Destroy(newbullet, 20f);

        bulletsLeft--;
        bulletsShot++;

        if(allowInvoke)
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
        Invoke(nameof(ReloadFinished), 1);
    }
    private void ReloadFinished()
    {
        bulletsLeft = MagazineSize;
        reloading = false;
    }
}
