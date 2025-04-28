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
    public TMP_Text scoreText;           // テキストメッシュで作成


    void Awake()
    {
        // NULLだったら
        if (Instance == null)
        {// シングルトン化する
            Instance = this;
        }
        else
        {
            // オブジェクトを消去
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // スコアUIを更新
        UpdateScoreText();
    }


    public void AddScore(int addValue)
    {
        // スコア加算
        AllScore += addValue;

        // テキスト更新
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            // スコアテキストに変更を加える
            scoreText.text = "" + AllScore.ToString();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
