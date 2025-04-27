using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{// プレイヤー制御スクリプト

    //***************************
    // メンバ変数
    //***************************
    public float fSpeed = 5.0f;    // 移動速度
    public int playerLife = 10;    // 体力

    // 弾のゲームオブジェクト
    public GameObject BulletObj;

    private float currentAngle = 0f; // 現在の角度
    private Rigidbody2D rb;          // Rigidbodyを取得する用の変数
    private Vector2 moveInput;       // 移動量

    Vector3 bulletPoint;             // 弾の位置

    //***************************
    // サウンド関係のメンバ変数
    //***************************
    public AudioClip shotSE;           // 発射時のSE
    private AudioSource audioSource;   // AudioSourceの取得用

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D を取得
        bulletPoint = transform.Find("BulletPoint").localPosition;  // 弾の位置設定

        audioSource = GetComponent<AudioSource>(); // AudioSourceを取得する
    }

    void Update()
    {
        if (playerLife > 0)
        {
            // 入力取得（左右・上下）
            float moveX = 0.0f;
            float moveY = 0.0f;

            if (Input.GetKey(KeyCode.W))
            {
                // Wキーが押された
                moveY = fSpeed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                // Sキー
                moveY = -fSpeed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                // Aキー
                moveX = -fSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                // Dキー
                moveX = fSpeed;
            }

            // ★ ゲームパッド移動入力（左スティック）
            moveX += Input.GetAxis("Horizontal");
            moveY += Input.GetAxis("Vertical");

            // キーボード
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentAngle -= 90f;
                if (currentAngle <= -360f) currentAngle = 0f;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentAngle += 90f;
                if (currentAngle >= 360f) currentAngle = 0f;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }

            // ゲームパッド
            if (Input.GetKeyDown(KeyCode.JoystickButton5)) // RBボタン
            {
                currentAngle -= 90f;
                if (currentAngle <= -360f) currentAngle = 0f;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton4)) // LBボタン
            {
                currentAngle += 90f;
                if (currentAngle >= 360f) currentAngle = 0f;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
            // Space or Aボタンを押したとき
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                // BulletPointのTransform取得
                Transform bulletPoint = transform.Find("BulletPoint");

                // 弾の生成
                GameObject bullet = Instantiate(BulletObj, bulletPoint.position, bulletPoint.rotation);

                // 方向をBulletControllerに伝える
                bullet.GetComponent<BulletController>().SetDirection(bulletPoint.up);

                // SE再生
                if (shotSE != null && audioSource != null)
                {// 弾発射SEがある かつ オーディオソースが取得できていたら

                    // SE再生
                    audioSource.PlayOneShot(shotSE);
                }
            }

            // 最終的な移動量
            moveInput = new Vector2(moveX, moveY).normalized;
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            Vector2 newPosition = rb.position + moveInput * fSpeed * Time.fixedDeltaTime;

            // 背景の黄色い枠の範囲に合わせて調整
            float minX = -1.3f; // 左端
            float maxX = 1.3f; // 右端
            float minY = -2.5f; // 下端
            float maxY = 3.0f; // 上端

            // ワールド座標で制限
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            rb.MovePosition(newPosition);
        }
    }


    public void HitDamage(int damage)
    {
        // ダメージの数値だけ体力を減らす
        playerLife -= damage;

        if (playerLife <= 0)
        {// 体力が0以下
            Debug.Log("プレイヤー死亡！");

            // シーン遷移でリザルトへ
            SceneController sceneController = FindObjectOfType<SceneController>();

            if (sceneController != null)
            {
                // シーンのリザルト番号を設定し,画面遷移
                sceneController.scenChange(3);
            }

        }
    }
}