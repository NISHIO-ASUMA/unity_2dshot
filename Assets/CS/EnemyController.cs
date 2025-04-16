using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{// �G����X�N���v�g

    // �g�p�ϐ�
    public int EnemyLife = 0;       // �G�̗̑�
    public float fEnemyMove = 0f;   // �G�̈ړ���
    public int nAttack = 0;         // �G�̍U����
    public float moveRange = 3.0f;  // �G�̉�������

    private Vector3 startPosition;     // �����ʒu
    private Vector3 moveDirection;     // �ړ�����

    //***********************
    // �G�̓���t���O�֌W
    //***********************
    private bool isBouncing = false;   // ���ǔ��˂̃t���O
    private bool isCollisionUp = false;// ��ǔ��˂̃t���O

    //**********************************************
    // ��ʓ��̈��̉ӏ������[�v�ړ�����t���O�֌W
    //**********************************************
    private List<Vector3> patrolPoints = new List<Vector3>(); // ����|�C���g
    private int currentPointIndex = 0;                        // ���݂̖ڕW�|�C���g

    //*********************************************
    // �G����ˏo�����o���b�g
    //*********************************************
    public GameObject bulletPrefab;     // �o���b�g�I�u�W�F�N�g���擾
    public float fireInterval = 1.0f;   // �C���^�[�o��
    private float fireTimer = 0f;

    //********************************************
    // �ǔ��p�̕ϐ�
    //********************************************
    public GameObject Player;      // �v���C���[�I�u�W�F�N�g�iInspector�Őݒ�j
    public float chaseSpeed = 2f;  // �ǔ��X�s�[�h

    // Start is called before the first frame update
    void Start()
    {
        // �����ʒu��������
        startPosition = transform.position;

        // �^�O�ɂ���ď����̈ړ�������ݒ�
        if (CompareTag("Enemy01"))
        {
            moveDirection = Vector3.right; // �����͉E����
            isBouncing = true;             // �t���O��L��������
        }
        else if (CompareTag("Enemy02"))
        {
            moveDirection = Vector3.up;    // �����͏����
            isCollisionUp = true;          // �t���O��L��������
        }
        else if (CompareTag("Enemy03"))
        {
            // �r���[�|�[�g��4�_�̃p�g���[���|�C���g��ݒ�
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.83f, 10))); // ����
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.9f, 0.83f, 10))); // �E��
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.9f, 0.1f, 10))); // �E��
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.1f, 10))); // ����

            transform.position = patrolPoints[0]; // �ŏ��̈ʒu�ɃZ�b�g
            currentPointIndex = 1; // ���Ɍ������n�_���Z�b�g
        }
        else
        {
            // �ړ����Ȃ�
            moveDirection = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �^�O���Ƃɉ������G�̈ړ�����
        if (CompareTag("Enemy01") && isBouncing)
        {// �^�O��Enemy01 ���� ���[�v���I���Ȃ�
            // �ړ�
            transform.Translate(moveDirection * fEnemyMove * Time.deltaTime);

            // �r���[�|�[�g���W�ɕϊ�
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

            // ���E�̒[�ɂԂ�����������𔽓]
            if (viewPos.x < 0.1f || viewPos.x > 0.9f)
            {
                moveDirection *= -1; // �������]
            }
        }
        else if (CompareTag("Enemy02") && isCollisionUp)
        {// �^�O��Enemy02 ���� ���[�v���I���Ȃ�
            // �ړ�
            transform.Translate(moveDirection * fEnemyMove * Time.deltaTime);

            // �r���[�|�[�g���W�ɕϊ�
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

            // �㉺�̒[�ɂԂ�����������𔽓]
            if (viewPos.y < 0.1f || viewPos.y > 0.9f)
            {
                moveDirection *= -1; // �������]
            }
        }
        else if (CompareTag("Enemy03") && patrolPoints.Count > 0)
        {// �^�O��Enemy03 ���� �J�E���g��0�ȏ�
            // ���݂̖ړI�n���擾
            Vector3 target = patrolPoints[currentPointIndex];

            // �ړ����� ( �����𐳋K�����Ĉ�葬�x�ňړ� �j
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * fEnemyMove * Time.deltaTime;

            // �ڕW�|�C���g�ɋ߂Â����玟�̃|�C���g��
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                // �|�C���g�̃C���f�b�N�X�ԍ����X�V
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            }
        }
        else if(CompareTag("Enemy04"))
        {// �^�O��Enemy04��������

            // �^�C�}�[�����Z
            fireTimer += Time.deltaTime;

            // �C���^�[�o���𒴂�����
            if (fireTimer >= fireInterval)
            {
                // �^�C�}�[�������l�ɂ���
                fireTimer = 0f;

                // �r���[�|�[�g���猻�݂̍��W���擾
                Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
                Vector3 bulletDir = Vector3.down;

                // ���݂̔z�u����Ă���ꏊ����e�𔭎�
                if (viewPos.x < 0.5f && viewPos.y >= 0.5f) bulletDir = new Vector3(1, -1, 0);
                else if (viewPos.x >= 0.5f && viewPos.y >= 0.5f) bulletDir = new Vector3(-1, -1, 0);
                else if (viewPos.x < 0.5f && viewPos.y < 0.5f) bulletDir = new Vector3(1, 1, 0);
                else if (viewPos.x >= 0.5f && viewPos.y < 0.5f) bulletDir = new Vector3(-1, 1, 0);

                // �e�̃I�u�W�F�N�g�𐶐�
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // BulletController.cs����R���|�[�l���g���擾
                BulletController bc = bullet.GetComponent<BulletController>();

                if (bc != null)
                {// NULL����Ȃ�������
                    bc.SetDirection(bulletDir);
                }
            }
        }
        else if (CompareTag("Enemy05") && Player != null)
        {
            // �v���C���[�̈ʒu���擾
            Vector3 targetPos = Player.transform.position;

            // Z���W�𖳎�����X��Y������ǂ�������
            targetPos.z = transform.position.z;

            // �����x�N�g�����v�Z���Ĉړ�
            Vector3 direction = (targetPos - transform.position).normalized;
            transform.position += direction * chaseSpeed * Time.deltaTime;
        }
        else
        {// ����ȊO�̎�
            float pingPong = Mathf.PingPong(Time.time * fEnemyMove, moveRange);
            transform.position = startPosition + moveDirection * pingPong;
        }

        Debug.Log("Player Position: " + Player.transform.position);
    }
}
