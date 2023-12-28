using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject[] _petsPrefabs;
    [SerializeField] private bool _summoned = false;


    [Header("UI")]
    [SerializeField] private TMP_Text _text;

    [HideInInspector] public bool _typing = false;

    private void Awake()
    {
        //If the player is logged-in and it has a pet, Loads the pet info
        if (LoadingData.LoggedIn)
        {
            StartCoroutine(SearchPetInfo());
        }
    }
    void Start()
    {
        //Start the checkpoints and fragments when the scene is loaded
        this.GetComponent<FragmentController>().StartFragments();
        this.GetComponent<CheckpointController>().StartCheckPoints();

        Time.timeScale = 1.0f;
    }

    void FixedUpdate()
    {
        if (LoadingData.LoggedIn && LoadingData.CurrentPet.id != 0 && !_summoned)
        {
            GameObject pet = Instantiate(_petsPrefabs[LoadingData.CurrentPet.id - 1], this.gameObject.transform);
            pet.transform.SetParent(null);
            _summoned = true;
        }
    }
    IEnumerator SearchPetInfo()
    {
        UnityWebRequest request = new UnityWebRequest(LoadingData.url + "pets/current", "GET");

        string JSONData = JsonUtility.ToJson(LoadingData.PlayerUserObj);
        byte[] JSONToSend = new System.Text.UTF8Encoding().GetBytes(JSONData);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(JSONToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            LoadingData.CurrentPet = JsonUtility.FromJson<Pet>(request.downloadHandler.text);
        }
        yield return null;
    }
    public IEnumerator Type(string s, string speaker)
    {
        if (!_typing)
        {
            _typing = true;
            _text.text = "";
            string temp = speaker + ": ";

            for (int i = 0; i < s.Length; i++)
            {
                temp += s[i];
                _text.text = temp;
                yield return new WaitForSeconds(0.04f);
            }
            yield return new WaitForSeconds(1f);

            for (int i = temp.Length - 1; i >= 0; i--)
            {
                temp = temp.Remove(i, 1);
                _text.text = temp;
                yield return new WaitForSeconds(0.04f);
            }
            _typing = false;
            yield return null;
        }
    }
}
