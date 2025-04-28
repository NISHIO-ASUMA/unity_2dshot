using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TMPCountDown : MonoBehaviour
{
    //*****************************
    // 使用メンバ変数
    //*****************************
    public TMP_Text timerText;      // テキストメッシュを取得
    public float startTime = 10f;   // 開始秒数

    private float timeLeft;
    private bool isCounting = true;

    public SceneController sceneController;     // SceneControllerの参照

    // Start is called before the first frame update
    void Start()
    {
        // 時間を代入
        timeLeft = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCounting) return;

        timeLeft -= Time.deltaTime;
        int displayTime = Mathf.Clamp(Mathf.FloorToInt(timeLeft), 0, 999);
        timerText.text = displayTime.ToString();

        if (timeLeft <= 0f)
        {
            isCounting = false;
            timerText.text = "0";

            // タイムアウト時にシーン遷移
            SceneChange();
        }
    }

    // シーン遷移用の関数
    void SceneChange()
    {
        // SceneControllerを使ってシーン遷移
        if (sceneController != null)
        {
            int resultSceneIndex = SceneManager.GetSceneByName("Game").buildIndex + 1;  // Resultシーンのインデックス
            sceneController.scenChange(resultSceneIndex);
        }
    }
}
