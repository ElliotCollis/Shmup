using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : MonoBehaviour // This is a controller for our player health icons that acts like the object pooling.
{
    [SerializeField] private GameObject _healthIconPrefab; // the prefab to use.
    [SerializeField] private Vector2 _spawnPosition; // it's start position
    [SerializeField] private float _positionOffest = 530f; // offset in UI space
    [SerializeField] private bool _spawnLeft = true; // Are we spawning left or right.
    private List<GameObject> _healthIcons = new List<GameObject>(6); // the list we'll use for our health icons.

    public void UpdateHealthUI(int health) // this gets called from the player health manager.
    {
        foreach(GameObject go in _healthIcons) // everytime we update we turn all our icons off first.
        {
            go.SetActive(false);
        }

        Vector2 lastposition = _spawnPosition; // save a reference of our start spawn position so we can adjust last position through our for statment

        for (int i = 0; i < health; i++) // run a for loop for the amount of health we currently have.
        {
            if (_healthIcons.Count > i) // if we have enough icons
            {
                _healthIcons[i].GetComponent<RectTransform>().localPosition = lastposition; // make sure the position is correct
                _healthIcons[i].SetActive(true); // then turn it on
            }
            else // otherwise create a new one.
            {
                GameObject temp = Instantiate(_healthIconPrefab, Vector3.zero, Quaternion.identity, this.transform);
                temp.GetComponent<RectTransform>().localPosition = lastposition;
                _healthIcons.Add(temp);
            }

            if(_spawnLeft) // if we're moving left or right, adjust the last position by the correct offset.
            {
                lastposition.x -= _positionOffest;
            }else
            {
                lastposition.x += _positionOffest;
            }
        }
    }
}
