using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip fieldBGM;
    public AudioClip battleBGM; // Background music for battle scenes
    public AudioClip finalBGM; // Background music for the final stage
    private AudioSource audioSource;

    public static BGMManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = fieldBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PauseBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeBGM()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PlayFieldBGM()
    {
        audioSource.clip = fieldBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayBattleBGM()
    {
        audioSource.clip = battleBGM;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayFinalBGM()
    {
        audioSource.clip = finalBGM;
        audioSource.loop = true;
        audioSource.Play();
    }
}
