using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{// ���U���g�X�R�A����X�N���v�g

    //*************************************
    // �g�p�����o�ϐ�
    //*************************************
    public TMP_Text ResultScoreText;              // �X�R�A��\������TextMeshPro�̃e�L�X�g

    // Start is called before the first frame update
    void Start()
    {
        // ScoreManager����X�R�A���擾���ĕ\��
        if (ResultScoreText != null)
        {
            ResultScoreText.text = ScoreManager.Instance.AllScore.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
