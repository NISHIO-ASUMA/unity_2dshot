using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{// ���y�Đ��X�N���v�g

    // ���y�̃����o�ϐ�
    private AudioSource audioSource;    // �I�[�f�B�I�\�[�X��ǉ�����

    // Start is called before the first frame update
    void Start()
    {
        // �I�[�f�B�I�\�[�X���擾
        audioSource = GetComponent<AudioSource>();

        // �����ł�����
        if (audioSource != null)
        {
            audioSource.Play();  // BGM�̍Đ����J�n
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
