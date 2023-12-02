using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PetCollect : Interact
{
    [SerializeField] private int _petCollectID;
    private void Start()
    {
        _layerText = "Collect Pet";
        if (!LoadingData.LoggedIn) { this.gameObject.SetActive(false); }
    }

    private void FixedUpdate()
    {
        foreach (PetController temp in FindObjectsByType<PetController>(FindObjectsSortMode.InstanceID))
        {
            if (temp._petID == _petCollectID)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    public override void Interaction()
    {
        print("Player collected the pet");
        // We should change this to the database input on 3rd delivery
        LoadingData.CurrentPetID = _petCollectID;
        this.gameObject.SetActive(false);
    }
}
