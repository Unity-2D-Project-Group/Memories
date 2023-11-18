using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AngryBlockActivator : MonoBehaviour
{
    private GameObject _angryBlock;

    private void Start()
    {
        _angryBlock = GameObject.FindGameObjectWithTag("AngryBlock");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _angryBlock.gameObject.GetComponent<AngryBlocks>().ActivateGravity();
        }
    }
}