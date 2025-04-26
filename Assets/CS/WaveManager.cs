using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab; // �o���G�v���n�u
        public Vector3 spawnPosition;  // �o���ꏊ
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawnData> enemies;
    }

    public List<Wave> waves;  // �S�E�F�[�u���X�g
    private int currentWave = 0;    // �E�F�[�u�p�X
    private List<GameObject> aliveEnemies = new List<GameObject>(); // �G����

    void Start()
    {
        // �E�F�[�u�̊J�n
        StartWave(currentWave);
    }

    void Update()
    {
        // ���񂾓G�����X�g���珜�O
        aliveEnemies.RemoveAll(enemy => enemy == null);

        // �S���|�����玟�E�F�[�u
        if (aliveEnemies.Count == 0)
        {
            // ���̃E�F�[�u�ɃJ�E���g��i�߂�
            NextWave();
        }
    }

    void StartWave(int waveIndex)
    {// �E�F�[�u�J�n����
        if (waveIndex >= waves.Count)
        {
            Debug.Log("���ׂẴE�F�[�u�I���I");
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
