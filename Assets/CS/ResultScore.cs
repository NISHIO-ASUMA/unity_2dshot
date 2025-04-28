using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{// リザルトスコア制御スクリプト

    //*************************************
    // 使用メンバ変数
    //*************************************
    public TMP_Text ResultScoreText;              // スコアを表示するTextMeshProのテキスト

    // Start is called before the first frame update
    void Start()
    {
        // ScoreManagerからスコアを取得して表示
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
