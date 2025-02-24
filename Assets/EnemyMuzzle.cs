using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMuzzle : MonoBehaviour
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
    private bool Reload = false;
    [SerializeField] GameObject effect;

    private float magazineLeft;
    private Animator enemyAnimator; // アニメーション用のAnimator

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("PlayerPMC");
        if (player != null)
        {
            playertransform = player.transform;
        }
        else
        {
            Debug.LogError("PlayerPMC not found!");
        }

        // 親オブジェクトからAnimatorを取得
        enemyAnimator = GetComponentInParent<Animator>();
        if (enemyAnimator == null)
        {
            Debug.LogError("Animator component not found in parent! Attach an Animator to the parent object.");
        }

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

        if (distancePlayer < BattleRange && ReadyFire && !Reload && IsPlayerVisible())
        {
            Attack();
        }
    }

    private bool IsPlayerVisible()
    {
        Vector3 directionToPlayer = (playertransform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, BattleRange))
        {
            if (hit.transform == playertransform)
            {
                return true;
            }
        }
        return false;
    }

    public void Attack()
    {
        if (magazineLeft <= 0) // 弾がない場合、リロード処理に移行
        {
            Reloading();
            return;
        }

        ReadyFire = false;

        // ターゲットの方向を計算し、銃口の回転を更新
        Vector3 directionToPlayer = (playertransform.position - Gun.position).normalized;
        Gun.rotation = Quaternion.LookRotation(directionToPlayer);

        // 弾丸を銃口の位置と回転で生成
        GameObject bullet = Instantiate(bulletenemy, Gun.position, Gun.rotation);

        // 発射エフェクトを銃口の位置と回転で生成
        GameObject newEffect = Instantiate(effect, Gun.position, Gun.rotation, Gun);
        Destroy(newEffect, 0.5f);

        // 弾丸の飛行方向を計算し、Rigidbodyに速度を適用
        Vector3 spreadVector = new Vector2(
            Random.Range(-spread, spread),
            Random.Range(-spread, spread)
        );

        Vector3 bulletDirection = directionToPlayer + spreadVector;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletDirection * bulletspeed;
        }

        if (allowInvoke)
        {
            Invoke("ResetShot", 1f / firerate);
            allowInvoke = false;
        }

        magazineLeft--; // マガジンの弾を減少
        Destroy(bullet, 10);
    }

    private void ResetShot()
    {
        ReadyFire = true;
        allowInvoke = true;
    }

    public void Reloading()
    {
        if (Reload) return; // 既にリロード中の場合は何もしない
        Reload = true;

        Debug.Log("Reloading...");
        
        // リロードアニメーションをトリガーで開始
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Reloaded"); // "Reloaded"トリガーをAnimatorに設定
        }

        // リロード処理を遅延して完了させる
        Invoke(nameof(FinishReloading), ReloadTime);
    }

    private void FinishReloading()
    {
        magazineLeft = MagazineSize; // マガジンをフル補充
        Reload = false;

        Debug.Log("Reload complete.");
    }
}
