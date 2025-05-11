using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{// �X�R�A����X�N���v�g

    //*************************************
    // �g�p�����o�ϐ�
    //*************************************
    public static ScoreManager Instance; // �V���O���g��
    public int AllScore = 0;             // ���݂̃X�R�A
    public TMP_Text scoreText;           // �X�R�A�\���pTextMeshPro

    void Awake()
    {
        // �V���O���g����
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // �����X�R�A�\��
        UpdateScoreText();
    }

    public void AddScore(int addValue)
    {
        // �X�R�A���Z
        AllScore += addValue;

        // �\���X�V
        UpdateScoreText();
    }

    // �X�R�A�e�L�X�g�X�V
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = AllScore.ToString();
        }
    }
}
