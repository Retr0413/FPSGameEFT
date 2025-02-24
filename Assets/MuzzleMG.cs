using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleMG : MonoBehaviour
{
    public GameObject bulletenemy;
    public Transform Gun;
    public float firerate = 0.2f;
    public float bulletspeed = 200f;
    public float spread = 0.4f;
    public float BattleRange = 50f;
    public float MagazineSize = 30f;
    public float ReloadTime = 10f;

    private Transform playertransform;
    private bool ReadyFire = true;
    private bool allowInvoke = true;
    private bool Reload;
    [SerializeField] GameObject effect;

    public float magazineLeft;

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

        // プレイヤーが攻撃範囲内で、視界に入っている場合のみ攻撃する
        if (distancePlayer < BattleRange && ReadyFire && !Reload && IsPlayerVisible())
        {
            Attack();
        }
    }

    // プレイヤーが視界に入っているかチェックするメソッド
    private bool IsPlayerVisible()
    {
        // プレイヤーへの方向を計算
        Vector3 directionToPlayer = (playertransform.position - transform.position).normalized;

        // Raycastを飛ばして、間に障害物があるか確認
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, BattleRange))
        {
            // Raycastがプレイヤーにヒットした場合、視界に入っている
            if (hit.transform == playertransform)
            {
                return true;
            }
        }

        // 障害物があれば視界に入っていない
        return false;
    }

    public void Attack()
    {
        ReadyFire = false;

        Vector3 directionToPlayer = (playertransform.position - Gun.position).normalized;
        Gun.rotation = Quaternion.LookRotation(directionToPlayer);

        GameObject bullet = Instantiate(bulletenemy, Gun.position, Gun.rotation);
        GameObject newEffect = Instantiate(effect, Gun.position, Gun.rotation, Gun);  // Muzzleを親としてエフェクトを生成
        Destroy(newEffect, 0.5f);

        Vector3 spreadVector = new Vector2(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread)
        );

        Vector3 bulletDirection = directionToPlayer + spreadVector;
        bullet.GetComponent<Rigidbody>().velocity = bulletDirection * bulletspeed;

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

    public void Reloading()
    {
        EnemyMoveAI.instance.TriggerReloadAnimation();
        Reload = true;
        Invoke(nameof(Set), ReloadTime);
    }

    private void Set()
    {
        magazineLeft = MagazineSize;
        Reload = false;
    }
}
