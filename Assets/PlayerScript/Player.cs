using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Player : MonoBehaviour
{
    AudioSource audioSource; // オーディオソース
    public AudioClip footstep; // 流したい音の設定
    private bool isRunning;

    private Vector3 knockbackVelocity = Vector3.zero;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float jumpPower;

    private CharacterController characterController;
    private Vector3 moveDirection;

    public float sensitivityX = 15f;
    public float sensitivityY = 15f;

    public float minimumX = -360f;
    public float maximumX = 360f;

    public float minimumY = -60f;
    public float maximumY = 60f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    public GameObject mainCam;

    private Animator anim;

    public GameObject sword; // 剣
    private BoxCollider swordCollider; // 剣のコライダー

    private AnimatorStateInfo stateInfo; // アニメーションの状態

    public GameObject prefabToSpawn1; // 生成するプレハブ1
    public GameObject prefabToSpawn2; // 生成するプレハブ2
    private bool isCooldown1 = false; // プレハブ1のクールタイム管理
    private bool isCooldown2 = false; // プレハブ2のクールタイム管理

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        swordCollider = sword.GetComponent<BoxCollider>(); // 剣のコライダーの値を取得

        audioSource = GetComponent<AudioSource>();
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            if (!isRunning)
            {
                audioSource.Stop();
                isRunning = true;
            }
        }

        if (knockbackVelocity != Vector3.zero)
        {
            var characterController = GetComponent<CharacterController>();
            characterController.Move(knockbackVelocity * Time.deltaTime);
        }

        rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        mainCam.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        transform.localEulerAngles = new Vector3(0, rotationX, 0);

        if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(gameObject.transform.forward * moveSpeed * Time.deltaTime);
            anim.SetBool("Run", true);
            if (isRunning)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("Run", false);
            if (isRunning)
            {
                audioSource.Stop();
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            characterController.Move(gameObject.transform.forward * moveSpeed * Time.deltaTime * -1);
            anim.SetBool("Run", true);
            if (isRunning)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("Run", false);
            if (isRunning)
            {
                audioSource.Stop();
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            characterController.Move(gameObject.transform.right * moveSpeed * Time.deltaTime);
            anim.SetBool("Run", true);
            if (isRunning)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("Run", false);
            if (isRunning)
            {
                audioSource.Stop();
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            characterController.Move(gameObject.transform.right * moveSpeed * Time.deltaTime * -1);
            anim.SetBool("Run", true);
            if (isRunning)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("Run", false);
            if (isRunning)
            {
                audioSource.Stop();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Attack", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("Attack", false);
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isCooldown1) // Qキーが押されたときの処理
        {
            anim.SetBool("Attack", true); // Attackアニメーションを開始
            Instantiate(prefabToSpawn1, transform.position + transform.forward, transform.rotation); // プレハブ1を生成（プレイヤーの回転に合わせる）
            StartCooldown1(); // プレハブ1のクールタイムを開始
        }

        if (Input.GetKeyUp(KeyCode.Q)) // Qキーが離されたときの処理
        {
            anim.SetBool("Attack", false); // Attackアニメーションを停止
        }

        if (Input.GetMouseButtonDown(1) && !isCooldown2) // 右クリックでプレハブ2を生成
        {
            var spawnedPrefab2 = Instantiate(prefabToSpawn2, transform.position, transform.rotation); // プレハブ2を生成
            StartCooldown2(spawnedPrefab2); // プレハブ2のクールタイムを開始し、追跡を行う
        }

        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpPower;
            }
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        stateInfo = anim.GetCurrentAnimatorStateInfo(0); // アニメーションの状態を取得
        if (stateInfo.IsName("Base Layer.NormalAttack01_SwordShield")) // もし、再生しているアニメーションがNormalAttack01_SwordShieldなら、
        {
            swordCollider.enabled = true; // 剣の当たり判定を有効にする
        }
        else // そうでなければ、
        {
            swordCollider.enabled = false; // 剣の当たり判定を無効にする
        }
    }

    private async void StartCooldown1()
    {
        isCooldown1 = true; // クールタイムを開始
        Debug.Log("Cooldown for Prefab 1 started");

        await UniTask.Delay(TimeSpan.FromSeconds(19)); // 19秒間待機

        isCooldown1 = false; // クールタイム終了
        Debug.Log("Cooldown for Prefab 1 ended, you can use it again");
    }

    private async void StartCooldown2(GameObject prefab)
    {
        isCooldown2 = true; // クールタイムを開始
        Debug.Log("Cooldown for Prefab 2 started");

        // 4秒間プレイヤーを追跡
        float elapsedTime = 0f;
        while (elapsedTime < 4f)
        {
            prefab.transform.position = transform.position; // プレハブ2がプレイヤーを追跡
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(); // 次のフレームまで待機
        }

        Debug.Log("Prefab 2 stopped following");

        await UniTask.Delay(TimeSpan.FromSeconds(15)); // 15秒間のクールタイム

        isCooldown2 = false; // クールタイム終了
        Debug.Log("Cooldown for Prefab 2 ended, you can use it again");
    }

    public async void Damage()
    {
        knockbackVelocity = (-transform.forward * 5f);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        knockbackVelocity = Vector3.zero;
    }
}
