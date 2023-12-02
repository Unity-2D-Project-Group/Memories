using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FragmentController : MonoBehaviour
{
    public static FragmentController _instance;

    [Header("Components")]
    public Hashtable _fragments = new Hashtable();

    public void StartFragments()
    {
        Save._instance = SaveLoad._savedGame;
        //Get the current level info
        Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];

        //Search for all the fragments on the scene
        foreach (Fragment fragment in FindObjectsOfType<Fragment>().ToList())
        {
            //Save the fragments in the list
            _fragments.Add(fragment._fragmentID.ToString(), fragment.gameObject);

            //If the fragment was already collected before, set it to unactive
            if (fragment._fragmentID < temp._currentFragment)
            {
                fragment._activated = false;
            }
        }
    }
}
