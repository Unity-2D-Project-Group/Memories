using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBlockActivator : MonoBehaviour
{
    [Header("Components")]
    private AngryBlock _angryBlock;

    private void Start()
    {
        //Get the components
        _angryBlock = GetComponentInParent<AngryBlock>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //If it verifys a collision with the player, it falls
        if (collision.gameObject.CompareTag("Player"))
        {
            _angryBlock.ActivateGravity();
        }
    }
}