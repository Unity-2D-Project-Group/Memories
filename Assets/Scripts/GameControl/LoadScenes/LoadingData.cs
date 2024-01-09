using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class LoadingData
{
    public static string url = "http://memories.games3.iade.xyz/";
    public static string SceneToBeLoaded;
    public static string SceneToBeUnloaded;
    public static string LoadingScene = "LoadingScene";
    public static bool LoadingSelection = false;
    public static int PlayingLevel;

    public static bool LoggedIn = false;
    public static User PlayerUserObj;

    public static Pet CurrentPet;
}