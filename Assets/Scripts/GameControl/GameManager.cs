using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<GameObject> _petsPrefabs = new List<GameObject>();


    [Header("UI")]
    [SerializeField] private TMP_Text _text;

    [HideInInspector] public bool _typing = false;
    void Start()
    {
        //Start the checkpoints and fragments when the scene is loaded
        this.GetComponent<FragmentController>().StartFragments();
        this.GetComponent<CheckpointController>().StartCheckPoints();

        Time.timeScale = 1.0f;

        //If the player is logged-in and it has a pet, instantiate the pet
        if (LoadingData.LoggedIn && LoadingData.CurrentPetID != -1)
        {
            GameObject pet = Instantiate(_petsPrefabs[LoadingData.CurrentPetID], this.gameObject.transform);
            pet.transform.SetParent(null);
        }
    }

    public IEnumerator Type(string s, string speaker)
    {
        if (!_typing)
        {
            _typing = true;
            _text.text = "";
            string temp = speaker + ": ";

            for (int i = 0; i < s.Length; i++)
            {
                temp += s[i];
                _text.text = temp;
                yield return new WaitForSeconds(0.04f);
            }
            yield return new WaitForSeconds(1f);

            for (int i = temp.Length - 1; i >= 0; i--)
            {
                temp = temp.Remove(i, 1);
                _text.text = temp;
                yield return new WaitForSeconds(0.04f);
            }
            _typing = false;
            yield return null;
        }
    }
}
