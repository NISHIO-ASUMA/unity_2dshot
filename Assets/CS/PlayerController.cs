using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{// �v���C���[����X�N���v�g

    //****************************
    // �v���C���[�̏��
    //****************************
    private enum PlayerState
    {
        Normal,    // �ʏ�
        Damage,    // �_���[�W���󂯂�
        Invincible // ���G
    }

    // �ϐ�
    private PlayerState currentState = PlayerState.Normal;  // �������
    private float invincibleTime = 1f;                      // ���G���ԁi�b�j
    private float invincibleTimer = 0f;                     // ���G�^�C�}�[

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

    //***************************
    // �_�łɊ֘A���郁���o�ϐ�
    //***************************
    private SpriteRenderer spriteRenderer;    // �X�v���C�g�����_���[
    private bool isBlinking = false;          // �_�Œ����ǂ���
    private float blinkTime = 0.2f;           // �_�ŊԊu
    private float blinkDuration = 1f;         // �_�Ŏ��Ԃ̌p������

    public int currentHealth;                 // �̗͌����l

    // �X�^�[�g�֐�

    void Start()
    {
        currentHealth = playerLife; // �Q�[���J�n���ɑ̗͂��ő�ɐݒ�

        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ���擾

        spriteRenderer = GetComponent<SpriteRenderer>(); // �X�v���C�g�����_���[���擾

        bulletPoint = transform.Find("BulletPoint").localPosition;  // �e�̈ʒu�ݒ�

        audioSource = GetComponent<AudioSource>(); // AudioSource���擾����
    }

    void Update()
    {
        // ��ɑ���\�ɂ��邽�߁A��ԂɊւ�炸�ړ�����
        HandleMovement();

        // �e�𔭎˂��鏈���i��ɑ���\�j
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // �e���ˊ֐�
            ShootBullet();
        }

        // ���G��Ԃ̏���
        if (currentState == PlayerState.Invincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                currentState = PlayerState.Normal;  // ���G���Ԃ��I�������ʏ���
            }
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
        if (currentState != PlayerState.Invincible) // ���G��Ԃł̓_���[�W���󂯂Ȃ�
        {
            Debug.Log("�_���[�W�󂯂�: " + damage);  // �_���[�W�l�����O�ɏo��

            // �̗͌���
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                // 0�ȉ��ɂȂ�����
                currentHealth = 0;

                // �f�o�b�O���O���o��
                Debug.Log("�v���C���[���S�I");

                // �V�[���J�ڂŃ��U���g��
                SceneController sceneController = FindObjectOfType<SceneController>();

                if (sceneController != null)
                {
                    sceneController.scenChange(3);
                }
            }
            else
            {
                // �_���[�W���󂯂��疳�G��Ԃɓ���
                currentState = PlayerState.Invincible;
                invincibleTimer = invincibleTime;

                // �_�ŏ������J�n
                StartCoroutine(BlinkEffect());
            }
        }

    }

    // �_�ŏ���
    private IEnumerator BlinkEffect()
    {
        // �t���O��L����
        isBlinking = true;
        float elapsedTime = 0f;

        // �_�Ŏ��Ԓ��J��Ԃ�����
        while (elapsedTime < blinkDuration)
        {
            // �X�v���C�g�̕\��/��\����؂�ւ�
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // ���̓_�ł܂ő҂�
            elapsedTime += blinkTime;
            yield return new WaitForSeconds(blinkTime);
        }

        // �_�ŏI����A�\����Ԃɖ߂�
        spriteRenderer.enabled = true;
        isBlinking = false;
    }

    // �ړ�����
    private void HandleMovement()
    {
        if (playerLife > 0)
        {
            // ���͎擾�i���E�E�㉺�j
            float moveX = 0.0f;
            float moveY = 0.0f;

            // �L�[����
            // TODO : �����Ƀp�b�h�̏\���L�[�ł̑����ǉ�
            if (Input.GetKey(KeyCode.W)) moveY = fSpeed;
            if (Input.GetKey(KeyCode.S)) moveY = -fSpeed;
            if (Input.GetKey(KeyCode.A)) moveX = -fSpeed;
            if (Input.GetKey(KeyCode.D)) moveX = fSpeed;

            // �Q�[���p�b�h�ړ����́i���X�e�B�b�N�j
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

            // Space or B�{�^�����������Ƃ�
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                // BulletPoint��Transform�擾
                Transform bulletPoint = transform.Find("BulletPoint");

                // �e�̐���
                GameObject bullet = Instantiate(BulletObj, bulletPoint.position, bulletPoint.rotation);

                // ������BulletController�ɓ`����
                bullet.GetComponent<BulletController>().SetDirection(bulletPoint.up);

                // SE�Đ�
                if (shotSE != null && audioSource != null)
                {
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

    // �e���ˏ����i��ɑ���\�j
    void ShootBullet()
    {
        // �o���b�g�|�C���g�����o
        Transform bulletPoint = transform.Find("BulletPoint");

        // �e�̐���
        GameObject bullet = Instantiate(BulletObj, bulletPoint.position, bulletPoint.rotation);

        // ������BulletController�ɓ`����
        bullet.GetComponent<BulletController>().SetDirection(bulletPoint.up);

        // SE�Đ�
        if (shotSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(shotSE);
        }
    }

    // �G�ƃv���C���[�̓����蔻��
    void OnTriggerEnter2D(Collider2D other)
    {
        // �G�ɋ��ʂ��Ă镔���̖��O�łƂ�
        if (other.tag.StartsWith("Enemy"))
        {
            // �f�o�b�O���O�\��
            Debug.Log("�G�ɓ�������!");

            // ���ʃ_���[�W����
            HitDamage(1);
        }
    }
}