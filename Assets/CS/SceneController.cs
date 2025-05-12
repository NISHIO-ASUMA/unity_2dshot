using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;          // �ǉ�
using UnityEngine.SceneManagement;     // �ǉ�

public class SceneController : MonoBehaviour
{// �V�[������X�N���v�g

    // �C���X�y�N�^�[����v���n�u������Canvas������p�̕ϐ�
    public GameObject fade;

    // ���삷��Canvas��ݒ肷��
    GameObject FadeCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // FadeManager����isFadeInstance���Q�Ƃ���
        if (!FadeManager.isFadeInstance)
        {
            // �����ő�����Ă����΂����g����
            FadeCanvas = Instantiate(fade);
        }

        // �^�O���o
        if (FadeCanvas == null)
        {
            // �O�̂���
            FadeCanvas = GameObject.FindGameObjectWithTag("Fade");
        }

        // �t�F�[�h�J�n
        FadeCanvas.GetComponent<FadeManager>().FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        // �G���^�[�L�[�������ꂽ�� or A�{�^��
        if (Input.GetKey(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            // ���݂̃V�[���ԍ���1�𑫂���������ێ�����
            int nSceneIndex = SceneManager.GetActiveScene().buildIndex + 1; // ���݂̃A�N�e�B�u�ȃV�[�����擾,���Z

            // �����Q�[���V�[���������疳������
            if (nSceneIndex == 3) 
            {
                return;
            }

            // �ő�V�[���ԍ��Ɣ�r,���������ɂȂ�����ŏ��̃V�[���ɖ߂�
            if (nSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nSceneIndex = 0;
            }

            scenChange(nSceneIndex);
        }

        // Esc�L�[�ŃQ�[���I��
        if (Input.GetKey(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // �Q�[���v���C�I��
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // �Q�[���v���C�I��
            Application.Quit();
#endif
        }
    }

    // �I�u�W�F�N�g���m�p�֐�
    void findFadeObject()
    {
        // null�`�F�b�N
        if (FadeCanvas == null)
        {
            Debug.LogError("FadeCanvas�i�^�O: Fade�j��������܂���ł����I");
            return;
        }
        // Canvas�������� (�^�O��T��)
        FadeCanvas = GameObject.FindGameObjectWithTag("Fade");

        // �t���O�𗧂�,�t�F�[�h�C���֐����Ăяo��
        FadeCanvas.GetComponent<FadeManager>().FadeIn();
    }

    // �V�[���؂�ւ��p�̊֐�
    public async void scenChange(int sceneIndex)
    {
        // null�`�F�b�N
        if (FadeCanvas == null)
        {
            Debug.LogWarning("FadeCanvas ���������Ă��܂���I");
            return;
        }

        // �t���O�𗧂Ă�,�t�F�[�h�A�E�g�֐����Ăяo��
        FadeCanvas.GetComponent<FadeManager>().FadeOut();

        // �Ó]����܂ł����ŏ������~�߂�
        await Task.Delay(100);

        // �Ó]������ɃV�[����؂�ւ���
        SceneManager.LoadScene(sceneIndex);
    }


    // �Ǝ��̃V�[���J�ڂ��s��
    private void LoadSceneWithoutManager(int sceneIndex)
    {
        // �V�[�������擾
        string sceneName = SceneManager.GetSceneAt(sceneIndex).name;

        // ���ۂ̃V�[���؂�ւ�
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
