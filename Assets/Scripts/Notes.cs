using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour // simple script you can attach to a gameobject to write notes for yourself or other team members.
{
    [TextArea(50,20)] // sets the property size of the string
    public string notes;
}
