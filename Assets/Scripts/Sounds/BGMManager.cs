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
        DontDestroyOnLoad(gameObject); // 씬 변경 시 유지하고 싶을 경우

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = fieldBGM;
        audioSource.loop = true;
        audioSource.Play();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
