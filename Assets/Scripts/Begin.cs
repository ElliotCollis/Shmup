using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Begin : MonoBehaviour // this controls the begining of the game and the UI between levels.
{
    // Different UI canvases for each state.
    [SerializeField] private GameObject _nextLevel;
    [SerializeField] private GameObject _pressToStart;
    [SerializeField] private GameObject _countdown;

    // checks for each state, this is a very rough way to do different states that should only be used when you have verr few. An althernative is Enums or FSM.
    bool beginCountdown = false;
    bool beginNextCountdown = false;

    void Awake()
    {
        // turn off all our canvases
        _nextLevel.SetActive(false);
        _pressToStart.SetActive(false);
        _countdown.SetActive(false);

        OpenBeginMenu();
    }

    public void OpenBeginMenu () // when we begin the game
    {
       _pressToStart.SetActive(true); // turn our press to start canvas on.

        GameManager.instance.begin = this; // make sure our gamemanager knows 
        GameManager.instance.waitingToBegin = true; // turn on our wait for input check.
    }

    public void BeginLevel () // start our countdown for begin level
    {
        if (!beginCountdown)
            StartCoroutine("BeginLevelCountdown");
    }

    public void BeginNextLevel () // start our countdown between levels.
    {
        if (!beginNextCountdown)
            StartCoroutine("BeginNextLevelCountdown");
    }

    IEnumerator BeginLevelCountdown () 
    {
        beginCountdown = true;
        //sets up the game scene
        GameManager.instance.PopulateObjectPools(); // runs init on all the object pools
        PlayerController player = GameManager.instance.SpawnPlayer(); // get the player
        player.inControl = true; // make sure the player is in control
        player.GetComponent<PlayerHealth>().StartIFrames(); // give the player some starting invincability just in case.

        _pressToStart.SetActive(false); //turn off the press to start UI
        _countdown.SetActive(true); // turn on the countdown UI

        //counts down with UI
        TextMeshProUGUI countdown = _countdown.GetComponent<TextMeshProUGUI>(); //save a quic reference to make things easy
        countdown.text = 3.ToString(); // count down
        yield return new WaitForSeconds(1f); // and wait between each number

        countdown.text = 2.ToString();
        yield return new WaitForSeconds(1f);

        countdown.text = 1.ToString();
        yield return new WaitForSeconds(1f);

        countdown.text = "GO!"; //GO!!!
        yield return new WaitForSeconds(1f);

        //starts the game
        _countdown.SetActive(false); //turn off countdown UI
        GameManager.instance.StartGame(); //start the game spawning enemies
        beginCountdown = false;
    }

    IEnumerator BeginNextLevelCountdown()
    {
        beginNextCountdown = true;
        // our between level countdown
        TextMeshProUGUI nextlvl = _nextLevel.GetComponent<TextMeshProUGUI>(); // save a reference to make things easy
        nextlvl.text = "NEXT LEVEL"; // same as countdown
        _nextLevel.SetActive(true);
        int waitTime = 1;

        yield return new WaitForSeconds(waitTime);
        nextlvl.text = "GET READY!";

        yield return new WaitForSeconds(waitTime);
        nextlvl.text = "GO!";

        yield return new WaitForSeconds(waitTime);
        _nextLevel.SetActive(false);

        GameManager.instance.StartNextLevel(); // start the next level
        beginNextCountdown = false;
    }

}
