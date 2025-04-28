using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{// 体力バー制御スクリプト

    //*****************************
    // 使用メンバ変数
    //*****************************
    public PlayerController playerController; // PlayerControllerの参照
    public Image lifeBar; // 体力バーのImageを取得

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのコントローラーを取得
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 体力バーの更新
        UpdateLifeBar();
    }

    // 体力バーの更新
    private void UpdateLifeBar()
    {
        if (playerController != null && lifeBar != null)
        {
            // 体力の割合を計算
            float healthPercentage = (float)playerController.currentHealth / playerController.playerLife;

            // 体力バーの進行度を設定
            lifeBar.fillAmount = healthPercentage;
        }
    }
}
