using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingScoreManager : MonoBehaviour
{
    public TMP_Text[] rankingTexts; // TextMeshPro�̃e�L�X�g��5�ݒ�

    private List<int> scoreRanking = new List<int>();

    void Start()
    {
        // �ۑ�����Ă����X�R�A��ǂݍ���
        LoadScores();

        // ����̃X�R�A��ǉ�
        scoreRanking.Add(ScoreManager.Instance.AllScore);

        // �~���Ń\�[�g
        scoreRanking.Sort((a, b) => b.CompareTo(a));

        // ���5���ɍi��
        if (scoreRanking.Count > 5)
        {
            scoreRanking = scoreRanking.GetRange(0, 5);
        }

        // �X�R�A�\��
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (i < scoreRanking.Count)
            {
                // �X�R�A������\��
                rankingTexts[i].text = scoreRanking[i].ToString();
            }
            else
            {
                // �X�R�A�����݂��Ȃ���
                rankingTexts[i].text = "0";
            }
        }

        // �X�R�A��ۑ�
        SaveScores();
    }

    void SaveScores()
    {
        for (int i = 0; i < scoreRanking.Count; i++)
        {
            PlayerPrefs.SetInt("RankingScore" + i, scoreRanking[i]);
        }
        PlayerPrefs.Save();
    }

    void LoadScores()
    {
        scoreRanking.Clear();
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("RankingScore" + i))
            {
                scoreRanking.Add(PlayerPrefs.GetInt("RankingScore" + i));
            }
        }
    }
}
