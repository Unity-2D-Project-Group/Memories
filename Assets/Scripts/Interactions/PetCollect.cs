using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PetCollect : Interact
{
    [SerializeField] private int _petCollectID;
    [SerializeField] private GameObject _petPrefab;
    private void Start()
    {
        _layerText = "Collect Pet";
    }

    public override void Interaction()
    {
        StartCoroutine(AddPet());
    }

    private void FixedUpdate()
    {
        if (!LoadingData.LoggedIn || LoadingData.CurrentPet != null && LoadingData.CurrentPet.pet_id > 0 && LoadingData.CurrentPet.pet_id == _petCollectID) { this.gameObject.SetActive(false); }
    }

    IEnumerator AddPet()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", LoadingData.PlayerUserObj.user_id);
        form.AddField("pet_id", _petCollectID);

        UnityWebRequest request = UnityWebRequest.Post(LoadingData.url + "pets/", form);
        yield return request.SendWebRequest();

        
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("Player collected the pet");

            this.gameObject.SetActive(false);
        }
    }
}

class PetToAdd
{
    public int pet_id;

    public PetToAdd(int id)
    {
        pet_id = id;
    }
}
