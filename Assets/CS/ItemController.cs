using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{// アイテム制御スクリプト

    // アイテムの種類
    public enum ItemType
    {
        ITEMTYPE_0,
        ITEMTYPE_1,
        ITEMTYPE_2,
        ITEMTYPE_3,
        ITEMTYPE_4,
        ITEMTYPE_5
    }

    //*************************************
    // 使用メンバ変数
    //*************************************
    public float fItemSpeed = 1.0f;         // アイテムの移動速度
    public float stopRange = 0.5f;           // 中央に近づいたら停止する範囲
    public int addScore = 0;                // スコア加算量

    private bool isMoveLeft = false;        // 左に行くフラグ
    private bool isMoveRight = false;       // 右に行くフラグ
    private bool isStopState = false;       // 移動終わったら停止するフラグ

    private Vector3 startPosition;     // 初期位置
    public ItemType itemType; // 種類を保存

    public AudioClip ItemSE; // SE
    private AudioSource AudioSource; // AudioSourceを取得

    // Start is called before the first frame update
    void Start()
    {
        // 初期位置を代入する
        startPosition = transform.position;

        // 出現した場所が画面の右か左かを判別する
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 0.5f)
        {
            // 左に移動
            isMoveLeft = true;
        }
        else
        {
            // 右に移動
            isMoveRight = true;
        }

        // オーディオソースを取得
        AudioSource = GetComponent<AudioSource>(); // AudioSourceを取得する

    }

    // Update is called once per frame
    void Update()
    {
        // 出現ポイントに応じたフラグの処理
        if (isStopState)
        {// 停止状態なら
            return;
        }

        // 移動量を初期化
        Vector3 moveDir = Vector3.zero;

        if (isMoveLeft)
        {
            // 左に移動
            moveDir = Vector3.left;
        }
        else if (isMoveRight)
        {
            // 右に移動
            moveDir = Vector3.right;
        }

        transform.Translate(moveDir * fItemSpeed * Time.deltaTime);

        // 中央に近づいたら停止
        if (Mathf.Abs(transform.position.x) <= stopRange)
        {
            // 停止状態を有効化
            isStopState = true;
        }

    }

    // TODO : ここの下にプレイヤーとの当たり判定関数の追加
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // オーディオソースが有効 かつ アイテムSEが有効
            if (AudioSource != null && ItemSE != null)
            {
                // サウンド再生
                AudioSource.PlayOneShot(ItemSE);
            }

            // スコア加算
            AddScoreByItemType(itemType);

            // 対象のアイテムを消去
            Destroy(gameObject);
        }
    }

    // 種類に応じたスコアの追加処理
    void AddScoreByItemType(ItemType type)
    {
        int AddScore = 0;

        switch (type)
        {

            case ItemType.ITEMTYPE_0:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_1:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_2:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_3:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_4:
                AddScore += addScore;
                break;
            case ItemType.ITEMTYPE_5:
                AddScore += addScore;
                break;
        }

        // 確認ログ
        Debug.Log($"アイテム取得！スコア +{addScore}");

        // ここでゲーム全体のスコアに加算する関数を呼び出すとかできる
        ScoreManager.Instance.AddScore(addScore);
    }
}
