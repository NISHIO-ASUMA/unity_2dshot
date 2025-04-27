using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{// �v���C���[����X�N���v�g

    //***************************
    // �����o�ϐ�
    //***************************
    public float fSpeed = 5.0f;    // �ړ����x
    public int playerLife = 10;    // �̗�

    // �e�̃Q�[���I�u�W�F�N�g
    public GameObject BulletObj;

    private float currentAngle = 0f; // ���݂̊p�x
    private Rigidbody2D rb;          // Rigidbody���擾����p�̕ϐ�
    private Vector2 moveInput;       // �ړ���

    Vector3 bulletPoint;             // �e�̈ʒu

    //***************************
    // �T�E���h�֌W�̃����o�ϐ�
    //***************************
    public AudioClip shotSE;           // ���ˎ���SE
    private AudioSource audioSource;   // AudioSource�̎擾�p

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ���擾
        bulletPoint = transform.Find("BulletPoint").localPosition;  // �e�̈ʒu�ݒ�

        audioSource = GetComponent<AudioSource>(); // AudioSource���擾����
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

            // �� �Q�[���p�b�h�ړ����́i���X�e�B�b�N�j
            moveX += Input.GetAxis("Horizontal");
            moveY += Input.GetAxis("Vertical");

            // �L�[�{�[�h
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

            // �Q�[���p�b�h
            if (Input.GetKeyDown(KeyCode.JoystickButton5)) // RB�{�^��
            {
                currentAngle -= 90f;
                if (currentAngle <= -360f) currentAngle = 0f;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton4)) // LB�{�^��
            {
                currentAngle += 90f;
                if (currentAngle >= 360f) currentAngle = 0f;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
            // Space or A�{�^�����������Ƃ�
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                // BulletPoint��Transform�擾
                Transform bulletPoint = transform.Find("BulletPoint");

                // �e�̐���
                GameObject bullet = Instantiate(BulletObj, bulletPoint.position, bulletPoint.rotation);

                // ������BulletController�ɓ`����
                bullet.GetComponent<BulletController>().SetDirection(bulletPoint.up);

                // SE�Đ�
                if (shotSE != null && audioSource != null)
                {// �e����SE������ ���� �I�[�f�B�I�\�[�X���擾�ł��Ă�����

                    // SE�Đ�
                    audioSource.PlayOneShot(shotSE);
                }
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


    public void HitDamage(int damage)
    {
        // �_���[�W�̐��l�����̗͂����炷
        playerLife -= damage;

        if (playerLife <= 0)
        {// �̗͂�0�ȉ�
            Debug.Log("�v���C���[���S�I");

            // �V�[���J�ڂŃ��U���g��
            SceneController sceneController = FindObjectOfType<SceneController>();

            if (sceneController != null)
            {
                // �V�[���̃��U���g�ԍ���ݒ肵,��ʑJ��
                sceneController.scenChange(3);
            }

        }
    }
}