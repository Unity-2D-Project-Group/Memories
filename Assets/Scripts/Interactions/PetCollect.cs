using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetCollect : Interact
{
    [SerializeField] private int _petCollectID;
    private void Start()
    {
        _layerText = "Collect Pet"; 
    }


    public override void Interaction()
    {
        // We should change this to the database input on 3rd delivery
        LoadingData.CurrentPetID = _petCollectID;
        print("Player collected the pet");
        this.gameObject.SetActive(false);
    }
}
