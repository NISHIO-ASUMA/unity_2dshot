using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{// ’e”­ŽËƒXƒNƒŠƒvƒg

    public float MoveSpeed = 10f;
    private Vector3 moveDirection;

    public const int deleteFrame = 120;
    const float deleteDistance = 35 * 35;

    GameObject playerObj = null;

    // ŠO•”‚©‚çŒÄ‚Î‚ê‚é‰Šú‰»ŠÖ”
    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;

        // ’e‚ÌŒü‚«‚É‡‚í‚¹‚Ä‰ñ“]
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // © ’e‚ªãŒü‚«‚È‚Ì‚Å-90
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
