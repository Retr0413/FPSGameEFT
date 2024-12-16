using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMuzzle : MonoBehaviour
{
    public GameObject bulletenemy;
    public Transform Gun;
    public float firerate = 0.2f;
    public float bulletspeed = 200f;
    public float spread = 0.3f;
    public float BattleRange = 1000f;
    public float MagazineSize = 1f;
    public float ReloadTime = 100f;

    private Transform playertransform;
    private bool ReadyFire = true;
    private bool allowInvoke = true;
    private bool Reload;

    public float magazineLeft;

    void Start()
    {
        GameObject player = GameObject.Find("PlayerPMC");
        playertransform = player.transform;
        magazineLeft = MagazineSize;
    }

    void Update()
    {
        if (playertransform == null)
        {
            return;
        }

        float distancePlayer = Vector3.Distance(transform.position, playertransform.position);
        if (distancePlayer < BattleRange && ReadyFire && !Reload)
        {
            Attack();
        }
    }

    public void Attack()
    {
        ReadyFire = false;

        Vector3 directionToPlayer = (playertransform.position - Gun.position).normalized;

        Gun.rotation = Quaternion.LookRotation(directionToPlayer);

        Instantiate(bulletenemy, Gun.position, Gun.rotation);

        if (allowInvoke)
        {
            Invoke("ResetShot", 1f / firerate);
            allowInvoke = false;
        }

        if (magazineLeft <= 0)
        {
            Reloading();
            return;
        }

        magazineLeft--;
    }

    private void ResetShot()
    {
        ReadyFire = true;
        allowInvoke = true;
    }

    private void Reloading()
    {
        Reload = true;
        Invoke(nameof(Set), ReloadTime);
    }

    private void Set()
    {
        magazineLeft = MagazineSize;
        Reload = false;
    }
}
