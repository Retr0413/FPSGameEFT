using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunMuzzle : MonoBehaviour
{
    public GameObject bulletenemy;
    public Transform Gun;
    public float firerate = 2f;
    public float bulletspeed = 200f;
    public float spread = 0.4f;
    public float BattleRange = 50f;
    public float MagazineSize = 10f;
    public float ReloadTime = 10f;

    private Transform playertransform;
    private bool ReadyFire = true;
    private bool allowInvoke = true;
    private bool Reload;
    [SerializeField] GameObject effect;

    public float magazineLeft;
    public int shotgunPellets = 9; // ショットガンの弾の数

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("PlayerPMC");
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
    }

    public void Attack()
    {
        ReadyFire = false;

        Vector3 directionToPlayer = (playertransform.position - Gun.position).normalized;
        Gun.rotation = Quaternion.LookRotation(directionToPlayer);

        // エフェクト生成
        GameObject newEffect = Instantiate(effect, Gun.position, Gun.rotation, Gun); 
        Destroy(newEffect, 0.5f);

        for (int i = 0; i < shotgunPellets; i++)
        {
            // 弾生成
            GameObject bullet = Instantiate(bulletenemy, Gun.position, Gun.rotation);

            // 拡散計算
            Vector3 spreadVector = new Vector3(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                Random.Range(-spread, spread) // 3D空間での拡散
            );

            Vector3 bulletDirection = (directionToPlayer + spreadVector).normalized;
            bullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletspeed;

            Destroy(bullet, 10); // 弾の寿命
        }

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

    public void Reloading()
    {
        // リロードアニメーションをトリガー
        // EnemyMoveAI.instance.TriggerReloadAnimation();
        Reload = true;
        Invoke(nameof(Set), ReloadTime);
    }

    private void Set()
    {
        magazineLeft = MagazineSize;
        Reload = false;
    }
}
