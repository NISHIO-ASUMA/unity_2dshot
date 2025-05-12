using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{// SE管理
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // SE再生関数
    public static void PlaySE(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        GameObject seObj = new GameObject("SE_Player");
        seObj.transform.position = position;

        AudioSource source = seObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        // SEの長さが終わったらオブジェクトを自動で破棄
        Destroy(seObj, clip.length);
    }
}
