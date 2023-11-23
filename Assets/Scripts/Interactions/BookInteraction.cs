using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteraction : Interact
{

    private void Start()
    {
        _layerText = "Take book";
    }
    public override void Interaction()
    {
        Debug.Log("Book");
        this.gameObject.SetActive(false);
    }
}
