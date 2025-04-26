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
            return;
        }

        foreach (var enemyData in waves[waveIndex].enemies)
        {
            GameObject enemy = Instantiate(enemyData.enemyPrefab, enemyData.spawnPosition, Quaternion.identity);
            aliveEnemies.Add(enemy);
        }
    }

    public void NextWave()
    {
        currentWave++;
        StartWave(currentWave);
    }
}
