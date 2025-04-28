using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{// �e���˃X�N���v�g

    //***************************
    // �g�p�ϐ�
    //***************************
    public float MoveSpeed = 10f;       // �e��
    private Vector3 moveDirection;      // ����

    const float deleteDistance = 13 * 13;   // ��ʊO�̍폜�͈�

    GameObject playerObj = null;            // �v���C���[�̃Q�[���I�u�W�F�N�g

    // �O������Ă΂�鏉�����֐�
    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;

        // �e�̌����ɍ��킹�ĉ�]
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // �� �e��������Ȃ̂�-90
    }

    // �X�^�[�g�֐�
    void Start()
    {
        // �v���C���[�I�u�W�F�N�g��������
        playerObj = GameObject.Find("Player");
    }

    // �X�V�֐�
    void Update()
    {
        // ���W�Ɍ��݂̑��x�����Z
        transform.position += moveDirection * MoveSpeed * Time.deltaTime;

        // �������v�Z����
        float distance = (playerObj.transform.position - transform.position).sqrMagnitude;

        if (distance > deleteDistance)
        {// ��ʊO�ɍs������
            // �e������
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {// �v���C���[�ƒe�̔���
        if (other.CompareTag("Player") && CompareTag("EnemyBullet"))
        {// �v���C���[ ���� �G�̒e��������
            // �v���C���[�X�N���v�g���擾���ă_���[�W�������Ă�
            PlayerController pc = other.GetComponent<PlayerController>();

            if (pc != null)
            {// �擾�ł�����
                pc.HitDamage(1); // �_���[�W������
            }

            // �e������
            Destroy(gameObject);
        }
    }
}
