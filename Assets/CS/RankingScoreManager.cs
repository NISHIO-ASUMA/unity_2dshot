using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingScoreManager : MonoBehaviour
{
    public TMP_Text[] rankingTexts; // TextMeshProのテキストを5つ設定

    private List<int> scoreRanking = new List<int>();

    void Start()
    {
        // 保存されていたスコアを読み込む
        LoadScores();

        // 今回のスコアを追加
        scoreRanking.Add(ScoreManager.Instance.AllScore);

        // 降順でソート
        scoreRanking.Sort((a, b) => b.CompareTo(a));

        // 上位5件に絞る
        if (scoreRanking.Count > 5)
        {
            scoreRanking = scoreRanking.GetRange(0, 5);
        }

        // スコア表示
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (i < scoreRanking.Count)
            {
                // スコアだけを表示
                rankingTexts[i].text = scoreRanking[i].ToString();
            }
            else
            {
                // スコアが存在しないと
                rankingTexts[i].text = "0";
            }
        }

        // スコアを保存
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
