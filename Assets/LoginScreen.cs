using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LoginScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _warningTxt;
    [SerializeField] private TMP_InputField _usernameIF;
    [SerializeField] private TMP_InputField _passwordIF;
    public void Login()
    {
        if(_usernameIF.text == "me" && _passwordIF.text == "123")
        {
            LoadingData.LoggedIn = true;
            FindObjectOfType<SceneLoader>().LoadScene("MainMenuScene");
        }
        else
        {
            _warningTxt.text = "Invalid Credentials";
        }
    }
    public void Pass()
    {
        FindObjectOfType<SceneLoader>().LoadScene("MainMenuScene");
    }

    public void ClearWarning()
    {
        _warningTxt.text = "";
    }
}
