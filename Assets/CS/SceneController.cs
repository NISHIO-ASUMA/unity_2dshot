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
        {// isFadeInstance��false��������

            Instantiate(fade);
        }

        // �N�����p��Canvas�̐����������҂� (����̏ꍇ��0.02�b��ɌĂяo��)
        Invoke("findFadeObject", 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        // �G���^�[�L�[�������ꂽ��
        if (Input.GetKey(KeyCode.Return))
        {
            // ���݂̃V�[���ԍ���1�𑫂���������ێ�����
            int nSceneIndex = SceneManager.GetActiveScene().buildIndex + 1; // ���݂̃A�N�e�B�u�ȃV�[�����擾,���Z

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
        // Canvas�������� (�^�O��T��)
        FadeCanvas = GameObject.FindGameObjectWithTag("Fade");

        // �t���O�𗧂�,�t�F�[�h�C���֐����Ăяo��
        FadeCanvas.GetComponent<FadeManager>().FadeIn();
    }

    // �V�[���؂�ւ��p�̊֐�
    public async void scenChange(int sceneIndex)
    {
        // �t���O�𗧂Ă�,�t�F�[�h�A�E�g�֐����Ăяo��
        FadeCanvas.GetComponent<FadeManager>().FadeOut();

        // �Ó]����܂ł����ŏ������~�߂�
        await Task.Delay(200);

        // �Ó]������ɃV�[����؂�ւ���
        SceneManager.LoadScene(sceneIndex);
    }
}
