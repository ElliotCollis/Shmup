using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    private AudioSource _audioSource; // declare the audiosouce

    void Start()
    {
        _audioSource = GetComponent<AudioSource>(); // save a reference to it
    }

    public void PlaySFX (AudioClip clip) // get an audioclip and play on shot.
    {
        _audioSource.PlayOneShot(clip);
    }

    public void PlaySFX (AudioClip clip, float volume) // overload the method so we can also adjust the volume.
    {
        _audioSource.PlayOneShot(clip, volume);
    }

    // could also use AudioSource.PlayClipAtPoint() for such a simple sfx Manager
}
