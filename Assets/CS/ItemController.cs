using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{// アイテム制御スクリプト

    //*************************************
    // 使用メンバ変数
    //*************************************
    public float fItemSpeed = 1.0f;         // アイテムの移動速度
    public int nItemLife = 1;               // アイテムの体力

    private bool isMoveLeft = false;        // 左に行くフラグ
    private bool isMoveRight = false;       // 右に行くフラグ
    private bool isStopState = false;       // 移動終わったら停止するフラグ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 出現ポイントに応じたフラグの処理
    }

    // TODO : ここの下にプレイヤーとの当たり判定関数の追加

}
