using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private GameObject _particlePrefab = null; // explosion effect
    [SerializeField] private AudioClip _explosionSFX; // explosion sound

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
        // will be good to implement a particle system pooler. But that will take a bit more time.
        // instead of implementing a seperate pool for each system a generic particle pool would be best.
        // Than applying the particle data ontop of that when needed.

        if(_particlePrefab != null) // make sure we have an explosion particles set.
        {
            GameObject go = Instantiate(_particlePrefab, transform.position, Quaternion.identity); // create the explosion
            Destroy(go, 2); // destroy after 2 secs.
        }
        GameManager.instance.sfxManager.PlaySFX(_explosionSFX); // play our sound effect

        gameObject.SetActive(false);
        //Destroy(this.gameObject); // destory this object. This object could be pooled to save memory, in which case this changes to SetActive(false).
    }
}
