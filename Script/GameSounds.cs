using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour
{
    [SerializeField] private AudioSource TitleBGM;

    [SerializeField] private AudioClip GizaBeet;
    [SerializeField] private AudioClip GizaBeet2;
    [SerializeField] private AudioClip Kuroneko;
    void Start()
    {
        TitleBGM.volume = 1.0f;
        RandomBGM();
    }

    void RandomBGM()
    {
        int index = Random.Range(1, 4);
        Debug.Log(index);
        if (index == 1)
        {
            TitleBGM.volume = 0.5f;
            TitleBGM.clip = GizaBeet;
            TitleBGM.Play();
        }
        else if (index == 2)
        {
            TitleBGM.clip = GizaBeet2;
            TitleBGM.Play();
        }
        else
        {
            TitleBGM.clip = Kuroneko;
            TitleBGM.Play();
        }
        TitleBGM.loop = true;
    }
}
