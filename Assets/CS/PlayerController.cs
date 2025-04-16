using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{// プレイヤー制御スクリプト

    public float fSpeed = 5.0f;    // 移動速度
    public int playerLife = 10;    // 体力

    // 弾のゲームオブジェクト
    public GameObject BulletObj;     

    private float currentAngle = 0f; // 現在の角度
    private Rigidbody2D rb;          // Rigidbodyを取得する用の変数
    private Vector2 moveInput;       // 移動量

    Vector3 bulletPoint;             // 弾の位置

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D を取得
        bulletPoint = transform.Find("BulletPoint").localPosition;  // 弾の位置設定
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

            // ボタンを押したとき
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // BulletPointのTransform取得
                Transform bulletPoint = transform.Find("BulletPoint");

                // 弾の生成
                GameObject bullet = Instantiate(BulletObj, bulletPoint.position, bulletPoint.rotation);

                // 方向をBulletControllerに伝える
                bullet.GetComponent<BulletController>().SetDirection(bulletPoint.up);
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
}