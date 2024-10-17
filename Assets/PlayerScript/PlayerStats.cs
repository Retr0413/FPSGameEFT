using Cysharp.Threading.Tasks;
using UnityEngine;
using System; // 追加するusingディレクティブ

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;
    public GameObject Healeffect;
    public GameObject AddHP;

    #region Singleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxTotalHealth;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }

    private bool canTakeDamage = true; // ダメージを受けるかどうかのフラグ
    private bool isCooldown = false;   // クールダウン中かどうかのフラグ

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isCooldown) // 右クリックが押された時、かつクールダウン中でない場合
        {
            StartDamageImmunity();
        }
    }

    private async void StartDamageImmunity()
    {
        canTakeDamage = false; // ダメージ無効化
        isCooldown = true; // クールダウン開始
        Debug.Log("Damage immunity activated");

        await UniTask.Delay(TimeSpan.FromSeconds(4)); // 4秒間ダメージ無効

        canTakeDamage = true; // ダメージ無効解除
        Debug.Log("Damage immunity ended");

        await UniTask.Delay(TimeSpan.FromSeconds(15)); // 15秒のクールダウン

        isCooldown = false; // クールダウン終了
        Debug.Log("Cooldown ended, you can use damage immunity again");
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        if (canTakeDamage) // ダメージを受けられる場合のみダメージを受ける
        {
            health -= dmg;
            ClampHealth();
        }
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }

    async void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // 右クリックが押されていないときだけダメージを受ける
            if (!Input.GetMouseButton(1)) // 1は右クリックを表します
            {
                TakeDamage(1);
            }
        }
        if (collision.gameObject.tag == "Boss")
        {
            // 右クリックが押されていないときだけダメージを受ける
            if (!Input.GetMouseButton(1)) // 1は右クリックを表します
            {
                TakeDamage(3);
            }
        }
    }

    void OnTriggerEnter(Collider heal)
    {
        if (heal.gameObject.tag == "Heal")
        {
            Debug.Log("HIT");
            Heal(5);
            var spHealeffect = Instantiate(Healeffect, transform.position, transform.rotation);
            StartHeal(spHealeffect);
        }

        else if (heal.gameObject.tag == "AddHP")
        {
            Debug.Log("HIT");
            AddHealth();
            var spHPeffect = Instantiate(AddHP, transform.position, transform.rotation);
            StartHP(spHPeffect);
        }
    }

    private async void StartHeal(GameObject prefab)
    {
        prefab.transform.position = transform.position;
        Destroy(prefab, 0.5f);
    }

    private async void StartHP(GameObject prefab)
    {
        prefab.transform.position = transform.position;
        Destroy(prefab, 0.5f);
    }
}
