using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Save : MonoBehaviour { // a very simple save system.

	public static Save instance = null; // make it a singleton.

	public TextMeshProUGUI _score; // we also have our score to display
	public TextMeshProUGUI _level;  // and level here.
	public int _currentScore = 0;
	public int _bestScore;

	void Start () {

		if(instance == null) instance = this;
		else if(instance != this) Destroy(this.gameObject);

		_score.text = _currentScore.ToString (); // we use tostring to convert an int or float to a string.
		UpdateLevel(1); // we update our showen level and say we start at level 1.
	}

	public void UpdateScore (int points) //we will increase the current score by 1;
	{
		if (PlayerPrefs.HasKey ("Score")) { // here we check if we already have saved something to the key "Score", if not we won't touch it, if we have, get the best score.
			_bestScore = loadScore (); // we use load score to get our score here.
		}

		//increase by one
		_currentScore += points; 

		//update the UI
		_score.text = _currentScore.ToString ();

		//Check if it's our best score and save it.
		if (_currentScore > _bestScore) {
			saveScore ();
		}
	}

	public void UpdateLevel (int level)
	{
		_level.text = level.ToString(); // set the level text
	}

	void saveScore () 
	{
		PlayerPrefs.SetInt ("Score", _currentScore); // save our score to player preferences as an int.
	}

	int loadScore ()
	{
		return PlayerPrefs.GetInt ("Score"); // get our score to player preferences as an int.
	}
}