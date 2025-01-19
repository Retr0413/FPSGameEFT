using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // 衝突時の処理
    private void OnCollisionEnter(Collision collision)
    {
        // 壁や床などに衝突した際にオブジェクトを削除
        Destroy(gameObject, 0.1f);
    }// 衝突時の処理
    private void OnTriggerEnter(Collider other)
    {
        // コルーチンを開始して 0.1 秒後に消去
        Destroy(gameObject, 0.1f);
    }
}
