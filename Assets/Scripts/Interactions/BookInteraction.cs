using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteraction : Interact
{
    void Interact()
    {
        Debug.Log("Book");
        this.gameObject.SetActive(false);
    }
}
