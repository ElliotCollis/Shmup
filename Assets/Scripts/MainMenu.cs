using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour // main menu functions for our buttons.
{
    private CanvasManager _canvasManager;
    [SerializeField] private GameObject _mainCanvas; //save references to our canvases.
    [SerializeField] private GameObject _OptionsCanvas;

    void Start()
    {
        _canvasManager = GetComponent<CanvasManager>(); // get our canvas manager to swap canvases easily.
        _canvasManager.SwapCanvas(_OptionsCanvas, _mainCanvas);// make sure our main menu canvas is on.

    }

    public void Play()
    {
        GameManager.instance.levelManager.LoadLevel("Game"); // load our game. We get our levelmanager through our instance of game manager here.
    }

    public void Options()
    {
        _canvasManager.SwapCanvas(_mainCanvas, _OptionsCanvas); // go to options canvas
    }

    public void Exit()
    {
        Application.Quit(); // quit the game
    }

    public void Back ()
    {
        _canvasManager.SwapCanvas(_OptionsCanvas, _mainCanvas); // go back from options to main menu canvas.
    }
}
