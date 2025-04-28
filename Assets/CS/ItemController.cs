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
    public float stopRange = 0.5f;           // �����ɋ߂Â������~����͈�
    public int addScore = 0;                // �X�R�A���Z��

    private bool isMoveLeft = false;        // ���ɍs���t���O
    private bool isMoveRight = false;       // �E�ɍs���t���O
    private bool isStopState = false;       // �ړ��I��������~����t���O

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
            // �I�[�f�B�I�\�[�X���L�� ���� �A�C�e��SE���L��
            if (AudioSource != null && ItemSE != null)
            {
                // �T�E���h�Đ�
                AudioSource.PlayOneShot(ItemSE);
            }

            // �X�R�A���Z
            AddScoreByItemType(itemType);

            // �Ώۂ̃A�C�e��������
            Destroy(gameObject);
        }
    }

    // ��ނɉ������X�R�A�̒ǉ�����
    void AddScoreByItemType(ItemType type)
    {
        int AddScore = 0;

        switch (type)
        {

            case ItemType.ITEMTYPE_0:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_1:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_2:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_3:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_4:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_5:
                AddScore += addScore;
                break;
        }

        // �m�F���O
        Debug.Log($"�A�C�e���擾�I�X�R�A +{addScore}");

        // �����ŃQ�[���S�̂̃X�R�A�ɉ��Z����֐����Ăяo���Ƃ��ł���
        ScoreManager.Instance.AddScore(addScore);
    }
}
