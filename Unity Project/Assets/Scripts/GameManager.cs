using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // this manages all the systems in the game. Making sure enemies spawn etc.
{
    public LevelManager _levelManager;
    public sfxManager _sfxManager;


    public int _currentLevel = 1; // use this as the current level display.
    public Begin _begin = null; // reference to our begin script
    public EndScreen _endScreen = null; // reference to our end scene UI
    public bool _waitingToBegin = false; // are we waiting for player input?

    public GameObject _playerPrefab; // our player prefab
    public GameObject _player = null; // our player in the game
    public GameObject _enemyPrefab; // our enemy prefab
    public GameObject _rockPrefab; //our rock prefab
    public GameObject _pickUpPrefab; // our pickup prefab

    public static GameManager instance = null; //make this a singleton

    bool _spawningWave = false; // are we in the middle of a spawning wave?
    void Awake() // set as singleton
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
    }
    void Start() // save references to level manager and sfx manager.
    {
        _levelManager = GetComponent<LevelManager>();
        _sfxManager = GetComponent<sfxManager>();
    }

    void Update()
    {
        if(_waitingToBegin) // if waiting to begin the game
        {
            if(Input.anyKey) // if player presses any button
            {
                _begin.BeginLevel(); // start the game
                _waitingToBegin = false; // turn off our check
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
            _player = Instantiate(_playerPrefab, new Vector3(0, -3, 0), Quaternion.identity);
        }
       else // otherwise turn the player onand in position
        {
            _player.transform.position = new Vector3(0,-3, 0);
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
        _currentLevel += 1; // increase level count
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
        _endScreen._score.text = Save.instance._currentScore.ToString(); // update our end screen UI with score and high score.
        _endScreen._highScore.text = Save.instance._bestScore.ToString();
        _endScreen.gameObject.SetActive(true); // turn end screen UI on.
    }

    public void Retry () // this resets everything when we retry.
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0) // make sure all enemies are dead first.
        {
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) //for each gameobject with the tag Enemy.
            {
                go.GetComponent<Enemy>().Death(); //kill it
            }
        }
        _endScreen._score.text = 0.ToString(); // everything here is just resetting score and level count.
        _endScreen._highScore.text = 0.ToString();
        _endScreen.gameObject.SetActive(false);
        Save.instance._currentScore = 0;
        Save.instance.UpdateScore(0);
        _currentLevel = 1;
        Save.instance.UpdateLevel(_currentLevel);
        _begin.BeginLevel(); // set our begin script again.
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
                spawnPosition.x = Random.Range(-6f, 6f); // make our spawn position random between -6 and 6.
                if (spawnEnemies)
                {
                    go = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity); 
                }else
                {
                    go = Instantiate(_rockPrefab, spawnPosition, Quaternion.identity);
                }
                yield return new WaitForSeconds(Mathf.Clamp(1f - (_currentLevel * 0.1f), 0.1f, 2)); // this reduces the wait as the game gets harder so more enemies spawn faster.
            }
            waveFrequency -= 1; // reduce wave frequency by 1 after spawning a wave.
            spawnEnemies = !spawnEnemies; // flip spawn enemies. basically says spawnenemies is equal to not spawnenemies. 
            yield return new WaitForSeconds(Mathf.Clamp(2f - (_currentLevel * 0.1f), 0.1f, 2)); // wait to spawn next wave, also gets shorter time.

            spawnPosition.x = Random.Range(-6f, 6f); 
            SpawnPickUp(spawnPosition); // at the end of a wave spawn a powerup.
        }

        while(GameObject.FindGameObjectsWithTag("Enemy").Length > 0) // while there are still enemies on screen, don't start the next level.
        {
            
            yield return null;
        }

        spawnPosition.x = Random.Range(-6f, 6f);
        SpawnPickUp(spawnPosition); // spawn another powerup at the end of the level

        _begin.BeginNextLevel(); // start the next level countdown
        _spawningWave = false;
    }

    public void SpawnPickUp (Vector3 pos) // our spawn pickup code. Sets it to a random powerup at pos. 
    {
        GameObject temppickup = Instantiate(_pickUpPrefab, pos, Quaternion.identity);
        int ran = Random.Range(1, 5);
        switch (ran) // using a switch statment to quickly choose the out come of the random number.
        {
            case 1:
                temppickup.GetComponent<PickUp>().UpdatePickUp(PickUp.PickUpType.FireRate);
                break;
            case 2:
                temppickup.GetComponent<PickUp>().UpdatePickUp(PickUp.PickUpType.Health);
                break;
            case 3:
                temppickup.GetComponent<PickUp>().UpdatePickUp(PickUp.PickUpType.ShotUp);
                break;
            case 4:
                temppickup.GetComponent<PickUp>().UpdatePickUp(PickUp.PickUpType.Speed);
                break;
            default:
                break;
        }
    }
}
