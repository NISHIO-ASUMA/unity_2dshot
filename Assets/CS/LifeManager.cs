using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{// �̗̓o�[����X�N���v�g

    //*****************************
    // �g�p�����o�ϐ�
    //*****************************
    public PlayerController playerController; // PlayerController�̎Q��
    public Image lifeBar; // �̗̓o�[��Image���擾

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�̃R���g���[���[���擾
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // �̗̓o�[�̍X�V
        UpdateLifeBar();
    }

    // �̗̓o�[�̍X�V
    private void UpdateLifeBar()
    {
        if (playerController != null && lifeBar != null)
        {
            // �̗͂̊������v�Z
            float healthPercentage = (float)playerController.currentHealth / playerController.playerLife;

            // �̗̓o�[�̐i�s�x��ݒ�
            lifeBar.fillAmount = healthPercentage;
        }
    }
}
