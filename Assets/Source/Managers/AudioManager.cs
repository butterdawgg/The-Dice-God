using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Sound[] sounds;
    [SerializeField] private Sound[] music;

    private bool isMusicPlaying;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;

            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;
        }

        for (int i = 0; i < music.Length; i++)
        {
            music[i].source = gameObject.AddComponent<AudioSource>();
            music[i].source.clip = music[i].clip;

            music[i].source.volume = music[i].volume;
            music[i].source.pitch = music[i].pitch;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source.volume = sounds[i].volume * SerializeManager.Instance.GetFloat(FloatType.SfxVolume) * SerializeManager.Instance.GetFloat(FloatType.MasterVolume);
        }

        for (int i = 0; i < music.Length; i++)
        {
            music[i].source.volume = music[i].volume * SerializeManager.Instance.GetFloat(FloatType.MusicVolume) * SerializeManager.Instance.GetFloat(FloatType.MasterVolume);
        }

        if (!isMusicPlaying)
        {
            isMusicPlaying = true;
            StartCoroutine(PlayMusicCoroutine());
        }
    }

    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
                sounds[i].source.Play();
        }
    }

    public void StopSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
                sounds[i].source.Stop();
        }
    }

    public void PlayMusic(string name)
    {
        for (int i = 0; i < music.Length; i++)
        {
            if (music[i].name == name)
                music[i].source.Play();
        }
    }

    public IEnumerator PlayMusicCoroutine()
    {
        int random = Random.Range(0, music.Length);

        PlayMusic(music[random].name);

        yield return new WaitForSeconds(music[random].clip.length);

        isMusicPlaying = false;
    }
}
