using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI�g���̂Œǉ�

public class FadeManager : MonoBehaviour
{// �t�F�[�h�V�X�e���X�N���v�g

    // �t�F�[�h�̊Ǘ��t���O�ϐ�
    public static bool isFadeInstance = false;

    bool isFadeIn = false;      // �t�F�[�h�C������t���O
    bool isFadeOut = false;     // �t�F�[�h�A�E�g����t���O

    public float alpha = 0.0f;      // �t�F�[�h�̓��ߗ�
    public float fadeSpeed = 0.2f;  // �t�F�[�h�̃X�s�[�h

    // �O������擾�\�ȃv���p�e�B��ǉ�
    public static bool IsFading { get; private set; } = false;


    // Start is called before the first frame update
    void Start()
    {
        // �N����
        if (!isFadeInstance)
        {// false�Ȃ�

            DontDestroyOnLoad(this);
            isFadeInstance = true;      // �t���O��L��������
        }
        else
        {// �N�����ȊO�͏d�����Ȃ��悤�ɂ���
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {// �t�F�[�h���L��

            // ���ߗ����v�Z����
            alpha -= Time.deltaTime / fadeSpeed;

            if (alpha <= 0.0f)
            {// 0.0f��艺�������
                // �t�F�[�h�C���t���O��false�ɂ���
                isFadeIn = false;

                // �����x��0.0f�ɌŒ�
                alpha = 0.0f;
            }

            // �J���[�𒲐�
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
        else if (isFadeOut)
        {// �t�F�[�h�A�E�g���L��

            // ���ߗ����v�Z����
            alpha += Time.deltaTime / fadeSpeed;

            if (alpha >= 1.0f)
            {//1.0f����������
                // �t�F�[�h�C���t���O��false�ɂ���
                isFadeOut = false;

                // �����x��1.0f�ɌŒ�
                alpha = 1.0f;
            }

            // �J���[�𒲐�
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
    }

    // �t�F�[�h�C���p�̊֐�
    public void FadeIn()
    {
        isFadeIn = true;    // In��true
        isFadeOut = false;  // Out��false
    }

    // �t�F�[�h�A�E�g�p�̊֐�
    public void FadeOut()
    {
        isFadeIn = false;   // In��false
        isFadeOut = true;   // Out��true
    }

}
