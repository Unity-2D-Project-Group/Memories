using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using static Cinemachine.DocumentationSortingAttribute;

public class RegisterScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _warningTxt;
    [SerializeField] private TMP_InputField _usernameIF;
    [SerializeField] private TMP_InputField _passwordIF;

    public void Register()
    {
        StartCoroutine(RegisterAPI(new UserData(_usernameIF.text, _passwordIF.text)));
    }
    public void Pass()
    {
        FindObjectOfType<SceneLoader>().LoadScene("MainMenu");
    }

    public void GoToLoginPage()
    {
        FindObjectOfType<SceneLoader>().LoadScene("Login");
    }
    public void ClearWarning()
    {
        _warningTxt.text = "";
    }

    IEnumerator RegisterAPI(UserData user)
    {
        UnityWebRequest request = new UnityWebRequest(LoadingData.url + "users/auth", "POST");

        string JSONData = JsonUtility.ToJson(user);
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
            DataFromServer temp = JsonUtility.FromJson<DataFromServer>(request.downloadHandler.text);
            if(request.responseCode != 200)
            {
                _warningTxt.text = temp.msg;
            }
            else
            {
                LoadingData.LoggedIn = true;

                LoadingData.PlayerUserObj = new User(temp.user_id, user.username);
                FindObjectOfType<SceneLoader>().LoadScene("MainMenu");
            }
        }
    }
}