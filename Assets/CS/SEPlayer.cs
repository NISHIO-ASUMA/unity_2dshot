using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{// SE�Ǘ�
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // SE�Đ��֐�
    public static void PlaySE(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        GameObject seObj = new GameObject("SE_Player");
        seObj.transform.position = position;

        AudioSource source = seObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        // SE�̒������I�������I�u�W�F�N�g�������Ŕj��
        Destroy(seObj, clip.length);
    }
}
