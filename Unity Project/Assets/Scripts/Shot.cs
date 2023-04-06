using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject _particlePrefab = null; // explosion effect
    public AudioClip _explosionSFX; // explosion sound

    private void OnTriggerEnter2D(Collider2D collision) // when we overlap something
    {
        if(collision.CompareTag("Enemy")) // if it's an enemy
        {
            collision.GetComponent<Enemy>().TakeDamage(); // the enemy takes damage
            explode(); // and we explore
        }
    }

    void explode()
    {
        if(_particlePrefab != null) // make sure we have an explosion particles set.
        {
            GameObject GO = Instantiate(_particlePrefab, transform.position, Quaternion.identity); // create the explosion
            Destroy(GO, 2); // destroy after 2 secs.
        }
        GameManager.instance._sfxManager.PlaySFX(_explosionSFX); // play our sound effect
        Destroy(this.gameObject); // destory this object. This object could be pooled to save memory, in which case this changes to SetActive(false).
    }
}
