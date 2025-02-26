using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // this allows our custom class to show up in the inspector.
public class Boundary // we create a small class here that we can use for our player boundary.
{
    public float xMin, xMax, yMin, yMax; // this is another way to declare more than on variable at a time.
}

public class PlayerController : MonoBehaviour // this controls the player position and shots.
{
    public bool inControl = false; // is the player in control.

    [SerializeField] private float _speed = 4f; // the players speed
    [SerializeField] private float _fireRate = 0.5f; // how fast the player shots
    [SerializeField] private int _shotNumber = 1; // how many shots the player has, max 5.
    [SerializeField] private float _shotSpread = 0.3f; // how spread ourt are the shots
    [SerializeField] private AudioClip _shotSFX; // the shooting sound effect
    [SerializeField] private Boundary _boundary; // we create an instance of our boundary class we can use in this script.
    [SerializeField] private Rigidbody2D _rigidbody2D; // save our rigidbody
    [SerializeField] private GameObject _shotObject; // the shot gameobject
    [SerializeField] private Transform _shotSpawn; // the position we spawn our shot at.
 
    private float nextFire; // we use this as a clock for our firerate.
    private Vector2 movment; // saving our input here to apply to the player.
    private ObjectPool shotPool = null;

    void Awake()
    {
        if (shotPool == null) // set our shot object pool if we haven't alread. 
        {
            shotPool = gameObject.AddComponent<ObjectPool>();
            shotPool.pooledObject = _shotObject;
            shotPool.pooledAmount = 50;
            shotPool.Init();
        }
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); // save the reference of our rigidbody.
    }

    void Update()
    {
        if (inControl) // if the player is in control.
        {
            movment.x = Input.GetAxisRaw("Horizontal"); // save the horazontal input
            movment.y = Input.GetAxisRaw("Vertical"); // and vertical 

            if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time > nextFire) // if we push space, or left mouse button, and the time is greater than our nextfire.
            {
                nextFire = Time.time + _fireRate; // set our next fire to be the current time since the game started + our fire rate.

                GameManager.instance.sfxManager.PlaySFX(_shotSFX); // play our shoot sound effect

                if(_shotNumber > 1) // if we need to create more than one shot
                {
                    float shotStartPos = (-_shotSpread * _shotNumber/2) + _shotSpread/2; // get our start position as half the number of shots timesed by negative the shot speed, which puts it far enough left to be centered when everything is spawned, plus a small offset.
                   
                    for (int i = 0; i < _shotNumber; i++) // for how many shots we need to spawn.
                    {
                        Vector3 newPos = _shotSpawn.position; // save the target position.
                        newPos.x += shotStartPos; // alter it by how far left we need to move it

                        GameObject shotObj = shotPool.GetPooledObject();
                        shotObj.transform.SetPositionAndRotation(newPos, _shotSpawn.rotation);
                        shotObj.SetActive(true);

                        //Instantiate(shot, newPos, shotSpawn.rotation); // create one shot

                        shotStartPos += _shotSpread; // then move our start position over by the spread amount.
                    }
                }else // else if we only need one shot we create one shot.
                {
                    Instantiate(_shotObject, _shotSpawn.position, _shotSpawn.rotation);
                }
            }
        }
    }

    void FixedUpdate() // we use fixed update for any physics we need to do.
    {
        if (inControl) // if we're in control
        {
            //this is one long line of code. that clamps our players position inside our boundaries we set up.
            _rigidbody2D.position = new Vector2(Mathf.Clamp(_rigidbody2D.position.x, _boundary.xMin, _boundary.xMax),
               Mathf.Clamp(_rigidbody2D.position.y, _boundary.yMin, _boundary.yMax));
                
            _rigidbody2D.MovePosition(_rigidbody2D.position + movment * _speed * Time.fixedDeltaTime); // then we move the player it's current position + it's new movment amount timesed by the player speed and fixed delta time to make it smooth.
        }
    }

    // these 4 functions are our power ups
    // can add amount to each of them.

    public void FireRateUp ()
    {
        _fireRate -= 0.1f; // the amount we decrease the wait time between shots.
        _fireRate = Mathf.Clamp(_fireRate, 0.1f, 0.5f); // but clamp it between a minimum.
    }

    public void SpeedUp () // same as firerateup but applies to speed
    {
        _speed += 0.5f;
        _speed = Mathf.Clamp(_speed, 4, 8);
    }

    public void ShotsUp ()// same as firerateup but applies to number of shots 
    {
        _shotNumber ++;
        _shotNumber = Mathf.Clamp(_shotNumber, 1, 5); ;
    }

    public void ResetPlayerController() // reset the power ups to the starting settings, change these if you alter them on the player prefab.
    {
        _fireRate = 0.5f;
        _speed = 4f;
        _shotNumber = 1;
    }
}