using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteraction : Interact
{
    public override void Interaction()
    {
        Debug.Log("Book");
        this.gameObject.SetActive(false);
    }
}
