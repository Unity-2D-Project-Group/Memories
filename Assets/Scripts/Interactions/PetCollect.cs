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
        if (!LoadingData.LoggedIn || LoadingData.CurrentPet.id == _petCollectID) { this.gameObject.SetActive(false); }
    }

    public override void Interaction()
    {
        print("Player collected the pet");
        // We should change this to the database input on 3rd delivery
        //LoadingData.CurrentPetID = _petCollectID;
        StartCoroutine(AddPet());
        this.gameObject.SetActive(false);
    }

    IEnumerator AddPet()
    {
        UnityWebRequest request = new UnityWebRequest(LoadingData.url + "pets/", "POST");

        string JSONData = JsonUtility.ToJson(new PetToAdd(_petCollectID));
        byte[] JSONToSend = new System.Text.UTF8Encoding().GetBytes(JSONData);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(JSONToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error: " + request.error);
        }
        yield return null;
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
