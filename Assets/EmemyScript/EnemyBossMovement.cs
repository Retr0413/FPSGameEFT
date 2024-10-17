using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBossMovement : MonoBehaviour
{
    Rigidbody rd;
    private Animator anim;
    public GameObject player;
    public float distance;
    public float speed = 5f; // プレイヤーを追いかける速度
    int MaxHP = 200;
    int currentHP;
    public Slider slider;
    private float originalHeight; // 元のy座標を保存
    private float targetHeight; // 目標のy座標
    private float heightChangeSpeed = 10f; // y座標の変更速度
    private bool isCircling = false; // 旋回中かどうかを判定するフラグ
    private float circleSpeed = 70f; // 旋回の速度
    private Vector3 circlingCenter; // 旋回の中心となる位置
    private float circleRadius = 20f; // 旋回の半径
    private float angle = 0f; // 旋回角度を管理する変数
    public GameObject breathPrefab; // 飛ばすプレハブ
    public Transform breathSpawnPoint; // プレハブを飛ばす位置
    public float breathSpeed = 20f; // プレハブの飛ばす速度
    private float nextBreathTime = 0f; // 次のブレス攻撃までのクールダウン時間
    public float breathCooldown = 2f; // ブレス攻撃のクールダウン
    public float breathDuration = 1.0f; // ブレスアニメーションの継続時間
    private bool isAttacking; // 攻撃の状態を管理するブール
    private bool hasReturnedToOriginalHeight = false; // 元の高さに戻ったかどうかを管理するブール
    public GameObject bossAttackObject; // BossAttackのゲームオブジェクト
    private BoxCollider bossAttackCollider; // BossAttackのコライダー

    private bool isDying = false; // 死亡中かどうかを管理するブール
    private float deathTimer = 2.0f; // 死亡後の待機時間

    // Start is called before the first frame update
    void Start()
    {
        rd = this.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("RPGHeroHP");
        originalHeight = transform.position.y; // 開始時のy座標を保存
        currentHP = MaxHP;
        slider.value = 1;
        targetHeight = originalHeight; // 初期目標高さは元の高さに設定
        isAttacking = false; // 初期状態は攻撃していない

        // BossAttackオブジェクトのコライダーを取得
        bossAttackCollider = bossAttackObject.GetComponent<BoxCollider>();
        bossAttackCollider.enabled = false; // 初期状態ではコライダーをオフにする
    }

    // Update is called once per frame
    void Update()
    {
        if (isDying)
        {
            // 死亡処理中はタイマーをカウントダウン
            deathTimer -= Time.deltaTime;

            if (deathTimer <= 0f)
            {
                // カウントを増やす
                EnemyCount counter = FindObjectOfType<EnemyCount>();
                if (counter != null)
                {
                    counter.IncrementEnemyCount(5);  // ここでカウントを1プラス
                }

                // ゲームオブジェクトを削除
                Destroy(this.gameObject);
            }
            return; // 他の処理を中断
        }

        slider.transform.rotation = Camera.main.transform.rotation;

        distance = Vector3.Distance(transform.position, player.transform.position);

        // アニメーションの制御
        if (distance > 100f)
        {
            rd.useGravity = false;
            anim.SetBool("Flying", true);
            anim.SetBool("NoFlying", false);
            isCircling = true; // 旋回を有効にする
            targetHeight = 50f; // 目標y座標を50に設定

            // 旋回を開始する位置を設定
            if (!isCircling || angle == 0f) // 初回のみ設定する
            {
                circlingCenter = transform.position; // 現在の位置を旋回の中心として設定
                angle = 0f; // 旋回角度をリセット
            }
        }
        else
        {
            anim.SetBool("Flying", false);
            anim.SetBool("NoFlying", true);
            isCircling = false; // 旋回を無効にする
            targetHeight = originalHeight; // 元の高さに戻る
            rd.useGravity = true;
        }

        // 200から100の間の距離の場合、速度を2倍にしてRunアニメーションをオンにする
        if (distance <= 90f && distance > 20f)
        {
            anim.SetBool("Run", true);
            speed = 10f; // 速度を2倍にする

            // ブレスアニメーションとブレス攻撃
            if (Time.time >= nextBreathTime)
            {
                anim.SetBool("Breath", true); // ブレスアニメーションを再生

                // プレハブからオブジェクトを生成し、飛ばす
                // GameObject breath = Instantiate(breathPrefab, breathSpawnPoint.position, breathSpawnPoint.rotation);
                // Rigidbody rb = breath.GetComponent<Rigidbody>();
                // rb.velocity = breathSpawnPoint.forward * breathSpeed;

                // // 10秒後にプレハブを破壊
                // Destroy(breath, 10f);

                // ブレスアニメーションの終了をタイミングを設定
                EndBreathAnimation();

                // 次のブレス攻撃までのクールダウンを設定
                nextBreathTime = Time.time + breathCooldown;
            }
        }
        else
        {
            anim.SetBool("Run", false);
            speed = 5f; // 速度を元に戻す
        }

        // 1以下の距離の場合、ランダムにAttack1かAttack2をブールで実行
        if (distance <= 4f && !isAttacking)
        {
            isAttacking = true; // 攻撃を開始

            // 1か2をランダムで選び、攻撃を実行
            bool attack1 = Random.Range(0, 2) == 0; // 50%の確率でAttack1かAttack2を選択

            if (attack1)
            {
                anim.SetBool("Attack1", true);
                anim.SetBool("Attack2", false);
            }
            else
            {
                anim.SetBool("Attack1", false);
                anim.SetBool("Attack2", true);
            }

            // 一定時間後に攻撃を解除
            EndAttackAnimation();
        }

        // プレイヤーを追いかける動き
        if (distance > 1f && distance <= 100f)
        {
            isCircling = false; // 旋回を無効にする
            // プレイヤーに向かって移動
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // プレイヤーの方向を向くが、x軸の回転を制限
            Vector3 lookDirection = player.transform.position - transform.position;
            lookDirection.y = 0; // y成分を無視して水平面にのみ向ける
            transform.rotation = Quaternion.LookRotation(lookDirection);

            // 一度元の高さに戻る
            if (!hasReturnedToOriginalHeight)
            {
                targetHeight = originalHeight;
            }
        }

        // 徐々に目標のy座標に近づく
        if (!hasReturnedToOriginalHeight && transform.position.y != targetHeight)
        {
            float newY = Mathf.MoveTowards(transform.position.y, targetHeight, heightChangeSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // 元の高さに戻ったらフラグを立てる
            if (transform.position.y == targetHeight)
            {
                hasReturnedToOriginalHeight = true;
            }
        }

        // その場で半径20の旋回動作
        if (isCircling)
        {
            angle += circleSpeed * Time.deltaTime; // 角度を増加
            float radian = angle * Mathf.Deg2Rad; // 角度をラジアンに変換
            float x = Mathf.Cos(radian) * circleRadius; // x座標を計算
            float z = Mathf.Sin(radian) * circleRadius; // z座標を計算

            // 旋回中心からの新しい位置を設定
            Vector3 newPosition = new Vector3(circlingCenter.x + x, transform.position.y, circlingCenter.z + z);

            // 進行方向を向く
            Vector3 forwardDirection = newPosition - transform.position;
            forwardDirection.y = 0; // y成分を無視して水平面にのみ向ける
            transform.rotation = Quaternion.LookRotation(forwardDirection);

            // 新しい位置を適用
            transform.position = newPosition;
        }

        // HPに基づく行動の処理
        if (currentHP < 150)
        {
            // HPが150未満の場合の処理
        }
        if (currentHP <= 0 && !isDying)
        {
            Die();
        }
    }

    // 衝突判定
    public void OnCollisionEnter(Collision hit)
    {
        Debug.Log("hit");
        if (hit.gameObject.tag == "Sword")
        {
            // 剣との衝突時の処理
            int damage = Random.Range(20, 80);
            currentHP = currentHP - damage;
            slider.value = (float)currentHP / (float)MaxHP;
        }
    }

    // ブレスアニメーションを終了するメソッド
    private void EndBreathAnimation()
    {
        anim.SetBool("Breath", false);
    }

    // 攻撃アニメーションを終了するメソッド
    private void EndAttackAnimation()
    {
        // 攻撃アニメーション中はコライダーをオンにする
        bossAttackCollider.enabled = true; 
        // 攻撃アニメーションの継続時間
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        isAttacking = false; // 攻撃を解除
        bossAttackCollider.enabled = false; // 攻撃が終了したらコライダーを無効にする
    }

    // ボスが死亡した際の処理
    private void Die()
    {
        anim.SetBool("Die", true); // Dieアニメーションをオンにする
        isDying = true; // 死亡フラグを立てる
        deathTimer = 2.0f; // 2秒間の待機タイマーを設定
    }
}
