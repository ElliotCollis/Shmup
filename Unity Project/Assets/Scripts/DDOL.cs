using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour // this script means our gamemanager will stay between scenes we load. We only need to use this once.
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // this sets it to not be destroyed when we load a new scene.
        GetComponent<LevelManager>().LoadLevel("MainMenu"); // We only use this in the preload scene so load the first scene after that which is our Main Menu.
    }
}
