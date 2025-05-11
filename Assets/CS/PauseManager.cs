using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �V�[�������[�h�p
using TMPro; // �e�L�X�g���b�V�����g�p

public class PauseManager : MonoBehaviour
{// �|�[�Y����X�N���v�g

    //********************************
    // �g�p�����o�ϐ�
    //********************************
    public GameObject pauseMenu;    // �|�[�Y���j���[�̃I�u�W�F�N�g
    public TMP_Text[] menuOptions;      // ���g���C�A�N�C�b�g�A�R���e�B�j���[���i�[
    private int currentIndex = 0;   // �C���f�b�N�X�Ǘ��p
    private bool isPaused = false;  // �|�[�Y�����ۂ�

    public AudioClip selectSE;    // ���j���[�ړ���SE
    public AudioClip enterSe;    // ���j���[���莞SE
    private AudioSource audioSource; // �I�[�f�B�I�\�[�X

    private float inputTimer = 0f;   // �^�C�}�[

    // Start is called before the first frame update
    void Start()
    {
        // �|�[�Y���j���[���\��
        pauseMenu.SetActive(false);

        // �I�[�f�B�I�\�[�X���擾
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // �^�C�}�[���X�V
        inputTimer += Time.unscaledDeltaTime;

        // �|�[�Y�J�n
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {// P�L�[ or Start�{�^������������

            if (!isPaused)
            {// false�Ȃ�
                // �|�[�Y�J�n
                Pause();
            }
            else
            {
                // �|�[�Y���ȊO
                RestartGame();
            }
        }

        // �t���O��true�Ȃ�
        if (isPaused)
        {
            // �L�[�{�[�h�̏㉺�L�[�Ń��j���[����
            float vertical = 0f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // ����L�[�܂���W�L�[
            {
                vertical = 1f;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) // �����L�[�܂���S�L�[
            {
                vertical = -1f;
            }

            // �Q�[���p�b�h�̏\���L�[�̏㉺
            vertical += Input.GetAxisRaw("Vertical");

            // ��ɑI��
            if (vertical > 0f)
            {
                currentIndex--;
                if (currentIndex < 0) currentIndex = menuOptions.Length - 1;
                UpdateMenu();
                PlaySE(selectSE); // SE�Đ�
            }
            // ���ɑI��
            else if (vertical < 0f)
            {
                currentIndex++;
                if (currentIndex >= menuOptions.Length) currentIndex = 0;
                UpdateMenu();
                PlaySE(selectSE); // SE�Đ�
            }

            // ����iEnter�L�[�܂���A�{�^���j
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                SelectOption();
                PlaySE(enterSe);
            }
        }
        
    }

    // �|�[�Y�J�n�֐�
    void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // �Q�[����~
        currentIndex = 0;
        UpdateMenu();
    }

    // �|�[�Y�I���֐�
    void RestartGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // �Q�[���ĊJ
    }

    // ���j���[���ڍX�V�֐�
    void UpdateMenu()
    {
        // ���j���[���ڂ̃n�C���C�g���X�V����
        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (i == currentIndex)
            {
                menuOptions[i].color = Color.yellow; // �I��

            }
            else
            {
                menuOptions[i].color = Color.white;  // ��I��
            }
        }
    }

    // ���ڑI���֐�
    void SelectOption()
    {
        // �C���f�b�N�X�ԍ��ŊǗ�
        switch (currentIndex)
        {
            case 0: // Contine�I����
                RestartGame();
                break;

            case 1: // ���g���C�I����
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // �Q�[���V�[�������ĊJ
                break;

            case 2: // quit�I����
                // �^�C�g���V�[���ɑJ��
                // SceneController���g���ă^�C�g���ɑJ�ڂ���
                SceneController sceneController = FindObjectOfType<SceneController>();

                if (sceneController != null)
                {
                    sceneController.scenChange(0); // �V�[���C���f�b�N�X 0�i�^�C�g���V�[���j
                }

                break;
        }
    }

    // ����炷�֐�
    void PlaySE(AudioClip clip)
    {
        // �I�[�f�B�I�N���b�v��NULL����Ȃ� ���� �I�[�f�B�I�\�[�X���擾�ł�����
        if (clip != null && audioSource != null)
        {
            // ���y���Đ�
            audioSource.PlayOneShot(clip);
        }
    }
}
