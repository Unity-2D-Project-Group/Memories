using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _petsPrefabs = new List<GameObject>();
    void Start()
    {
        this.GetComponent<FragmentController>().StartFragments();
        this.GetComponent<CheckpointController>().StartCheckPoints();
        Time.timeScale = 1.0f;
        if (LoadingData.LoggedIn && LoadingData.CurrentPetID != -1)
        {
            GameObject pet = Instantiate(_petsPrefabs[LoadingData.CurrentPetID], this.gameObject.transform);
            pet.transform.SetParent(null);
        }
    }
}
