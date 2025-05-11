using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFlashEX : MonoBehaviour
{// �^�C�g���摜�_�Ő���

    //*********************************
    // �g�p�����o�ϐ�
    //*********************************
    private GameObject titleKeyObject; // �Q�[���I�u�W�F�N�g
    private Coroutine flashCoroutine;  // �_�ŃJ�E���^�[

    public AudioClip KeySE;           // �L�[���͎���SE
    private AudioSource audioSource;  // AudioSource�̎擾�p

    // Start is called before the first frame update
    void Start()
    {
        // "TitleKey"�^�O�̃I�u�W�F�N�g���擾
        titleKeyObject = GameObject.FindWithTag("TitleKey");

        // ��O����
        if (titleKeyObject == null)
        {
            // �f�o�b�O���O
            Debug.LogWarning("TitleKey�^�O�̃I�u�W�F�N�g��������܂���");
        }

        audioSource = GetComponent<AudioSource>(); // AudioSource���擾����

    }

    // Update is called once per frame
    void Update()
    {
        // Enter�L�[�܂��̓Q�[���p�b�h��A�{�^���������ꂽ��_�ŊJ�n
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0)) && titleKeyObject != null)
        {
            if (flashCoroutine == null)
            {
                // �_�ŃJ�E���^�[���X�V����
                flashCoroutine = StartCoroutine(FlashRoutine());
            }

            // SE�Đ�
            if (KeySE != null && audioSource != null)
            {
                // SE�Đ�
                audioSource.PlayOneShot(KeySE);
            }

        }
    }

    // �_�ŏ����̃R���[�`���֐�
    IEnumerator FlashRoutine()
    {
        // �R���|�[�l���g�擾
        Renderer renderer = titleKeyObject.GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.LogWarning("TitleKey�I�u�W�F�N�g��Renderer������܂���");
            yield break;
        }

        while (true)
        {
            // �_�Ŋ��o�̐؂�ւ�
            renderer.enabled = !renderer.enabled;

            yield return new WaitForSeconds(0.03f); // 0.5�b���Ƃɐ؂�ւ�
        }
    }
}
