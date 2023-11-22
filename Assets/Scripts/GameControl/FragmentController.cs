using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FragmentController : MonoBehaviour
{
    public static FragmentController Instance;

    [Header("Components")]
    public Hashtable _fragments = new Hashtable();

    public void StartFragments()
    {
        Game._instance = SaveLoad._savedGame;

        foreach (Fragment fragment in FindObjectsOfType<Fragment>().ToList())
        {
            _fragments.Add(fragment._fragmentID.ToString(), fragment.gameObject);

            if (fragment._fragmentID < SaveLoad._savedGame._currentFragment)
            {
                fragment._activated = false;
            }
        }
    }
}
