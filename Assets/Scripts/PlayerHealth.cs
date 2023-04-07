using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour // this manages the players health.
{
    [SerializeField] private int _health = 3; // max health is 25 because that's all that fits on screen.
    [SerializeField] private bool _invincible = false; // check to see if the player can take damage or not.
    [SerializeField] private SpriteRenderer _spriteRenderer; // the sprite for our player, this is for the iFrames. this needs to be set to 1 sprite on the player, you can change this to animation if you want.
    [SerializeField] private GameObject _particlePrefab; // player death explosion effect
    [SerializeField] private AudioClip _damagedSFX; // audio effect for when the player takes dinner.
    [SerializeField] private AudioClip _destroyedSFX; // audio effect when the player dies.
    [SerializeField] private Color _playerColour; // save the players colour for the iframes effect.

    private PlayerHealthUI _healthUI; // the health UI script.
    private PlayerController _playerController; // the player controller script

    void Start()
    {
        _healthUI = GameObject.FindObjectOfType<PlayerHealthUI>(); // save the refernce to our health UI 
        _healthUI.UpdateHealthUI(_health); // update the health UI to show our starting health.
        _playerController = GetComponent<PlayerController>(); // save our player controller reference too
        _spriteRenderer.color = _playerColour;
    }

    public void StartIFrames () // play IFs animation and make the player invincable for 3 seconds.
    {
        if(!_invincible)
        {
            StartCoroutine("IFrames", 3f);
        }
    }

    public void ResetHealth() // resets our starting health to 3, and updates the UI
    {
        _health = 3;
        _healthUI.UpdateHealthUI(_health);
    }

    public void HealthPack () // when we get a health pack, update our health if it's less than 25, and update the UI
    {
        if (_health <= 25)
        {
            _health ++;
            _healthUI.UpdateHealthUI(_health);
        }
    }

    public void TakeAHit() // when we take damage, if we're not invincable
    {
        if (_health > 1 && !_invincible) // and not dead
        {
            StartCoroutine("IFrames", 1f); // apply invincability for 1 second
            _health -= 1; // take away 1 health
            _healthUI.UpdateHealthUI(_health); // update the UI
            GameManager.instance.sfxManager.PlaySFX(_damagedSFX); // and play our damage sound effect.
        }
        else if (_health == 1 && !_invincible) // or we are dead
        {
            _health -= 1; // take our health to zero
            _healthUI.UpdateHealthUI(_health); //update UI
            GameObject go = Instantiate(_particlePrefab, transform.position, Quaternion.identity); // play explosion particle effect
            Destroy(go, 3); // destroy the particles after 3 seconds
            GameManager.instance.sfxManager.PlaySFX(_destroyedSFX); // play our destroy sound effect
            GameManager.instance.GameOver(); // set our game over state.
        }
        _playerController.ResetPlayerController(); // reset all the powerups we've had back to the basics, a little mean, you can change this if you want.
    }

    IEnumerator IFrames(float timeToWait) // our invincability frames, is actually a timmer not set frames.
    {
        _invincible = true; // make us invincable.

        // switch our player colour from red to white a few times. 

        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(timeToWait / 5);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(timeToWait / 5);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(timeToWait / 5);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(timeToWait / 5);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(timeToWait / 5);
        _spriteRenderer.color = _playerColour;

        _invincible = false; // turn off invincability.
    }

    private void OnTriggerEnter2D(Collider2D collision) // if we overlap with anything.
    {
        if(collision.CompareTag("Enemy")) // if it's the enemy.
        {
            TakeAHit(); // take damage
            collision.GetComponent<Enemy>().Death(); // kil lthe enemy.
        }
        if(collision.CompareTag("PickUp")) // if it's a pickup
        {
            switch (collision.GetComponent<PickUp>()._pickUp) // do a different powerup depending on what the pickup was.
            {
                case PickUp.PickUpType.Health:
                    HealthPack();
                    break;
                case PickUp.PickUpType.FireRate:
                    _playerController.FireRateUp();
                    break;
                case PickUp.PickUpType.ShotUp:
                    _playerController.ShotsUp();
                    break;
                case PickUp.PickUpType.Speed:
                    _playerController.SpeedUp();
                    break;
                default:
                    break;
            }
            collision.GetComponent<PickUp>().Death(); // kill the powerup.
        }
    }
}
