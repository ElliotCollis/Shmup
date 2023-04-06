using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour //Anything that leaves the boundary will be destroyed to save memory.
{
    private void OnTriggerExit2D(Collider2D collision) // when a trigger leaves
    {
        if (!collision.CompareTag("Player")) //if it's not the player. Just in case something goes wrong.
        {
            Destroy(collision.gameObject, 0.1f); // destroy it after 0.1 seconds.
        }
    }
}
