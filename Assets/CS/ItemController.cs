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
    public float stopRange = 0.5f;          // 中央に近づいたら停止する範囲
    private int addScore = 0;               // スコア加算量

    private bool isMoveLeft = false;        // 左に行くフラグ
    private bool isMoveRight = false;       // 右に行くフラグ
    private bool isStopState = false;       // 移動終わったら停止するフラグ
    private int nLife = 1;
    private bool isGetItem = false;

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
            // 体力を減らす
            nLife--;

            // オーディオソースが有効 かつ アイテムSEが有効
            if (AudioSource != null && ItemSE != null)
            {
                // サウンド再生
                AudioSource.PlayOneShot(ItemSE);
            }

            // 体力が0以下 かつ フラグがfalse
            if (nLife <= 0)
            {
                if (!isGetItem)
                {
                    switch (gameObject.tag)
                    {
                        // 種類に応じて加算量を決める
                        case "Item000":
                            ScoreManager.Instance.AddScore(1000); // 1種類目
                            break;

                        case "Item001":
                            ScoreManager.Instance.AddScore(1000); // 2種類目
                            break;

                        case "Item002":
                            ScoreManager.Instance.AddScore(4000); // 3種類目
                            break;

                        case "Item003":
                            ScoreManager.Instance.AddScore(5000); // 4種類目
                            break;

                        case "Item004":
                            ScoreManager.Instance.AddScore(10000); // 5種類目
                            break;

                        case "Item005":
                            ScoreManager.Instance.AddScore(20000); // 6種類目
                            break;

                        default:
                            ScoreManager.Instance.AddScore(0);
                            break;
                    }

                    // フラグ有効化
                    isGetItem = true;
                }
                // 対象のアイテムを消去
                Destroy(gameObject);
            }

        }
    }

    // タグに応じたスコアの追加処理
    void AddScoreByItemType(ItemType type)
    {

        // 確認ログ
        Debug.Log($"アイテム取得！スコア +{addScore}");
    }
}
