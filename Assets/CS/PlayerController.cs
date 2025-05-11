using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{// プレイヤー制御スクリプト

    //****************************
    // プレイヤーの状態
    //****************************
    private enum PlayerState
    {
        Normal,    // 通常
        Damage,    // ダメージを受けた
        Invincible // 無敵
    }

    // 変数
    private PlayerState currentState = PlayerState.Normal;  // 初期状態
    private float invincibleTime = 1f;                      // 無敵時間（秒）
    private float invincibleTimer = 0f;                     // 無敵タイマー

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

    //***************************
    // 点滅に関連するメンバ変数
    //***************************
    private SpriteRenderer spriteRenderer;    // スプライトレンダラー
    private bool isBlinking = false;          // 点滅中かどうか
    private float blinkTime = 0.2f;           // 点滅間隔
    private float blinkDuration = 1f;         // 点滅時間の継続時間

    public int currentHealth;                 // 体力減少値

    // スタート関数

    void Start()
    {
        currentHealth = playerLife; // ゲーム開始時に体力を最大に設定

        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D を取得

        spriteRenderer = GetComponent<SpriteRenderer>(); // スプライトレンダラーを取得

        bulletPoint = transform.Find("BulletPoint").localPosition;  // 弾の位置設定

        audioSource = GetComponent<AudioSource>(); // AudioSourceを取得する
    }

    void Update()
    {
        // 常に操作可能にするため、状態に関わらず移動処理
        HandleMovement();

        // 弾を発射する処理（常に操作可能）
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // 弾発射関数
            ShootBullet();
        }

        // 無敵状態の処理
        if (currentState == PlayerState.Invincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                currentState = PlayerState.Normal;  // 無敵時間が終わったら通常状態
            }
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
        if (currentState != PlayerState.Invincible) // 無敵状態ではダメージを受けない
        {
            Debug.Log("ダメージ受けた: " + damage);  // ダメージ値をログに出力

            // 体力減少
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                // 0以下になったら
                currentHealth = 0;

                // デバッグログを出力
                Debug.Log("プレイヤー死亡！");

                // シーン遷移でリザルトへ
                SceneController sceneController = FindObjectOfType<SceneController>();

                if (sceneController != null)
                {
                    sceneController.scenChange(3);
                }
            }
            else
            {
                // ダメージを受けたら無敵状態に入る
                currentState = PlayerState.Invincible;
                invincibleTimer = invincibleTime;

                // 点滅処理を開始
                StartCoroutine(BlinkEffect());
            }
        }

    }

    // 点滅処理
    private IEnumerator BlinkEffect()
    {
        // フラグを有効化
        isBlinking = true;
        float elapsedTime = 0f;

        // 点滅時間中繰り返し処理
        while (elapsedTime < blinkDuration)
        {
            // スプライトの表示/非表示を切り替え
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // 次の点滅まで待つ
            elapsedTime += blinkTime;
            yield return new WaitForSeconds(blinkTime);
        }

        // 点滅終了後、表示状態に戻す
        spriteRenderer.enabled = true;
        isBlinking = false;
    }

    // 移動処理
    private void HandleMovement()
    {
        if (playerLife > 0)
        {
            // 入力取得（左右・上下）
            float moveX = 0.0f;
            float moveY = 0.0f;

            // キー操作
            // TODO : ここにパッドの十字キーでの操作を追加
            if (Input.GetKey(KeyCode.W)) moveY = fSpeed;
            if (Input.GetKey(KeyCode.S)) moveY = -fSpeed;
            if (Input.GetKey(KeyCode.A)) moveX = -fSpeed;
            if (Input.GetKey(KeyCode.D)) moveX = fSpeed;

            // ゲームパッド移動入力（左スティック）
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

            // Space or Bボタンを押したとき
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                // BulletPointのTransform取得
                Transform bulletPoint = transform.Find("BulletPoint");

                // 弾の生成
                GameObject bullet = Instantiate(BulletObj, bulletPoint.position, bulletPoint.rotation);

                // 方向をBulletControllerに伝える
                bullet.GetComponent<BulletController>().SetDirection(bulletPoint.up);

                // SE再生
                if (shotSE != null && audioSource != null)
                {
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

    // 弾発射処理（常に操作可能）
    void ShootBullet()
    {
        // バレットポイントを検出
        Transform bulletPoint = transform.Find("BulletPoint");

        // 弾の生成
        GameObject bullet = Instantiate(BulletObj, bulletPoint.position, bulletPoint.rotation);

        // 方向をBulletControllerに伝える
        bullet.GetComponent<BulletController>().SetDirection(bulletPoint.up);

        // SE再生
        if (shotSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(shotSE);
        }
    }

    // 敵とプレイヤーの当たり判定
    void OnTriggerEnter2D(Collider2D other)
    {
        // 敵に共通してる部分の名前でとる
        if (other.tag.StartsWith("Enemy"))
        {
            // デバッグログ表示
            Debug.Log("敵に当たった!");

            // 共通ダメージ処理
            HitDamage(1);
        }
    }
}