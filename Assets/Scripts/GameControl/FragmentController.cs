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
        Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{LoadingData.PlayingLevel}"];
        foreach (Fragment fragment in FindObjectsOfType<Fragment>().ToList())
        {
            _fragments.Add(fragment._fragmentID.ToString(), fragment.gameObject);

            if (fragment._fragmentID < temp._currentFragment)
            {
                fragment._activated = false;
            }
        }
    }
}
