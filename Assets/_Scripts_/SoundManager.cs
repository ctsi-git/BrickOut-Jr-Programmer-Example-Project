using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
       
    [SerializeField] AudioClip bounceSound;
    [SerializeField] AudioClip lostBallSound;
    
    private AudioSource soundEffectsSource;

    private void Start()
    {
        instance = this;
        soundEffectsSource = GetComponent<AudioSource>();
    }

    // Sets the Bounce sound effect as clip to be played
    public void SetBounceSoundEffect()
    {
        soundEffectsSource.clip = bounceSound;
        PlaySoundEffect();
    }

    
    // Sets the sound effect when the ball is lost as clip to be played
    public void SetLostBallSoundEffect()
    {
        soundEffectsSource.clip = lostBallSound;
        PlaySoundEffect();
    }

    // Plays the defined sound effect
    private void PlaySoundEffect()
    {
        if (soundEffectsSource.isPlaying)
        {
            soundEffectsSource.Stop();
        }
        soundEffectsSource.Play();
    }
}
