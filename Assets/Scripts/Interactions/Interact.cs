using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [HideInInspector] public string _layerText;
    public virtual void Interaction()
    {
        Debug.Log("Interacted");
    }
}
