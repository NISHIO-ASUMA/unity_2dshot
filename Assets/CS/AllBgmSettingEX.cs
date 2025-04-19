using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllBgmSettingEX : MonoBehaviour
{
    // インスタンス
    public static AllBgmSettingEX instance;

    public AudioClip[] bgmClips; // シーンごとのBGMリスト
    private AudioSource audioSource;

    void Awake()
    {
        // シングルトン化（重複防止）
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいで保持
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
    }

    void Start()
    {
        PlayBgmBySceneIndex(SceneManager.GetActiveScene().buildIndex);

        // シーンが変更されたときに呼ばれるイベント登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBgmBySceneIndex(scene.buildIndex);
    }

    public void PlayBgmBySceneIndex(int index)
    {
        if (bgmClips == null || index >= bgmClips.Length) return;

        AudioClip clip = bgmClips[index];
        if (clip != null && audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void Update()
    {
        // フェード中はBGMを止める／終わったら再開
        if (FadeManager.IsFading)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause(); // フェード中 → 一時停止
            }
        }
        else
        {
            if (!audioSource.isPlaying && audioSource.clip != null)
            {
                audioSource.UnPause(); // フェード終了 → 再開
            }
        }
    }
}