using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource aud;
    [SerializeField] private AudioClip welcomeAudioClip;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.playOnAwake = false;
        aud.loop = false;
    }
    //we play the welcome audio clip
    public void PlayWelcomeAudioClip()
    {
        aud.clip= welcomeAudioClip;
        aud.Play();
    }
}
