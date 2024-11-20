using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public SoundType[] Sounds;

    public AudioSource soundEffect;
    public AudioSource soundMusic;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        soundMusic.loop = true;
        PlayMusic(global::Sounds.Music);
    }


    public void PlayMusic(Sounds sound)
    {
        AudioClip clip = GetSoundClip(sound);
        if(clip != null)
        {
            soundMusic.clip = clip;
            soundMusic.Play();
        }
        else
        {
            Debug.LogError("Clip not found");
        }
    }

    public void Play(Sounds sound)
    {
        AudioClip clip = GetSoundClip(sound);
        if(clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found");
        }
    }

    private AudioClip GetSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(Sounds, i => i.soundType == sound);
        if( item != null)
            return item.soundClip;
        return null;
    }
}

[Serializable]  
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}

public enum Sounds
{
    ButtonClick,
    Music,
    PlayerDeath,
    Gainer,
    PowerUp,
    Burner,
    LevelFinish,
    LevelFail,
}
