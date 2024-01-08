using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using static Cinemachine.DocumentationSortingAttribute;

public class LoginScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _warningTxt;
    [SerializeField] private TMP_InputField _usernameIF;
    [SerializeField] private TMP_InputField _passwordIF;
    public void Login()
    {
        UserData user = new UserData(_usernameIF.text, _passwordIF.text);
        StartCoroutine(LoginAPI(user));
    }
    public void Pass()
    {
        FindObjectOfType<SceneLoader>().LoadScene("MainMenu");
    }
    public void GoToRegister()
    {
        FindObjectOfType<SceneLoader>().LoadScene("Register");
    }
    public void ClearWarning()
    {
        _warningTxt.text = "";
    }

    IEnumerator LoginAPI(UserData user)
    {
        UnityWebRequest request = new UnityWebRequest(LoadingData.url + $"users/auth?username={user.username}&password={user.password}", "GET");

        string JSONData = JsonUtility.ToJson(user);
        byte[] JSONToSend = new System.Text.UTF8Encoding().GetBytes(JSONData);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(JSONToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            DataFromServer temp = JsonUtility.FromJson<DataFromServer>(request.downloadHandler.text);
            if (request.responseCode != 200)
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

        yield return null;
    }

}
class DataFromServer
{
    public string msg;
    public int user_id;
}
public class UserData
{
    public string username;
    public string password;
    public UserData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}
