using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{// 音楽再生スクリプト

    // 音楽のメンバ変数
    private AudioSource audioSource;    // オーディオソースを追加する

    // Start is called before the first frame update
    void Start()
    {
        // オーディオソースを取得
        audioSource = GetComponent<AudioSource>();

        // 発見できたら
        if (audioSource != null)
        {
            audioSource.Play();  // BGMの再生を開始
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
