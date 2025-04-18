using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{// 弾発射スクリプト

    public float MoveSpeed = 10f;
    private Vector3 moveDirection;

    public const int deleteFrame = 120;
    const float deleteDistance = 35 * 35;

    GameObject playerObj = null;

    // 外部から呼ばれる初期化関数
    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;

        // 弾の向きに合わせて回転
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // ← 弾が上向きなので-90
    }

    void Start()
    {
        playerObj = GameObject.Find("Player");
    }

    void Update()
    {
        transform.position += moveDirection * MoveSpeed * Time.deltaTime;

        float distance = (playerObj.transform.position - transform.position).sqrMagnitude;
        if (distance > deleteDistance)
        {
            Destroy(gameObject);
        }
    }

}
