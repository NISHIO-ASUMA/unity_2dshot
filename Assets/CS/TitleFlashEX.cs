using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFlashEX : MonoBehaviour
{// タイトル画像点滅制御

    //*********************************
    // 使用メンバ変数
    //*********************************
    private GameObject titleKeyObject; // ゲームオブジェクト
    private Coroutine flashCoroutine;  // 点滅カウンター

    public AudioClip KeySE;           // キー入力時のSE
    private AudioSource audioSource;  // AudioSourceの取得用

    // Start is called before the first frame update
    void Start()
    {
        // "TitleKey"タグのオブジェクトを取得
        titleKeyObject = GameObject.FindWithTag("TitleKey");

        // 例外処理
        if (titleKeyObject == null)
        {
            // デバッグログ
            Debug.LogWarning("TitleKeyタグのオブジェクトが見つかりません");
        }

        audioSource = GetComponent<AudioSource>(); // AudioSourceを取得する

    }

    // Update is called once per frame
    void Update()
    {
        // EnterキーまたはゲームパッドのAボタンが押されたら点滅開始
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0)) && titleKeyObject != null)
        {
            if (flashCoroutine == null)
            {
                // 点滅カウンターを更新する
                flashCoroutine = StartCoroutine(FlashRoutine());
            }

            // SE再生
            if (KeySE != null && audioSource != null)
            {
                // SE再生
                audioSource.PlayOneShot(KeySE);
            }

        }
    }

    // 点滅処理のコルーチン関数
    IEnumerator FlashRoutine()
    {
        // コンポーネント取得
        Renderer renderer = titleKeyObject.GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.LogWarning("TitleKeyオブジェクトにRendererがありません");
            yield break;
        }

        while (true)
        {
            // 点滅感覚の切り替え
            renderer.enabled = !renderer.enabled;

            yield return new WaitForSeconds(0.03f); // 0.5秒ごとに切り替え
        }
    }
}
