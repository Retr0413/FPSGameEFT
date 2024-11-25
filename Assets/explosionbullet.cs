using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionbullet : MonoBehaviour
{
    public GameObject explosionEffect; // 爆発エフェクト
    public float lifeTime = 10f;       // 弾丸の寿命

    void Start()
    {
        // 一定時間後に削除
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 敵または地面に衝突したときの処理
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
