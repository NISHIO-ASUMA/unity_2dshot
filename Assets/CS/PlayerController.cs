using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{// �v���C���[����X�N���v�g

    public float fSpeed = 5.0f;    // �ړ����x
    public int playerLife = 10;    // �̗�

    // �e�̃Q�[���I�u�W�F�N�g
    public GameObject BulletObj;     

    private float currentAngle = 0f; // ���݂̊p�x
    private Rigidbody2D rb;          // Rigidbody���擾����p�̕ϐ�
    private Vector2 moveInput;       // �ړ���

    Vector3 bulletPoint;             // �e�̈ʒu

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ���擾
        bulletPoint = transform.Find("BulletPoint").localPosition;  // �e�̈ʒu�ݒ�
    }

    void Update()
    {
        if (playerLife > 0)
        {
            // ���͎擾�i���E�E�㉺�j
            float moveX = 0.0f;
            float moveY = 0.0f;

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

            // �{�^�����������Ƃ�
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // BulletPoint��Transform�擾
                Transform bulletPoint = transform.Find("BulletPoint");

                // �e�̐���
                GameObject bullet = Instantiate(BulletObj, bulletPoint.position, bulletPoint.rotation);

                // ������BulletController�ɓ`����
                bullet.GetComponent<BulletController>().SetDirection(bulletPoint.up);
            }

            // �ŏI�I�Ȉړ���
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

            // �w�i�̉��F���g�͈̔͂ɍ��킹�Ē���
            float minX = -1.3f; // ���[
            float maxX = 1.3f; // �E�[
            float minY = -2.5f; // ���[
            float maxY = 3.0f; // ��[

            // ���[���h���W�Ő���
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            rb.MovePosition(newPosition);
        }
    }
}