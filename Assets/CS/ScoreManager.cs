using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{// スコア制御スクリプト

    //*************************************
    // 使用メンバ変数
    //*************************************
    public static ScoreManager Instance; // シングルトン
    public int AllScore = 0;             // 現在のスコア
    public TMP_Text scoreText;           // スコア表示用TextMeshPro

    void Awake()
    {
        // シングルトン化
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
        // 初期スコア表示
        UpdateScoreText();
    }

    public void AddScore(int addValue)
    {
        // スコア加算
        AllScore += addValue;

        // 表示更新
        UpdateScoreText();
    }

    // スコアテキスト更新
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = AllScore.ToString();
        }
    }
}
