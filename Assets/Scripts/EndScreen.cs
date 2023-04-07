using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // use the text mesh pro namesspace

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI score; // referenve to our score UI text
    public TextMeshProUGUI highScore;

    void Awake()
    {
        GameManager.instance.endScreen = this; // make sure our gamemanger has access to this.
        gameObject.SetActive(false); // turn this object off.
    }

    public void Retry ()
    {
        GameManager.instance.Retry(); // if we hit the rety button start again.
    }

    public void MainMenu ()
    {
        GameManager.instance.levelManager.UnPause(); // you always need to unpause before changing level!!
        GameManager.instance.levelManager.LoadLevel("MainMenu");
    }
}
