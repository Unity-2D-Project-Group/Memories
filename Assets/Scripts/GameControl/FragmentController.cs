using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FragmentController : MonoBehaviour
{
    public static FragmentController Instance;
    //private GameObject _player;

    [Header("Components")]
    [SerializeField] private List<Fragment> _fragments = new List<Fragment>();

    public void StartFragments()
    {
        Game._instance = SaveLoad._savedGame;

        _fragments = FindObjectsByType<Fragment>(FindObjectsSortMode.InstanceID).ToList();

        if (SaveLoad._savedGame._currentFragment >= 1 && _fragments.Count > 0)
        {
            foreach (Fragment fragment in _fragments)
            {
                if (fragment._fragmentID <= SaveLoad._savedGame._currentFragment)
                {
                    fragment._activated = false;
                    fragment.gameObject.SetActive(false);
                }
            }
        }
    }
}
