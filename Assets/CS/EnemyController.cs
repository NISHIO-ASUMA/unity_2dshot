using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{// 敵制御スクリプト

    // 使用変数
    public int EnemyLife = 0;       // 敵の体力
    public float fEnemyMove = 0f;   // 敵の移動量
    public int nAttack = 0;         // 敵の攻撃力
    public float moveRange = 3.0f;  // 敵の往復距離

    private Vector3 startPosition;     // 初期位置
    private Vector3 moveDirection;     // 移動方向

    //*********************************
    // 敵1で使用
    //*********************************
    private float fireTimer_1 = 0f;
    public float fireInterval_1 = 1.0f; // Enemy01用の発射間隔

    //***********************
    // 敵2の動作フラグ関係
    //***********************
    private bool isBouncing = false;   // 横壁反射のフラグ
    private bool isCollisionUp = false;// 上壁反射のフラグ
    private float fireTimer_2 = 0f;
    public float fireInterval_2 = 1.0f; // Enemy02用の発射間隔

    //*********************************************
    // 敵から射出されるバレット
    //*********************************************
    public GameObject bulletPrefab;     // バレットオブジェクトを取得
    public float fireInterval = 1.0f;   // インターバル
    private float fireTimer = 0f;

    //*********************************************
    // 時計回り巡回
    //*********************************************
    private int nPointIdx = 0;
    private List<Vector3> Enemy03Point = new List<Vector3>(); // 巡回ポイント

    //********************************************
    // 追尾用の変数
    //********************************************
    public GameObject Player;      // プレイヤーオブジェクト（Inspectorで設定）
    public float chaseSpeed = 2f;  // 追尾スピード

    Transform playerTr;             // プレイヤーのTransform

    private List<Vector3> patrolPoints = new List<Vector3>(); // 巡回ポイント
    private int currentPointIndex = 0;                        // 現在の目標ポイント

    //********************************************
    // ワープポイント用の変数
    //********************************************
    public float warpInterval = 3.0f; // ワープ間隔
    private float warpTimer = 0f;     // ワープタイマー

    //********************************************
    // パーティクル生成
    //********************************************
    public GameObject hitParticlePrefab;     // 2DパーティクルのPrefab
    private object rigidBody;

    //*********************************************
    // アイテムプレハブ
    //*********************************************
    public GameObject ItemPrefab; // アイテムのプレハブ

    private bool GetScore = false;

    // Start is called before the first frame update
    void Start()
    {
        // 初期位置を代入する
        startPosition = transform.position;

        // タグによって初期の移動方向を設定
        if (CompareTag("Enemy01"))
        {// タグがEnemy01
            moveDirection = Vector3.right; // 初期は右向き
            isBouncing = true;             // フラグを有効化する
        }
        else if (CompareTag("Enemy02"))
        {// タグがEnemy02
            moveDirection = Vector3.up;    // 初期は上向き
            isCollisionUp = true;          // フラグを有効化する
        }
        else if (CompareTag("Enemy03"))
        {// タグがEnemy03

            // 時計回りに巡回する4点（左上→右上→右下→左下）
            Enemy03Point.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.2f, 0.8f, 10))); // 左上
            Enemy03Point.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0.8f, 10))); // 右上
            Enemy03Point.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.8f, 0.2f, 10))); // 右下
            Enemy03Point.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.2f, 0.2f, 10))); // 左下

            transform.position = Enemy03Point[0]; // 初期位置を左上に
            nPointIdx = 1; // 次に向かうのは右上
        }
        else if (CompareTag("Enemy05"))
        {// タグがEnemy05            
            // プレイヤーのTransformを取得（プレイヤーのタグをPlayerに設定必要）
            playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        }
        else if (CompareTag("Enemy06"))
        {// タグがEnemy06
            // 左・右・中央の3点をワールド座標で設定（Viewportベース）
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.5f, 10))); // 左
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.9f, 0.5f, 10))); // 右
            patrolPoints.Add(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10))); // 中央

            transform.position = patrolPoints[0]; // 最初は左
            currentPointIndex = 1; // 次に向かうのは右
        }
        else
        {
            // 移動しない
            moveDirection = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // タグごとに応じた敵の移動処理
        if (CompareTag("Enemy01") && isBouncing)
        {// タグがEnemy01 かつ ループがオンなら
            // 移動
            transform.Translate(moveDirection * fEnemyMove * Time.deltaTime);

            // ビューポート座標に変換
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

            // 左右の端にぶつかったら方向を反転
            if (viewPos.x < 0.1f || viewPos.x > 0.9f)
            {
                moveDirection *= -1; // 方向反転
            }

            // ======= 弾発射処理をタイマー制御する =======
            fireTimer_1 += Time.deltaTime; // タイマー進める

            if (fireTimer_1 >= fireInterval_1)
            {
                fireTimer_1 = 0f; // タイマーリセット

                // 弾の方向を決める
                Vector3 bulletDir = Vector3.down;

                if (viewPos.y < 0.5f)
                {
                    // 弾を上に発射
                    bulletDir = Vector3.up;
                }
                else
                {
                    // 弾を下に発射
                    bulletDir = Vector3.down;
                }

                // 弾のオブジェクトを生成
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // BulletController.csからコンポーネントを取得
                BulletController bc = bullet.GetComponent<BulletController>();

                if (bc != null)
                {
                    bc.SetDirection(bulletDir);
                }
            }
        }
        else if (CompareTag("Enemy02") && isCollisionUp)
        {// タグがEnemy02 かつ ループがオンなら
            // 移動
            transform.Translate(moveDirection * fEnemyMove * Time.deltaTime);

            // ビューポート座標に変換
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

            // 上下の端にぶつかったら方向を反転
            if (viewPos.y < 0.1f || viewPos.y > 0.9f)
            {
                moveDirection *= -1; // 方向反転
            }

            // ======= 弾発射処理をタイマー制御する =======
            fireTimer_2 += Time.deltaTime; // タイマー進める

            if (fireTimer_2 >= fireInterval_2)
            {
                fireTimer_2 = 0f; // タイマーリセット

                // 弾の方向を決める
                Vector3 bulletDir = Vector3.left;

                if (viewPos.x < 0.5f)
                {
                    // 弾を右に発射
                    bulletDir = Vector3.right;
                }
                else
                {
                    // 弾を左に発射
                    bulletDir = Vector3.left;
                }

                // 弾のオブジェクトを生成
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // BulletController.csからコンポーネントを取得
                BulletController bc = bullet.GetComponent<BulletController>();

                if (bc != null)
                {
                    bc.SetDirection(bulletDir);
                }

            }
        }
        else if (CompareTag("Enemy03"))
        {// タグがEnemy03

            // ポイント巡回
            Vector3 targetPoint = Enemy03Point[nPointIdx];

            transform.position = Vector3.MoveTowards(transform.position, targetPoint, fEnemyMove * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                nPointIdx = (nPointIdx + 1) % Enemy03Point.Count;
            }

        }
        else if (CompareTag("Enemy04"))
        {// タグがEnemy04だったら

            // タイマーを加算
            fireTimer += Time.deltaTime;

            // インターバルを超えたら
            if (fireTimer >= fireInterval)
            {
                // タイマーを初期値にする
                fireTimer = 0f;

                // ビューポートから現在の座標を取得
                Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
                Vector3 bulletDir = Vector3.down;

                // 現在の配置されている場所から弾を発射
                if (viewPos.x < 0.5f && viewPos.y >= 0.5f) bulletDir = new Vector3(1, -1, 0);
                else if (viewPos.x >= 0.5f && viewPos.y >= 0.5f) bulletDir = new Vector3(-1, -1, 0);
                else if (viewPos.x < 0.5f && viewPos.y < 0.5f) bulletDir = new Vector3(1, 1, 0);
                else if (viewPos.x >= 0.5f && viewPos.y < 0.5f) bulletDir = new Vector3(-1, 1, 0);

                // 弾のオブジェクトを生成
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // BulletController.csからコンポーネントを取得
                BulletController bc = bullet.GetComponent<BulletController>();

                if (bc != null)
                {// NULLじゃなかったら
                    bc.SetDirection(bulletDir);
                }
            }
        }
        else if (CompareTag("Enemy05") && Player != null)
        {
            // プレイヤーとの距離が0.1f未満になったらそれ以上実行しない
            if (Vector2.Distance(transform.position, playerTr.position) < 0.1f)
                return;

            // プレイヤーに向けて進む
            transform.position = Vector2.MoveTowards
                (
                transform.position,
                new Vector2(playerTr.position.x, playerTr.position.y),
                chaseSpeed * Time.deltaTime);
        }
        else if (CompareTag("Enemy06") && patrolPoints.Count >= 3)
        {// タグがEnemy06 かつ ポイントのカウントが3より大きくなったら
         // タイマーを加算
            warpTimer += Time.deltaTime;

            if (warpTimer >= warpInterval)
            {// インターバルより大きくなったら
             // 初期値に戻す
                warpTimer = 0f;

                // ワープ実行
                transform.position = patrolPoints[currentPointIndex];

                // 次のポイントに切り替え ( 特定の３点に移動する )
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
            }
        }
        else
        {// それ以外の時
            float pingPong = Mathf.PingPong(Time.time * fEnemyMove, moveRange);
            transform.position = startPosition + moveDirection * pingPong;
        }

     }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーの弾タグに当たったら
        if (collision.CompareTag("PlayerBullet"))
        {
            // 敵の体力を減らす
            EnemyLife--;

            // 弾を消す
            Destroy(collision.gameObject);

            // 体力が0以下なら自身を削除
            if (EnemyLife <= 0)
            {
                // アイテムを生成する
                if (ItemPrefab != null)
                {
                    // 敵を倒した位置に出現する
                    Instantiate(ItemPrefab, transform.position, Quaternion.identity);
                }

                if (!GetScore)
                {
                    // 敵のタグに応じてスコアを加算する
                    switch (gameObject.tag)
                    {
                        case "Enemy01":
                            ScoreManager.Instance.AddScore(10000); // Enemy01
                            break;

                        case "Enemy02":
                            ScoreManager.Instance.AddScore(20000); // Enemy02
                            break;

                        case "Enemy03":
                            ScoreManager.Instance.AddScore(30000); // Enemy03
                            break;

                        case "Enemy04":
                            ScoreManager.Instance.AddScore(40000); // Enemy04
                            break;

                        case "Enemy05":
                            ScoreManager.Instance.AddScore(50000); // Enemy05
                            break;

                        case "Enemy06":
                            ScoreManager.Instance.AddScore(50000); // Enemy06
                            break;

                        default:
                            // 対策で0代入
                            ScoreManager.Instance.AddScore(0); // 初期値
                            break;
                    }

                    // フラグを立てて、スコア加算を1回だけ実行
                    GetScore = true;
                }

                // 敵を消去
                Destroy(gameObject);
            }
            else
            {
                // パーティクルを生成
                if (hitParticlePrefab != null)
                {
                    // 衝突位置にパーティクルを生成
                    GameObject hitParticle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);

                    // パーティクルを0.5秒後に消す
                    Destroy(hitParticle, 0.5f);
                }
            }

        }
    }
}
