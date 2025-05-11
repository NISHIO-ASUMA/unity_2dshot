using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{// �A�C�e������X�N���v�g

    // �A�C�e���̎��
    public enum ItemType
    {
        ITEMTYPE_0,
        ITEMTYPE_1,
        ITEMTYPE_2,
        ITEMTYPE_3,
        ITEMTYPE_4,
        ITEMTYPE_5
    }

    //*************************************
    // �g�p�����o�ϐ�
    //*************************************
    public float fItemSpeed = 1.0f;         // �A�C�e���̈ړ����x
    public float stopRange = 0.5f;          // �����ɋ߂Â������~����͈�
    private int addScore = 0;               // �X�R�A���Z��

    private bool isMoveLeft = false;        // ���ɍs���t���O
    private bool isMoveRight = false;       // �E�ɍs���t���O
    private bool isStopState = false;       // �ړ��I��������~����t���O
    private int nLife = 1;
    private bool isGetItem = false;

    private Vector3 startPosition;     // �����ʒu
    public ItemType itemType; // ��ނ�ۑ�

    public AudioClip ItemSE; // SE
    private AudioSource AudioSource; // AudioSource���擾

    // Start is called before the first frame update
    void Start()
    {
        // �����ʒu��������
        startPosition = transform.position;

        // �o�������ꏊ����ʂ̉E�������𔻕ʂ���
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 0.5f)
        {
            // ���Ɉړ�
            isMoveLeft = true;
        }
        else
        {
            // �E�Ɉړ�
            isMoveRight = true;
        }

        // �I�[�f�B�I�\�[�X���擾
        AudioSource = GetComponent<AudioSource>(); // AudioSource���擾����

    }

    // Update is called once per frame
    void Update()
    {
        // �o���|�C���g�ɉ������t���O�̏���
        if (isStopState)
        {// ��~��ԂȂ�
            return;
        }

        // �ړ��ʂ�������
        Vector3 moveDir = Vector3.zero;

        if (isMoveLeft)
        {
            // ���Ɉړ�
            moveDir = Vector3.left;
        }
        else if (isMoveRight)
        {
            // �E�Ɉړ�
            moveDir = Vector3.right;
        }

        transform.Translate(moveDir * fItemSpeed * Time.deltaTime);

        // �����ɋ߂Â������~
        if (Mathf.Abs(transform.position.x) <= stopRange)
        {
            // ��~��Ԃ�L����
            isStopState = true;
        }
    }

    // TODO : �����̉��Ƀv���C���[�Ƃ̓����蔻��֐��̒ǉ�
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �̗͂����炷
            nLife--;

            // �I�[�f�B�I�\�[�X���L�� ���� �A�C�e��SE���L��
            if (AudioSource != null && ItemSE != null)
            {
                // �T�E���h�Đ�
                AudioSource.PlayOneShot(ItemSE);
            }

            // �̗͂�0�ȉ� ���� �t���O��false
            if (nLife <= 0)
            {
                if (!isGetItem)
                {
                    switch (gameObject.tag)
                    {
                        // ��ނɉ����ĉ��Z�ʂ����߂�
                        case "Item000":
                            ScoreManager.Instance.AddScore(1000); // 1��ޖ�
                            break;

                        case "Item001":
                            ScoreManager.Instance.AddScore(1000); // 2��ޖ�
                            break;

                        case "Item002":
                            ScoreManager.Instance.AddScore(4000); // 3��ޖ�
                            break;

                        case "Item003":
                            ScoreManager.Instance.AddScore(5000); // 4��ޖ�
                            break;

                        case "Item004":
                            ScoreManager.Instance.AddScore(10000); // 5��ޖ�
                            break;

                        case "Item005":
                            ScoreManager.Instance.AddScore(20000); // 6��ޖ�
                            break;

                        default:
                            ScoreManager.Instance.AddScore(0);
                            break;
                    }

                    // �t���O�L����
                    isGetItem = true;
                }
                // �Ώۂ̃A�C�e��������
                Destroy(gameObject);
            }

        }
    }

    // �^�O�ɉ������X�R�A�̒ǉ�����
    void AddScoreByItemType(ItemType type)
    {

        // �m�F���O
        Debug.Log($"�A�C�e���擾�I�X�R�A +{addScore}");
    }
}
