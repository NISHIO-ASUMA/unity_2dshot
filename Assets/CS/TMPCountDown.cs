using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TMPCountDown : MonoBehaviour
{
    //*****************************
    // �g�p�����o�ϐ�
    //*****************************
    public TMP_Text timerText;      // �e�L�X�g���b�V�����擾
    public float startTime = 10f;   // �J�n�b��

    private float timeLeft;
    private bool isCounting = true;

    public SceneController sceneController;     // SceneController�̎Q��

    // Start is called before the first frame update
    void Start()
    {
        // ���Ԃ���
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

            // �^�C���A�E�g���ɃV�[���J��
            SceneChange();
        }
    }

    // �V�[���J�ڗp�̊֐�
    void SceneChange()
    {
        // SceneController���g���ăV�[���J��
        if (sceneController != null)
        {
            int resultSceneIndex = SceneManager.GetSceneByName("Game").buildIndex + 1;  // Result�V�[���̃C���f�b�N�X
            sceneController.scenChange(resultSceneIndex);
        }
    }
}
