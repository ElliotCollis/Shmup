using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour // simple script for swapping UI canvases.
{
    public void SwapCanvas (GameObject TurnOff, GameObject TurnOn) // takes in one canvas to turn off and one to turn on.
    {
        TurnOff?.SetActive(false);
        TurnOn?.SetActive(true);
    }
}
