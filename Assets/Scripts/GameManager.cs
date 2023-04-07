using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // this manages all the systems in the game. Making sure enemies spawn etc.
{
    public static GameManager instance = null; //make this a singleton

    public LevelManager levelManager;
    public sfxManager sfxManager;
    public Begin begin = null; // reference to our begin script
    public EndScreen endScreen = null; // reference to our end scene UI
    public bool waitingToBegin = false; // are we waiting for player input?

    [Space]

    [SerializeField] private GameObject _playerPrefab; // our player prefab
    [SerializeField] private GameObject _player = null; // our player in the game
    [SerializeField] private GameObject _enemyPrefab; // our enemy prefab
    [SerializeField] private GameObject _rockPrefab; //our rock prefab
    [SerializeField] private GameObject _pickUpPrefab; // our pickup prefab

    private int _currentLevel = 1; // use this as the current level display.
    private bool _spawningWave = false; // are we in the middle of a spawning wave?
    private ObjectPool _enemyPool;
    private ObjectPool _rockPool;
    private ObjectPool _pickUpPool;
    private int _minWidth = -6; // set the screen width.
    private int _maxWidth = 6;
    private Vector3 _playerStartPos = new Vector3(0, -3, 0);

    void Awake() // set as singleton
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);

        // save references to level manager and sfx manager.
        levelManager = GetComponent<LevelManager>();
        sfxManager = GetComponent<sfxManager>();

        //set up the object pools. add a new pool for each object, and set the object reference.
        _enemyPool = gameObject.AddComponent<ObjectPool>();
        _enemyPool.pooledObject = _enemyPrefab;

        _rockPool = gameObject.AddComponent<ObjectPool>();
        _rockPool.pooledObject = _rockPrefab;

        _pickUpPool = gameObject.AddComponent<ObjectPool>();
        _pickUpPool.pooledObject = _pickUpPrefab;
    }

    void Update()
    {
        if(waitingToBegin) // if waiting to begin the game
        {
            if(Input.anyKey) // if player presses any button
            {
                begin.BeginLevel(); // start the game
                waitingToBegin = false; // turn off our check
            }
        }

        if(Input.GetKeyDown(KeyCode.P)) // if we press p spawn a pickup, this is a cheat for testing purposes.
        {
            SpawnPickUp(Vector3.zero); //spawn a pick up at 0, 0, 0
        }
    }
    
    public PlayerController SpawnPlayer () // spawn our player
    {
       if(_player == null) // if we don't have a player in the game instantiate one.
        {
            _player = Instantiate(_playerPrefab, _playerStartPos, Quaternion.identity);
        }
       else // otherwise turn the player onand in position
        {
            _player.transform.position = _playerStartPos;
            _player.GetComponent<PlayerHealth>().ResetHealth();
            _player.SetActive(true);
        }

        return _player.GetComponent<PlayerController>(); // return a reference, this is used in our begin script.
    }

    public void StartGame () //start the game
    {
        if(!_spawningWave) // if not spawning start the spawn wave coroutine.
        {
            StartCoroutine("SpawnWave");
        }
    }

    public void StartNextLevel () // start the next level
    {
        _currentLevel ++; // increase level count by one
        Save.instance.UpdateLevel(_currentLevel); //update level UI
        if (!_spawningWave) // if not spawning start the spawn wave coroutine.
        {
            StartCoroutine("SpawnWave");
        }
    }

    public void GameOver () // when our player dies this is called.
    {
        _player.SetActive(false); // turn the player off
        StopAllCoroutines(); // this stops all spawning.
        _spawningWave = false; // and make sure our check is now false.
        endScreen.score.text = Save.instance.currentScore.ToString(); // update our end screen UI with score and high score.
        endScreen.highScore.text = Save.instance.bestScore.ToString();
        endScreen.gameObject.SetActive(true); // turn end screen UI on.
    }

    public void Retry() // this resets everything when we retry.
    {
        foreach (GameObject go in _enemyPool.pooledObjects) //Check for active objects in the scene.
        {
            if (go.activeInHierarchy)
                go.GetComponent<Enemy>().Death(); //kill it
        }

        foreach (GameObject go in _rockPool.pooledObjects) 
        {
            if (go.activeInHierarchy)
                go.GetComponent<Enemy>().Death();
        }

        foreach (GameObject go in _pickUpPool.pooledObjects)
        {
            if (go.activeInHierarchy)
                go.GetComponent<PickUp>().Death();
        }

        endScreen.score.text = 0.ToString(); // everything here is just resetting score and level count.
        endScreen.highScore.text = 0.ToString();
        endScreen.gameObject.SetActive(false);
        Save.instance.currentScore = 0;
        Save.instance.UpdateScore(0);
        _currentLevel = 1;
        Save.instance.UpdateLevel(_currentLevel);
        begin.BeginLevel(); // set our begin script again.
    }

    IEnumerator SpawnWave () // our spawn coroutine, A coroutine is a special function that can loop or hold code in it.
    {
        _spawningWave = true; // say we're spawning
        yield return new WaitForSeconds(0.2f); //start wait time;
        int waveCount = Random.Range(1 + _currentLevel,8 + _currentLevel); // what's our wave count, and adjust as the player progresses. 
        int waveFrequency = 1 + _currentLevel; // same for frequency of waves.
        bool spawnEnemies = false; // save a reference if we're spawning enemies or rocks.
        Vector2 spawnPosition = new Vector2(0, 6); // set our position for spawning enemies.

        while (waveFrequency > 0) // while our wave count is greater than zero repeat this code snippit. 
        {
            for (int i = 0; i < waveCount; i++) // Run through this block of code for our wave count amount.
            {
                GameObject go; // create gameobject reference and instantiate an enemy or rock as it.
                spawnPosition.x = Random.Range(_minWidth, _maxWidth); // make our spawn position random between our min and max width
                if (spawnEnemies)
                {
                    go = _enemyPool.GetPooledObject(); // get an object from the pooled list.
                    //go = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity); // updated to use object pool. 
                }else
                {
                    go = _rockPool.GetPooledObject();
                    //go = Instantiate(_rockPrefab, spawnPosition, Quaternion.identity);
                }

                go.transform.position = spawnPosition;
                go.SetActive(true);

                yield return new WaitForSeconds(Mathf.Clamp(1f - (_currentLevel * 0.1f), 0.1f, 2)); // this reduces the wait as the game gets harder so more enemies spawn faster.
            }
            waveFrequency -= 1; // reduce wave frequency by 1 after spawning a wave.
            spawnEnemies = !spawnEnemies; // flip spawn enemies. basically says spawnenemies is equal to not spawnenemies. 
            yield return new WaitForSeconds(Mathf.Clamp(2f - (_currentLevel * 0.1f), 0.1f, 2)); // wait to spawn next wave, also gets shorter time.

            spawnPosition.x = Random.Range(_minWidth, _maxWidth); ; 
            SpawnPickUp(spawnPosition); // at the end of a wave spawn a powerup.
        }

        bool activeUnits = true;

        while(activeUnits) // while there are still enemies on screen, don't start the next level.
        {
            activeUnits = false;
            foreach (GameObject go in _enemyPool.pooledObjects)
            {
                if (go.activeInHierarchy) activeUnits = true;
            }
            foreach (GameObject go in _rockPool.pooledObjects)
            {
                if (go.activeInHierarchy) activeUnits = true;
            }
            yield return null;
        }

        spawnPosition.x = Random.Range(_minWidth, _maxWidth); ;
        SpawnPickUp(spawnPosition); // spawn another powerup at the end of the level

        begin.BeginNextLevel(); // start the next level countdown
        _spawningWave = false;
    }

    public void SpawnPickUp (Vector3 pos) // our spawn pickup code. Sets it to a random powerup at pos. 
    {
        //GameObject temppickup = Instantiate(_pickUpPrefab, pos, Quaternion.identity);
        PickUp temppickup = _pickUpPool.GetPooledObject().GetComponent<PickUp>(); // updated to use object pool.
        temppickup.transform.position = pos;
        temppickup.gameObject.SetActive(true);

        int ran = Random.Range(1, 5);
        switch (ran) // using a switch statment to quickly choose the out come of the random number.
        {
            case 1:
                temppickup.UpdatePickUp(PickUp.PickUpType.FireRate);
                break;
            case 2:
                temppickup.UpdatePickUp(PickUp.PickUpType.Health);
                break;
            case 3:
                temppickup.UpdatePickUp(PickUp.PickUpType.ShotUp);
                break;
            case 4:
                temppickup.UpdatePickUp(PickUp.PickUpType.Speed);
                break;
            default:
                break; 
        }
    }
}
