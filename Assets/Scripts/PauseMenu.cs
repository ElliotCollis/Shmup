using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour // similar to our main menu
{
    CanvasManager _canvasManager;
    [SerializeField] private GameObject _PauseCanvas;
    [SerializeField] private GameObject _OptionsCanvas;
    void Awake()
    {
        _canvasManager = GetComponent<CanvasManager>();
        _canvasManager.SwapCanvas(_OptionsCanvas, _PauseCanvas);

        this.gameObject.SetActive(false); // make sure this is turned off when we start the game.
    }

    public void Resume ()
    {
        GameManager.instance.levelManager.UnPause(); // start our game back up, the levelmanage manages our timescale instead of here.
    }

    public void Options ()
    {
        _canvasManager.SwapCanvas(_PauseCanvas, _OptionsCanvas); // same as main menu
    }

    public void Mainmenu ()
    {
        GameManager.instance.levelManager.UnPause(); // you always need to unpause before changing level!!
        GameManager.instance.levelManager.LoadLevel("MainMenu"); // change level to main menu.
    }

    public void Back ()
    {
        _canvasManager.SwapCanvas(_OptionsCanvas, _PauseCanvas); // same as main menu
    }
}
