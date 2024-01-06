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
        Level temp = (Level)SaveLoad._savedGame.Levels.getNode(LoadingData.PlayingLevel).data;

        //Search for all the fragments on the scene
        foreach (Fragment fragment in FindObjectsOfType<Fragment>().ToList())
        {
            //Save the fragments in the list
            _fragments.Add(fragment._fragmentID.ToString(), fragment.gameObject);

            //If the fragment was already collected before, set it to unactive
            for (int i = 0; i < temp._collectedFragments.Length; i++)
            {
                if (temp._collectedFragments[i] == fragment._fragmentID)
                {
                    fragment._activated = false;
                    fragment.gameObject.SetActive(false);
                }
            }
        }
    }
}
