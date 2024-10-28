using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMuzzle : MonoBehaviour
{
    public GameObject bulletdrone;
    public Transform Gun;
    public float firerate = 0.2f;
    public float bulletspeed = 200f;
    public float spread = 0.1f;
    public float BattleRange = 20f;
    public float MagazineSize = 50f;
    public float ReloadTime = 5f;
    [SerializeField] GameObject effect;

    private Transform enemytransform;
    private bool ReadyFire = true;
    private bool allowInvoke = true;
    private bool Reload;

    public float magazineLeft;


    // Start is called before the first frame update
    void Start()
    {
        GameObject enemy = GameObject.Find("Rougue Variant");
        enemytransform = enemy.transform;
        magazineLeft = MagazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemytransform == null)
        {
            return;
        }

        float distanceenemy = Vector3.Distance(transform.position, enemytransform.position);
        if (distanceenemy < BattleRange && ReadyFire && !Reload)
        {
            Attack();
        }
    }
    public void Attack()
    {
        ReadyFire = false;

        Vector3 directionToenemy = (enemytransform.position - Gun.position).normalized;
        Gun.rotation = Quaternion.LookRotation(directionToenemy);

        GameObject bullet = Instantiate(bulletdrone, Gun.position, Gun.rotation);

        Vector3 spreadVector = new Vector2(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread)
        );

        Vector3 bulletDirection = directionToenemy + spreadVector;
        bullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletspeed;

        GameObject newEffect = Instantiate(effect, Gun.position, Gun.rotation, Gun); 
        Destroy(newEffect, 0.5f); 

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
