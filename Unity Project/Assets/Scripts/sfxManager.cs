using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    AudioSource audioSource; // declare the audiosouce
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // save a reference to it
    }

    public void PlaySFX (AudioClip clip) // get an audioclip and play on shot.
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlaySFX (AudioClip clip, float volume) // overload the method so we can also adjust the volume.
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
