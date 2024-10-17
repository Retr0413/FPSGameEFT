using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] GameObject heal;
    int maxHp = 155;
    int currentHp;
    public Slider slider;
    public float customY = 10.0f;

    void Start()
    {
        slider.value = 1;
        currentHp = maxHp;
    }

    void Update()
    {
        slider.transform.rotation = Camera.main.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            int damage = Random.Range(1, 100);
            currentHp -= damage;
            slider.value = (float)currentHp / (float)maxHp;

            if (currentHp <= 0)
            {
                // Instantiate heal prefab and adjust its position
                GameObject healInstance = Instantiate(heal, this.transform.position, Quaternion.identity);
                Vector3 healPosition = healInstance.transform.position;
                healPosition.y += 2.0f;
                healInstance.transform.position = healPosition;

                // カウントを増やす
                EnemyCount counter = FindObjectOfType<EnemyCount>();
                if (counter != null)
                {
                    counter.IncrementEnemyCount(1);  // ここでカウントを1プラス
                }

                Destroy(this.gameObject);
            }
        }
    }
}
