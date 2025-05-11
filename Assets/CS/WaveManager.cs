using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab; // 出す敵プレハブ
        public Vector3 spawnPosition;  // 出現場所
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawnData> enemies;
    }

    public List<Wave> waves;  // 全ウェーブリスト
    private int currentWave = 0;    // ウェーブパス
    private List<GameObject> aliveEnemies = new List<GameObject>(); // 敵生成

    void Start()
    {
        // ウェーブの開始
        StartWave(currentWave);
    }

    void Update()
    {
        // 死んだ敵をリストから除外
        aliveEnemies.RemoveAll(enemy => enemy == null);

        // 全部倒したら次ウェーブ
        if (aliveEnemies.Count == 0)
        {
            // 次のウェーブにカウントを進める
            NextWave();
        }
    }

    void StartWave(int waveIndex)
    {// ウェーブ開始処理
        if (waveIndex >= waves.Count)
        {
            Debug.Log("すべてのウェーブ終了！");

            // Coroutineを使って遷移処理を遅延させる
            StartCoroutine(DelayedSceneChange());

            // 関数の処理終了
            return;
        }

        // ウェーブカウントが上限に達していない場合に敵を出現させる
        if (waveIndex < waves.Count)  // ここで上限チェック
        {
            foreach (var enemyData in waves[waveIndex].enemies)
            {
                // 敵出現
                GameObject enemy = Instantiate(enemyData.enemyPrefab, enemyData.spawnPosition, Quaternion.identity);
                aliveEnemies.Add(enemy);
            }
        }
        else
        {
            Debug.Log("ウェーブカウントが上限に達したため、敵は出現しません。");
        }
    }

    public void NextWave()
    {
        currentWave++;
        StartWave(currentWave);
    }

    // 1秒待ってからシーンを遷移するCoroutine
    IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(1f);  // 1秒待機
        SceneController controller = FindObjectOfType<SceneController>();

        if (controller != null)
        {
            controller.scenChange(3); // 例：ResultシーンのBuildIndexが3なら
        }
        else
        {
            Debug.LogWarning("SceneController が見つかりません！");
        }
    }
}
