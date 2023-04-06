using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour // our enemy script used by the enemy and rocks that can hurt the player, need to be tagged as enemy!
{
    public int _health = 1; // how much health this unit has
    public int _points = 1; // how many points is this unit worth
    public GameObject _particlePrefab = null; // explosion effect
    public AudioClip _damagedSFX; // sound when hit
    public AudioClip _destroyedSFX; // sound when destroyed

    public void TakeDamage () // it's hit!
    {
        _health -= 1; // minus one health
        if(_health <= 0) // if health is 0, it dies
        {
            if (_particlePrefab != null) // if we have an exposion effect set, then we create it
            {
                GameObject GO = Instantiate(_particlePrefab, transform.position, Quaternion.identity);
                Destroy(GO, 3); // and destory it after 3 seconds
            }
            Save.instance.UpdateScore(_points); // we update our players score.
            GameManager.instance._sfxManager.PlaySFX(_destroyedSFX); // and play the destroyed sound effect
            Destroy(this.gameObject); // finaly we actually destroy the enemy. This is a great place to use object pooling for optimisation
        }else // or if we're not dead
        {
            GameManager.instance._sfxManager.PlaySFX(_damagedSFX); // play a simple hit sound effect
        }
    }

    public void Death () // if we die from not loosing health, so not the players reason. Instead out of bounds or resetting the game/
    {
        if (_particlePrefab != null)
        {
            GameObject GO = Instantiate(_particlePrefab, transform.position, Quaternion.identity); // spawn an explosion effect still
            Destroy(GO, 3);
        }
        Destroy(this.gameObject); // destory the enemy, but don't increas the players score or make a sound.
    }
}
