using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoadingData
{
    public static string url = "localhost:5000/";
    public static string SceneToBeLoaded;
    public static string SceneToBeUnloaded;
    public static string LoadingScene = "LoadingScene";
    public static int PlayingLevel;

    public static bool LoggedIn = false;
    public static User PlayerUserObj = null;

    public static Pet CurrentPet = null;
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
    public int hungry;
    public int happiness;
    public int hygiene;
    public string state;
    public string humor;
}