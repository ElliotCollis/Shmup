using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _pickUpSprite; // this get's set in the inspector.
    [SerializeField] private GameObject _particlePrefab; // this is spawned when this is destroyed.

    [SerializeField] private AudioClip _SFX1; // 4 different pickup audio cues for each type of pickup.
    [SerializeField] private AudioClip _SFX2;
    [SerializeField] private AudioClip _SFX3;
    [SerializeField] private AudioClip _SFX4;

    private AudioClip pickUpSFX; // the reference audio clip we will set when instantiated, and call when destroyed.

    // there are four pick up types that can be set.
    // I'm using an enum here
    public enum PickUpType // This is creating my custom enum type, this is just stating what is in our enum. Not actually assigning data to it. We can declare multiple of these if we want.
    {
        Health,
        FireRate,
        ShotUp,
        Speed
    }

    public PickUpType _pickUp; // this is declaring my custom enum type. We use the declaired type in out code. This has data assigned to it.

    public void UpdatePickUp(PickUpType SetPickUpType) // When we want to spawn a pickup, we need to also set it's type here, this will set a few things based on what type we pass it.
    {
        _pickUp = SetPickUpType; // We save out new pickup type here.The player can access this later.
        switch (_pickUp) // I'm using a switch statment here to go straight to the code for the given condition. It's much more efficiant than 4 if else statments.
        {
            case PickUpType.Health:
                _pickUpSprite.color = Color.yellow; // set the colour we want to represent our pickup to the player.
                pickUpSFX = _SFX1; // set the specific audio clip for each pickup.
                break;
            case PickUpType.FireRate:
                _pickUpSprite.color = Color.red;
                pickUpSFX = _SFX2;
                break;
            case PickUpType.ShotUp:
                _pickUpSprite.color = Color.blue;
                pickUpSFX = _SFX3;
                break;
            case PickUpType.Speed:
                _pickUpSprite.color = Color.green;
                pickUpSFX = _SFX4;
                break;
            default: // a switch statment always has a default in case nothing is set. If nothing is set we will just say this is a health up.
                _pickUp = PickUpType.Health; // this is how you say what type our enum is.
                _pickUpSprite.color = Color.yellow;
                pickUpSFX = _SFX1;
                break;
        }
    }

    public void Death() // when we destroy this game object, this get's called.
    {
        GameObject go = Instantiate(_particlePrefab, transform.position, Quaternion.identity); // spawn the prefab
        Destroy(go, 3); // destroy it after 3 secs.
        GameManager.instance.sfxManager.PlaySFX(pickUpSFX); // call our sfx manager through our game manager.
        gameObject.SetActive(false);
        //Destroy(this.gameObject); // destroy the pickup.
    }
}
