using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public AudioSource a_src;
    public List<AudioClip> sounds = new List<AudioClip>();
    public static SoundManager instance;
    void Awake()
    {
        instance = this;
    }

    public void PlaySound(int id)
    {
        a_src.clip = sounds[id];
        a_src.Play();
    }
}
