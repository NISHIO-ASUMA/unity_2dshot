using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;          // 追加
using UnityEngine.SceneManagement;     // 追加

public class SceneController : MonoBehaviour
{// シーン制御スクリプト

    // インスペクターからプレハブ化したCanvasを入れる用の変数
    public GameObject fade;

    // 操作するCanvasを設定する
    GameObject FadeCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // FadeManagerからisFadeInstanceを参照する
        if (!FadeManager.isFadeInstance)
        {
            // ここで代入しておけばすぐ使える
            FadeCanvas = Instantiate(fade);
        }

        // タグ検出
        if (FadeCanvas == null)
        {
            // 念のため
            FadeCanvas = GameObject.FindGameObjectWithTag("Fade");
        }

        // フェード開始
        FadeCanvas.GetComponent<FadeManager>().FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        // エンターキーが押されたら or Aボタン
        if (Input.GetKey(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            // 現在のシーン番号に1を足した数字を保持する
            int nSceneIndex = SceneManager.GetActiveScene().buildIndex + 1; // 現在のアクティブなシーンを取得,加算

            // もしゲームシーンだったら無視する
            if (nSceneIndex == 3) 
            {
                return;
            }

            // 最大シーン番号と比較,超えそうになったら最初のシーンに戻す
            if (nSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nSceneIndex = 0;
            }

            scenChange(nSceneIndex);
        }

        // Escキーでゲーム終了
        if (Input.GetKey(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // ゲームプレイ終了
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // ゲームプレイ終了
            Application.Quit();
#endif
        }
    }

    // オブジェクト検知用関数
    void findFadeObject()
    {
        // nullチェック
        if (FadeCanvas == null)
        {
            Debug.LogError("FadeCanvas（タグ: Fade）が見つかりませんでした！");
            return;
        }
        // Canvasを見つける (タグを探す)
        FadeCanvas = GameObject.FindGameObjectWithTag("Fade");

        // フラグを立て,フェードイン関数を呼び出す
        FadeCanvas.GetComponent<FadeManager>().FadeIn();
    }

    // シーン切り替え用の関数
    public async void scenChange(int sceneIndex)
    {
        // nullチェック
        if (FadeCanvas == null)
        {
            Debug.LogWarning("FadeCanvas が見つかっていません！");
            return;
        }

        // フラグを立てて,フェードアウト関数を呼び出す
        FadeCanvas.GetComponent<FadeManager>().FadeOut();

        // 暗転するまでここで処理を止める
        await Task.Delay(100);

        // 暗転した後にシーンを切り替える
        SceneManager.LoadScene(sceneIndex);
    }


    // 独自のシーン遷移を行う
    private void LoadSceneWithoutManager(int sceneIndex)
    {
        // シーン名を取得
        string sceneName = SceneManager.GetSceneAt(sceneIndex).name;

        // 実際のシーン切り替え
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
