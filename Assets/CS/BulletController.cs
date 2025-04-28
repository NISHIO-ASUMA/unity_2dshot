using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{// 弾発射スクリプト

    //***************************
    // 使用変数
    //***************************
    public float MoveSpeed = 10f;       // 弾速
    private Vector3 moveDirection;      // 方向

    const float deleteDistance = 13 * 13;   // 画面外の削除範囲

    GameObject playerObj = null;            // プレイヤーのゲームオブジェクト

    // 外部から呼ばれる初期化関数
    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;

        // 弾の向きに合わせて回転
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // ← 弾が上向きなので-90
    }

    // スタート関数
    void Start()
    {
        // プレイヤーオブジェクトを見つける
        playerObj = GameObject.Find("Player");
    }

    // 更新関数
    void Update()
    {
        // 座標に現在の速度を加算
        transform.position += moveDirection * MoveSpeed * Time.deltaTime;

        // 距離を計算する
        float distance = (playerObj.transform.position - transform.position).sqrMagnitude;

        if (distance > deleteDistance)
        {// 画面外に行ったら
            // 弾を消す
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {// プレイヤーと弾の判定
        if (other.CompareTag("Player") && CompareTag("EnemyBullet"))
        {// プレイヤー かつ 敵の弾だった時
            // プレイヤースクリプトを取得してダメージ処理を呼ぶ
            PlayerController pc = other.GetComponent<PlayerController>();

            if (pc != null)
            {// 取得できたら
                pc.HitDamage(1); // ダメージ数調整
            }

            // 弾を消す
            Destroy(gameObject);
        }
    }
}
