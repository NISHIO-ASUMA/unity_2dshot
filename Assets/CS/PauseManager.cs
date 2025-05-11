using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーンリロード用
using TMPro; // テキストメッシュを使用

public class PauseManager : MonoBehaviour
{// ポーズ制御スクリプト

    //********************************
    // 使用メンバ変数
    //********************************
    public GameObject pauseMenu;    // ポーズメニューのオブジェクト
    public TMP_Text[] menuOptions;      // リトライ、クイット、コンティニューを格納
    private int currentIndex = 0;   // インデックス管理用
    private bool isPaused = false;  // ポーズ中か否か

    public AudioClip selectSE;    // メニュー移動時SE
    public AudioClip enterSe;    // メニュー決定時SE
    private AudioSource audioSource; // オーディオソース

    private float inputTimer = 0f;   // タイマー

    // Start is called before the first frame update
    void Start()
    {
        // ポーズメニューを非表示
        pauseMenu.SetActive(false);

        // オーディオソースを取得
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // タイマーを更新
        inputTimer += Time.unscaledDeltaTime;

        // ポーズ開始
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {// Pキー or Startボタンを押したら

            if (!isPaused)
            {// falseなら
                // ポーズ開始
                Pause();
            }
            else
            {
                // ポーズ中以外
                RestartGame();
            }
        }

        // フラグがtrueなら
        if (isPaused)
        {
            // キーボードの上下キーでメニュー操作
            float vertical = 0f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // 上矢印キーまたはWキー
            {
                vertical = 1f;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) // 下矢印キーまたはSキー
            {
                vertical = -1f;
            }

            // ゲームパッドの十字キーの上下
            vertical += Input.GetAxisRaw("Vertical");

            // 上に選択
            if (vertical > 0f)
            {
                currentIndex--;
                if (currentIndex < 0) currentIndex = menuOptions.Length - 1;
                UpdateMenu();
                PlaySE(selectSE); // SE再生
            }
            // 下に選択
            else if (vertical < 0f)
            {
                currentIndex++;
                if (currentIndex >= menuOptions.Length) currentIndex = 0;
                UpdateMenu();
                PlaySE(selectSE); // SE再生
            }

            // 決定（EnterキーまたはAボタン）
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                SelectOption();
                PlaySE(enterSe);
            }
        }
        
    }

    // ポーズ開始関数
    void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // ゲーム停止
        currentIndex = 0;
        UpdateMenu();
    }

    // ポーズ終了関数
    void RestartGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // ゲーム再開
    }

    // メニュー項目更新関数
    void UpdateMenu()
    {
        // メニュー項目のハイライトを更新する
        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (i == currentIndex)
            {
                menuOptions[i].color = Color.yellow; // 選択中

            }
            else
            {
                menuOptions[i].color = Color.white;  // 非選択
            }
        }
    }

    // 項目選択関数
    void SelectOption()
    {
        // インデックス番号で管理
        switch (currentIndex)
        {
            case 0: // Contine選択時
                RestartGame();
                break;

            case 1: // リトライ選択時
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ゲームシーンをを再開
                break;

            case 2: // quit選択時
                // タイトルシーンに遷移
                // SceneControllerを使ってタイトルに遷移する
                SceneController sceneController = FindObjectOfType<SceneController>();

                if (sceneController != null)
                {
                    sceneController.scenChange(0); // シーンインデックス 0（タイトルシーン）
                }

                break;
        }
    }

    // 音を鳴らす関数
    void PlaySE(AudioClip clip)
    {
        // オーディオクリップがNULLじゃない かつ オーディオソースが取得できたら
        if (clip != null && audioSource != null)
        {
            // 音楽を再生
            audioSource.PlayOneShot(clip);
        }
    }
}
