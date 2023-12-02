using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    //That's the parent of the other interactable objects, all the other interactables will inherit the variables and methods from here
    [HideInInspector] public string _layerText;
    public virtual void Interaction()
    {
        Debug.Log("Interacted");
    }
}
