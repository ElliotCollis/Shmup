using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour // main menu functions for our buttons.
{
    CanvasManager _CM;
    public GameObject _MainCanvas; //save references to our canvases.
    public GameObject _OptionsCanvas;
    void Start()
    {
        _CM = GetComponent<CanvasManager>(); // get our canvas manager to swap canvases easily.
        _CM.SwapCanvas(_OptionsCanvas, _MainCanvas);// make sure our main menu canvas is on.

    }

    public void Play()
    {
        GameManager.instance._levelManager.LoadLevel("Game"); // load our game. We get our levelmanager through our instance of game manager here.
    }

    public void Options()
    {
        _CM.SwapCanvas(_MainCanvas, _OptionsCanvas); // go to options canvas
    }

    public void Exit()
    {
        Application.Quit(); // quit the game
    }

    public void Back ()
    {
        _CM.SwapCanvas(_OptionsCanvas, _MainCanvas); // go back from options to main menu canvas.
    }
}
