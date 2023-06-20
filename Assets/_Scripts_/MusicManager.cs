using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioClip[] music;

    private AudioSource musicSource;
    private bool playMusic = true;

    private void Start()
    {
        instance = this;
        musicSource = GetComponent<AudioSource>();
        playMusic = GameManager.instance.PlayMusic;        
    }

    private void FixedUpdate()
    {
        if (!musicSource.isPlaying) 
        { 
            SetMusicClip();
        }

        if(playMusic != GameManager.instance.PlayMusic)
        {
            playMusic = GameManager.instance.PlayMusic;

            if (!playMusic)
            {
                StopMusic();
            }
            else
            {
                PlayMusic();
            }
        }
    }

    // Sets the Jump sound effect as clip to be played
    public void SetMusicClip()
    {
        musicSource.clip = music[Random.Range(0, music.Length)];
        PlayMusic();
    }

    // Plays the defined sound effect
    private void PlayMusic()
    {
        if (playMusic)
        {
            if (musicSource.isPlaying)
            {
                musicSource.Stop();
            }
            musicSource.Play();
        }
    }

    // Stop playing background music
    public void StopMusic()
    {
        musicSource.Stop();
    }
}