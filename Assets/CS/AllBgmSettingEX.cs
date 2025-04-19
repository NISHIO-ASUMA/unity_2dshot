using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllBgmSettingEX : MonoBehaviour
{
    // �C���X�^���X
    public static AllBgmSettingEX instance;

    public AudioClip[] bgmClips; // �V�[�����Ƃ�BGM���X�g
    private AudioSource audioSource;

    void Awake()
    {
        // �V���O���g�����i�d���h�~�j
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �V�[�����܂����ŕێ�
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

        // �V�[�����ύX���ꂽ�Ƃ��ɌĂ΂��C�x���g�o�^
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
        // �t�F�[�h����BGM���~�߂�^�I�������ĊJ
        if (FadeManager.IsFading)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause(); // �t�F�[�h�� �� �ꎞ��~
            }
        }
        else
        {
            if (!audioSource.isPlaying && audioSource.clip != null)
            {
                audioSource.UnPause(); // �t�F�[�h�I�� �� �ĊJ
            }
        }
    }
}