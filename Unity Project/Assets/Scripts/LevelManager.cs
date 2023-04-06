using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // access the scenemanagement name space to change scenes.

public class LevelManager : MonoBehaviour // this manages our scenes and pause time
{
    public GameObject _pauseMenu = null; //save a reference to our pause menu canvas

    bool paused = false; // a reference if we're paused or not.

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) // if we press R we want to restart the game.
        {
            LoadLevel(SceneManager.GetActiveScene().buildIndex); // load this scene with the same build index.
        }

        if(Input.GetKeyDown(KeyCode.Escape)) // if we hit escape we want to pause or unpause the game.
        {
            if(SceneManager.GetActiveScene().buildIndex != 1) // meaning not our main menu! Can add extras here for scenes you don't want to pause.
            {
                if (_pauseMenu != null) // make sure the pause menu is set first
                {
                    if (paused) // if paused then unpause
                    {
                        UnPause();
                    }
                    else // or if not pause then pause.
                    {
                        Pause();
                    }
                }
            }
        }
    }

    public void LoadLevel (int LvlID) // here we're loading a scene by it's build index, used for restart.
    {
         SceneManager.LoadScene(LvlID);
    }

    public void LoadLevel(string LvlID) // here we're loading a scene by it's name, for everything else.
    {
        SceneManager.LoadScene(LvlID);
    }

    private void Pause () // when we pause the game
    {
        _pauseMenu.SetActive(true); // turn on the pause menu
        paused = true; // and our check
        Time.timeScale = 0; // turn timescale to 0 so no time bases.
    }

    public void UnPause () // when we unpause the game
    {
        Time.timeScale = 1; // set the timescale back to 1 first
        _pauseMenu.GetComponent<PauseMenu>().Back(); // make sure our pausemenu is on it's main page.
        _pauseMenu.SetActive(false); // turn it off
        paused = false; // save our check.
    }
}
