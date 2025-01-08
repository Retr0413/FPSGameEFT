using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    // プレハブの参照（敵の種類）
    public GameObject enemyType1;
    public GameObject enemyType2;

    // 敵が出現する範囲を設定
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    // 出現させる敵の総数
    public int totalEnemies = 10;


    public Transform parentObject;

    void Start()
    {
        GenerateEnemies();
    }

    void GenerateEnemies()
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            GameObject selectedEnemy = Random.Range(0, 2) == 0 ? enemyType1 : enemyType2;

            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            GameObject enemy = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);

            if (parentObject != null)
            {
                enemy.transform.SetParent(parentObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((spawnAreaMin + spawnAreaMax) / 2, spawnAreaMax - spawnAreaMin);
    }
}
