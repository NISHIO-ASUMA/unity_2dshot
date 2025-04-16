using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPCountDown : MonoBehaviour
{
    // �����o�ϐ�
    public TMP_Text timerText;      // �e�L�X�g���b�V�����擾
    public float startTime = 10f;   // �J�n�b��

    private float timeLeft;
    private bool isCounting = true;

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
        }
    }
}
