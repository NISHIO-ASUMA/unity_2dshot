using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{// �G����X�N���v�g

    // �g�p�ϐ�
    public int EnemyLife = 0;       // �G�̗̑�
    public float fEnemyMove = 0f;   // �G�̈ړ���
    public int nAttack = 0;         // �G�̍U����
    public float moveRange = 3.0f;  // �G�̉�������

    private Vector3 startPosition;     // �����ʒu
    private Vector3 moveDirection;     // �ړ�����

    //*********************************
    // �G1�Ŏg�p
    //*********************************
    private float fireTimer_1 = 0f;
    public float fireInterval_1 = 1.0f; // Enemy01�p�̔��ˊԊu

    //***********************
    // �G2�̓���t���O�֌W
    //***********************
    private bool isBouncing = false;   // ���ǔ��˂̃t���O
    private bool isCollisionUp = false;// ��ǔ��˂̃t���O
    private float fireTimer_2 = 0f;
    public float fireInterval_2 = 1.0f; // Enemy02�p�̔��ˊԊu

    //*********************************************
    // �G����ˏo�����o���b�g
    //*********************************************
    public GameObject bulletPrefab;     // �o���b�g�I�u�W�F�N�g���擾
    public float fireInterval = 1.0f;   // �C���^�[�o��
    private float fireTimer = 0f;

    //*********************************************
    // ���v��菄��
    //*********************************************
    private int nPointIdx = 0;
    private List<Vector3> Enemy03Point = new List<Vector3>(); // ����|�C���g

    //********************************************
    // �ǔ��p�̕ϐ�
    //********************************************
    public GameObject Player;      // �v���C���[�I�u�W�F�N�g�iInspector�Őݒ�j
    public float chaseSpeed = 2f;  // �ǔ��X�s�[�h

    Transform playerTr;             // �v���C���[��Transform

    private List<Vector3> patrolPoints = new List<Vector3>(); // ����|�C���g
    private int currentPointIndex = 0;                        // ���݂̖ڕW�|�C���g

    //********************************************
    // ���[�v�|�C���g�p�̕ϐ�
    //********************************************
    public float warpInterval = 3.0f; // ���[�v�Ԋu
    private float warpTimer = 0f;     // ���[�v�^�C�}�[

    //********************************************
    // �p�[�e�B�N������
    //********************************************
    public GameObject hitParticlePrefab;     // 2D�p�[�e�B�N����Prefab
    private object rigidBody;

    //*********************************************
    // �A�C�e���v���n�u
    //*********************************************
    public GameObject ItemPrefab; // �A�C�e���̃v���n�u

    private bool GetScore = false;

    // Start is called before the first frame update
    void Start()
    {
        // �����ʒu��������
        startPosition = transform.position;

        // �^�O�ɂ���ď����̈ړ�������ݒ�
        if (CompareTag("Enemy01"))
        {// �^�O��Enemy01
            moveDirection = Vector3.right; // �����͉E����
            isBouncing = true;             // �t���O��L��������
        }
        else if (CompareTag("Enemy02"))
        {// �^�O��Enemy02
            moveDirection = Vector3.up;    // �����͏����
            isCollisionUp = true;          // �t���O��L��������
        }
        else if (CompareTag("Enemy03"))
        {// �^�O��Enemy03

            // ���v���ɏ��񂷂�4�_�i���と�E�と�E���������j
            Enemy03Point.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.2f, 0.8f, 10))); // ����
            Enemy03Point.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0.8f, 10))); // �E��
            Enemy03Point.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0.2f, 10))); // �E��
            Enemy03Point.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.2f, 0.2f, 10))); // ����

            transform.position = Enemy03Point[0]; // �����ʒu�������
            nPointIdx = 1; // ���Ɍ������͉̂E��
        }
        else if (CompareTag("Enemy05"))
        {// �^�O��Enemy05            
            // �v���C���[��Transform���擾�i�v���C���[�̃^�O��Player�ɐݒ�K�v�j
            playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        }
        else if (CompareTag("Enemy06"))
        {// �^�O��Enemy06
            // ���E�E�E������3�_�����[���h���W�Őݒ�iViewport�x�[�X�j
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.5f, 10))); // ��
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.9f, 0.5f, 10))); // �E
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10))); // ����

            transform.position = patrolPoints[0]; // �ŏ��͍�
            currentPointIndex = 1; // ���Ɍ������͉̂E
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

            // ======= �e���ˏ������^�C�}�[���䂷�� =======
            fireTimer_1 += Time.deltaTime; // �^�C�}�[�i�߂�

            if (fireTimer_1 >= fireInterval_1)
            {
                fireTimer_1 = 0f; // �^�C�}�[���Z�b�g

                // �e�̕��������߂�
                Vector3 bulletDir = Vector3.down;

                if (viewPos.y < 0.5f)
                {
                    // �e����ɔ���
                    bulletDir = Vector3.up;
                }
                else
                {
                    // �e�����ɔ���
                    bulletDir = Vector3.down;
                }

                // �e�̃I�u�W�F�N�g�𐶐�
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // BulletController.cs����R���|�[�l���g���擾
                BulletController bc = bullet.GetComponent<BulletController>();

                if (bc != null)
                {
                    bc.SetDirection(bulletDir);
                }
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

            // ======= �e���ˏ������^�C�}�[���䂷�� =======
            fireTimer_2 += Time.deltaTime; // �^�C�}�[�i�߂�

            if (fireTimer_2 >= fireInterval_2)
            {
                fireTimer_2 = 0f; // �^�C�}�[���Z�b�g

                // �e�̕��������߂�
                Vector3 bulletDir = Vector3.left;

                if (viewPos.x < 0.5f)
                {
                    // �e���E�ɔ���
                    bulletDir = Vector3.right;
                }
                else
                {
                    // �e�����ɔ���
                    bulletDir = Vector3.left;
                }

                // �e�̃I�u�W�F�N�g�𐶐�
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // BulletController.cs����R���|�[�l���g���擾
                BulletController bc = bullet.GetComponent<BulletController>();

                if (bc != null)
                {
                    bc.SetDirection(bulletDir);
                }

            }
        }
        else if (CompareTag("Enemy03"))
        {// �^�O��Enemy03

            // �|�C���g����
            Vector3 targetPoint = Enemy03Point[nPointIdx];

            transform.position = Vector3.MoveTowards(transform.position, targetPoint, fEnemyMove * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                nPointIdx = (nPointIdx + 1) % Enemy03Point.Count;
            }

        }
        else if (CompareTag("Enemy04"))
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
            // �v���C���[�Ƃ̋�����0.1f�����ɂȂ����炻��ȏ���s���Ȃ�
            if (Vector2.Distance(transform.position, playerTr.position) < 0.1f)
                return;

            // �v���C���[�Ɍ����Đi��
            transform.position = Vector2.MoveTowards
                (
                transform.position,
                new Vector2(playerTr.position.x, playerTr.position.y),
                chaseSpeed * Time.deltaTime);
        }
        else if (CompareTag("Enemy06") && patrolPoints.Count >= 3)
        {// �^�O��Enemy06 ���� �|�C���g�̃J�E���g��3���傫���Ȃ�����
         // �^�C�}�[�����Z
            warpTimer += Time.deltaTime;

            if (warpTimer >= warpInterval)
            {// �C���^�[�o�����傫���Ȃ�����
             // �����l�ɖ߂�
                warpTimer = 0f;

                // ���[�v���s
                transform.position = patrolPoints[currentPointIndex];

                // ���̃|�C���g�ɐ؂�ւ� ( ����̂R�_�Ɉړ����� )
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            }
        }
        else
        {// ����ȊO�̎�
            float pingPong = Mathf.PingPong(Time.time * fEnemyMove, moveRange);
            transform.position = startPosition + moveDirection * pingPong;
        }

     }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�̒e�^�O�ɓ���������
        if (collision.CompareTag("PlayerBullet"))
        {
            // �G�̗̑͂����炷
            EnemyLife--;

            // �e������
            Destroy(collision.gameObject);

            // �̗͂�0�ȉ��Ȃ玩�g���폜
            if (EnemyLife <= 0)
            {
                // �A�C�e���𐶐�����
                if (ItemPrefab != null)
                {
                    // �G��|�����ʒu�ɏo������
                    Instantiate(ItemPrefab, transform.position, Quaternion.identity);
                }

                if (!GetScore)
                {
                    // �G�̃^�O�ɉ����ăX�R�A�����Z����
                    switch (gameObject.tag)
                    {
                        case "Enemy01":
                            ScoreManager.Instance.AddScore(10000); // Enemy01
                            break;

                        case "Enemy02":
                            ScoreManager.Instance.AddScore(20000); // Enemy02
                            break;

                        case "Enemy03":
                            ScoreManager.Instance.AddScore(30000); // Enemy03
                            break;

                        case "Enemy04":
                            ScoreManager.Instance.AddScore(40000); // Enemy04
                            break;

                        case "Enemy05":
                            ScoreManager.Instance.AddScore(50000); // Enemy05
                            break;

                        case "Enemy06":
                            ScoreManager.Instance.AddScore(50000); // Enemy06
                            break;

                        default:
                            // �΍��0���
                            ScoreManager.Instance.AddScore(0); // �����l
                            break;
                    }

                    // �t���O�𗧂ĂāA�X�R�A���Z��1�񂾂����s
                    GetScore = true;
                }

                // �G������
                Destroy(gameObject);
            }
            else
            {
                // �p�[�e�B�N���𐶐�
                if (hitParticlePrefab != null)
                {
                    // �Փˈʒu�Ƀp�[�e�B�N���𐶐�
                    GameObject hitParticle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);

                    // �p�[�e�B�N����0.5�b��ɏ���
                    Destroy(hitParticle, 0.5f);
                }
            }

        }
    }
}
