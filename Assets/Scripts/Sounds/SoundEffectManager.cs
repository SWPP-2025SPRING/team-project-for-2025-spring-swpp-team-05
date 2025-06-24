using UnityEngine;
using System.Collections.Generic;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    // 재사용 가능한 AudioSource 풀
    private Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();
    private AudioSource oneShotSource; // 단회성 재생 전용

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            oneShotSource = gameObject.AddComponent<AudioSource>();
            oneShotSource.loop = false;
            oneShotSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool PlaySound(string id, AudioClip clip, bool loop = false, float volume = 1f)
    {
        if (sources.ContainsKey(id))
        {
            return false; // 이미 재생 중인 경우 false 반환
        }
        else
        {
            // 새로운 AudioSource 생성
            var src = gameObject.AddComponent<AudioSource>();
            sources[id] = src;
        }

        var source = sources[id];
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        source.Play();
        return true;
    }

    public void PlayOneShotOnce(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            oneShotSource.PlayOneShot(clip, volume);
        }
    }

    public void StopSound(string id)
    {
        if (sources.TryGetValue(id, out var source))
        {
            source.Stop();
            Destroy(source);
            sources.Remove(id);
        }
    }

    public bool IsPlaying(string id)
    {
        return sources.ContainsKey(id) && sources[id].isPlaying;
    }
}
