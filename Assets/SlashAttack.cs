using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 5秒後にオブジェクトを削除
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        // Z軸方向に速さ10で移動
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Enemy または Boss タグを持つオブジェクトに当たった場合に消える
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }
    }
}
