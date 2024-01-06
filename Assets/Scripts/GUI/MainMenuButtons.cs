using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Button _loadSaveButton;
    [SerializeField] private GameObject _loginSection;
    [SerializeField] private GameObject _authSection;
    [SerializeField] private GameObject _playScreen;
    [SerializeField] private GameObject _selectScreen;
    [SerializeField] private TMP_Text _usernameTxt;
    void Start()
    {
        if (LoadingData.LoadingSelection)
        {
            _playScreen.SetActive(false);
            _selectScreen.SetActive(true);
            LoadingData.LoadingSelection = false;
        }
        SaveLoad.LoadSave();
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "GameSave.txt")) || SaveLoad._savedGame._currentLevel == 0)
        {
            _loadSaveButton.interactable = false;
        }
        if (LoadingData.LoggedIn)
        {
            _loginSection.SetActive(false);
            _authSection.SetActive(true);
            _usernameTxt.text = LoadingData.PlayerUserObj.username;
        }
    }
        public void NewGame()
    {
        SaveLoad.NewSave();
    }
    public void LoadGame()
    {
        SaveLoad.LoadSave();
    }
    public void CloseApp()
    {
        Application.Quit();
    }
    public void LoginButton()
    {
        FindObjectOfType<SceneLoader>().LoadScene("Login");
    }
    public void LogoutButton()
    {
        LoadingData.PlayerUserObj = null;
        LoadingData.CurrentPet = null;
        LoadingData.LoggedIn = false;
        _loginSection.SetActive(true);
        _authSection.SetActive(false);
    }
    public void RegisterButton()
    {
        FindObjectOfType<SceneLoader>().LoadScene("Register");
    }
}