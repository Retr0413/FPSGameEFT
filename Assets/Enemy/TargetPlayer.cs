using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    public GameObject bulletenemy;
    public Transform GunTransform;
    public float firerate;
    public float bulletspeed;
    public float spread;
    public float BattleRange;
    public int MagazineSize;
    public float ReloadTime;

    private Transform playertransform;
    private bool ReadyFire = true;
    private bool allowInvoke = true;
    private bool Reload;

    int magazineLeft;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playertransform = player.transform;
        magazineLeft = MagazineSize;
    }

    // Update is called once per frame
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

        //if (magazineLeft <= 0 && !Reload)
        //{
        //Reloading();
        //}
    }

    public void Attack()
    {
        ReadyFire = false;

        // ÉvÉåÉCÉÑÅ[ÇÃï˚å¸Ç…èeÇå¸ÇØÇÈ
        Vector3 directionToPlayer = (playertransform.position - GunTransform.position).normalized;
        GunTransform.rotation = Quaternion.LookRotation(directionToPlayer);

        // íeÇê∂ê¨
        GameObject bullet = Instantiate(bulletenemy, GunTransform.position, GunTransform.rotation);

        // íeÇÃÇŒÇÁÇ¬Ç´Çí«â¡
        Vector3 spreadVector = new Vector2(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread)
        );

        // íeÇÃï˚å¸Çê›íË
        Vector3 bulletDirection = directionToPlayer + spreadVector;
        bullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletspeed;

        // åÇÇƒÇÈèÛë‘ÇÉäÉZÉbÉg
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
        Destroy(bullet, 10);
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
