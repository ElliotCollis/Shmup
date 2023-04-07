using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour // not used in this project, but good to have and learn how to do. Object pooling saves memory by recycling objects instead of creating and destroying them.
{
	public GameObject pooledObject; // the object we're going to pool
	public int pooledAmount = 20; // how many objects we start with
	public bool willGrow = true; // can the above amount grow or is it limited.

	private List<GameObject> _pooledObjects; // create a list to save all our objects in.

	public List<GameObject> pooledObjects => _pooledObjects; // public getter for our pooled objects.

	void Start()
	{
		_pooledObjects = new List<GameObject>(pooledAmount); // set the list empty. 
		for (int i = 0; i < pooledAmount; i++) // use a for loop to fill it up. This runs the code inside the number of pooled amount times.
		{
			GameObject obj = (GameObject)Instantiate(pooledObject); // create the gameobject. 
			obj.SetActive(false); // turn it off.
			_pooledObjects.Add(obj); // add it to our list.
		}
	}

	public GameObject GetPooledObject() // returns the first avaliable object, or creates on if possible.
	{
		for (int i = 0; i < _pooledObjects.Count; i++) // run through the list of objects. 
		{
			if (!_pooledObjects[i].activeInHierarchy) // if it's currently turned off.
			{
				return _pooledObjects[i]; // return the object.
			}
		}

		if (willGrow) // if the list can grow, create a new object and return that.
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.SetActive(false);
			_pooledObjects.Add(obj);
			return obj;
		}

		return null; // otherwise return nothing.
	}
}
