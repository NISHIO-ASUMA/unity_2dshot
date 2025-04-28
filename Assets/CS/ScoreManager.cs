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
    public TMP_Text scoreText;           // �e�L�X�g���b�V���ō쐬


    void Awake()
    {
        // NULL��������
        if (Instance == null)
        {// �V���O���g��������
            Instance = this;
        }
        else
        {
            // �I�u�W�F�N�g������
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // �X�R�AUI���X�V
        UpdateScoreText();
    }


    public void AddScore(int addValue)
    {
        // �X�R�A���Z
        AllScore += addValue;

        // �e�L�X�g�X�V
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            // �X�R�A�e�L�X�g�ɕύX��������
            scoreText.text = "" + AllScore.ToString();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
