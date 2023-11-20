using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<FragmentController>().StartFragments();
        this.GetComponent<CheckpointController>().StartCheckPoints();
        Time.timeScale = 1.0f;
    }
}
