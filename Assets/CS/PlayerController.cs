using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{// プレイヤー制御スクリプト

    public float fSpeed = 5.0f;   // 移動速度
    public int playerLife = 10;   // 体力

    private Rigidbody2D rb;       // Rigidbodyを取得する用の変数
    private Vector2 moveInput;    // 移動量

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D を取得
    }

    void Update()
    {
        if (playerLife > 0)
        {
            // 入力取得（左右・上下）
            float moveX = 0f;
            float moveY = 0f;

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

            // 最終的な移動量
            moveInput = new Vector2(moveX, moveY).normalized;
        }
        else
        {// playerのライフが0以下
            moveInput = Vector2.zero; // 死亡時は停止
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2Dの移動はFixedUpdate内で処理
        rb.velocity = moveInput * fSpeed;
    }
}