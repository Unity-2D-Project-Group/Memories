using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
    }

    private void FixedUpdate()
    {
        if (LoadingData.CurrentPet != null && LoadingData.CurrentPet.pet_id > 0 && !_summoned)
        {
            GameObject pet = Instantiate(_petsPrefabs[LoadingData.CurrentPet.pet_id - 1], this.gameObject.transform);
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
            if(JsonUtility.FromJson<Pet>(request.downloadHandler.text) != null)
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
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(0.7f);

            for (int i = temp.Length - 1; i >= 0; i--)
            {
                temp = temp.Remove(i, 1);
                _text.text = temp;
                yield return new WaitForSeconds(0.01f);
            }
            _typing = false;
            yield return null;
        }
    }
}

public class User
{
    public string username;
    public int user_id;
    public User(int id, string name)
    {
        this.user_id = id;
        this.username = name;
    }
}
public class Pet
{
    public int id;
    public string name;
    public int user_id;
    public int pet_id;
    public int hungry;
    public int happiness;
    public int hygiene;
    public string state;
    public string humor;
}