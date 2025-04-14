using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{// �v���C���[����X�N���v�g

    public float fSpeed = 5.0f;   // �ړ����x
    public int playerLife = 10;   // �̗�

    private Rigidbody2D rb;       // Rigidbody���擾����p�̕ϐ�
    private Vector2 moveInput;    // �ړ���

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ���擾
    }

    void Update()
    {
        if (playerLife > 0)
        {
            // ���͎擾�i���E�E�㉺�j
            float moveX = 0f;
            float moveY = 0f;

            if (Input.GetKey(KeyCode.W))
            {
                // W�L�[�������ꂽ
                moveY = fSpeed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                // S�L�[
                moveY = -fSpeed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                // A�L�[
                moveX = -fSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                // D�L�[
                moveX = fSpeed;
            }

            // �ŏI�I�Ȉړ���
            moveInput = new Vector2(moveX, moveY).normalized;
        }
        else
        {// player�̃��C�t��0�ȉ�
            moveInput = Vector2.zero; // ���S���͒�~
        }
    }

    void FixedUpdate()
    {
        // Rigidbody2D�̈ړ���FixedUpdate���ŏ���
        rb.velocity = moveInput * fSpeed;
    }
}