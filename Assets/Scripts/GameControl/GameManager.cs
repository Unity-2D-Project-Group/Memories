using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<GameObject> _petsPrefabs = new List<GameObject>();
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
}
